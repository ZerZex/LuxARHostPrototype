using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour {

	public float speed = 3.0f;   // units per second
	public GameObject bulletPrefab;
	public Transform bulletSpawn;

	private Transform initialLocation;
	private float journeyLength;

	// Use this for initialization
	void Start () {
		ResetMotion ();

        // Might not be the best place to do this, but OK for now.
        Input.location.Start();
        if (Input.location.status == LocationServiceStatus.Failed) {
            Debug.Log("Unable to start GPS tracker");
        }
	}

	public void ResetMotion () {
		// Call whenever goal position changes.

	}

    private void OnDestroy()
    {
        if (Input.location.isEnabledByUser) {
            Input.location.Stop();
            Debug.Log("GPS Stopped");
        }
    }


    // Update is called once per frame
    void Update () {
		// Change position based on target location

        if (!isLocalPlayer)
        {
            return;
        }

        if (Application.isMobilePlatform)
        {
            // Process mobile device input mechanism
            Vector3 dir = Vector3.zero;

            // Assume device held parallel to ground and home button to right

            // Remamp device acceleration axis to game coordinates:
            // 1. XY plane of device mapped onto XZ plane
            // 2. rotated 90 degrees around Y axis
            dir.x = -Input.acceleration.y;
            dir.z = Input.acceleration.x;

            // clamp accel vector to unit
            if (dir.sqrMagnitude > 1) {
                dir.Normalize();
            }
            dir *= Time.deltaTime;
            transform.Translate(dir * speed);


        }
        else
        {
            // Use keyboard/mouse for computers
            var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
            var z = Input.GetAxis("Vertical") * Time.deltaTime * speed;

            transform.Rotate(0, x, 0);
            transform.Translate(0, 0, z);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                CmdFire();
            }
        }
	}

    private void OnGUI()
    {
        // Let's grab some GPS coordinates and dump them out
        if (Input.location.status == LocationServiceStatus.Running)
        {
            string text = string.Format("GPS: {0}, {1}",
                                        Input.location.lastData.latitude,
                                        Input.location.lastData.longitude);

            GUI.Label(new Rect(10, 100, 100, 20), text);
            Debug.Log(text);
        }
        else
        {
            string text = string.Format("GPS Status={0}", Input.location.status.ToString());
            GUI.Label(new Rect(10, 200, 300, 40), text);
        }
    }

    public override void OnStartLocalPlayer()
  {
        //base.OnStartLocalPlayer();
        SetColour(Color.blue);
	}

  public void SetColour (Color newcolour)
	{
		// Sets particle start colour
		GameObject p = this.transform.Find("Player Flare").gameObject;

		if (p) {
			ParticleSystem psys = p.GetComponent<ParticleSystem>();

			var main = psys.main;
			main.startColor = newcolour;
		} else {
			Debug.LogError ("Unable to set colour - Flare object not found.");
		}
	}

    [Command]
	void CmdFire ()
	{
		// Create the bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate (bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

		// Add some velocity
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

        // Spawn the bullet on the clients
        NetworkServer.Spawn(bullet);

		// Destroy after 2 seconds
		Destroy(bullet, 2.0f);
	}
}
