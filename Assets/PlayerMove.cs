using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

	public Transform goal;
	public float speed = 1.0f;   // units per second

	private Transform initialLocation;
	private float startTime;  // Time when movement started
	private float journeyLength;

	// Use this for initialization
	void Start () {
		ResetMotion ();
	}

	void ResetMotion () {
		// Call whenever goal position changes.
		initialLocation = transform;
		startTime = Time.time;
		journeyLength = Vector3.Distance (initialLocation.position, goal.position);	
	}


	// Update is called once per frame
	void Update () {

		// Testing - update pos with Mouse click
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;

			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
				goal.position = hit.point;
				ResetMotion ();
			}
		}

		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;

		transform.position = Vector3.Lerp (initialLocation.position, goal.position, fracJourney);
	}
}
