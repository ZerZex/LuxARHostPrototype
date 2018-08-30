using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour {

	public Transform goal;
	public float speed = 1.0f;   // units per second
	public bool moveToGoal = false; // Navigation mode. If false, position is updated directly.

	private Transform initialLocation;
	private float startTime;  // Time when movement started
	private float journeyLength;

	// Use this for initialization
	void Start () {
		ResetMotion ();
	}

	public void ResetMotion () {
		// Call whenever goal position changes.
		initialLocation = transform;
		startTime = Time.time;
		journeyLength = Vector3.Distance (initialLocation.position, goal.position);	
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
}
