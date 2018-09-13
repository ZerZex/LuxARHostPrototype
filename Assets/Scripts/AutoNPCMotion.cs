using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Server side code for controlling motion of automated non player
// characters
public class AutoNPCMotion : NetworkBehaviour {

	[SyncVar(hook="OnChangeColor")]
	public Color color = Color.green;

	// Temp info for random walk
	private float howFarToMove = 0.0f;
	private Vector3 targetPos;
	private float startTime;
	private float speed = 4.0f;
	private Vector3 startPos;

	void Start () {
		color = Random.ColorHSV ();
		ResetMotion ();
	}
		
	private void ResetMotion () {
		
		howFarToMove = Random.Range (2.0f, 10.0f);
		speed = Random.Range (1.0f, 15.0f);
	
		Vector3 dir = transform.forward * howFarToMove;
		targetPos = transform.position + dir;
		startTime = Time.time;
		startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer) {
			// Not sure this will ever get called, as object should be marked
			// server only. But, safety first.
			Debug.Log("Warning: Unexpected attempt at local player authority on NPC");
			return;
		}

		if (isServer) {
			// Want to move in a random walk
			float distanceSoFar = (Time.time - startTime) * speed;
			float fractionTravelled = distanceSoFar / howFarToMove;

			if (fractionTravelled <= 1.0f) {
				transform.position = Vector3.Lerp (startPos, targetPos, fractionTravelled);
			} else {
				// Change direction
				transform.Rotate (0.0f, Random.Range (-90.0f, 90.0f), 0.0f);
				ResetMotion ();
			}
		}
	}

	void OnChangeColor (Color newcolor) {
		color = newcolor;
		SetComponentColor ("Player Flare", color);
	}

	public void SetComponentColor (string component, Color newcolor)
	{
		// Sets particle start colour
		GameObject p = this.transform.Find(component).gameObject;

		if (p) {
			ParticleSystem psys = p.GetComponent<ParticleSystem>();

			var main = psys.main;
			main.startColor = newcolor;
		} else {
			Debug.LogError (string.Format("Unable to set colour - {0} object not found.", component));
		}
	}
}
