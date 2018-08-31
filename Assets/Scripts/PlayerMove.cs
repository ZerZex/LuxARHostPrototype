using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour {

	public float speed = 1.0f;   // units per second
	public GameObject bulletPrefab;
	public Transform bulletSpawn;

	private Transform initialLocation;
	private float startTime;  // Time when movement started
	private float journeyLength;

	// Use this for initialization
	void Start () {
		ResetMotion ();
	}

	public void ResetMotion () {
		// Call whenever goal position changes.
		startTime = Time.time;
	}


	// Update is called once per frame
	void Update () {
		// Change position based on target location

        if (!isLocalPlayer)
        {
            return;
        }

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

				if (Input.GetKeyDown(KeyCode.Space))
				{
					Fire();
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

	void Fire ()
	{
		// Create the bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate (bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

		// Add some velocity
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

		// Destroy after 2 seconds
		Destroy(bullet, 2.0f);
	}
}
