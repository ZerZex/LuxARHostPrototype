using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Server side code for controlling motion of automated non player
// characters
public class AutoNPCMotion : NetworkBehaviour {

	[SyncVar(hook="OnChangeParticleColor")]
	public Color particleColor = Color.green;

    [SyncVar(hook = "OnChangeTrailColor")]
    public Color trailColor = Color.yellow;

    // Temp info for random walk
    private float howFarToMove = 0.0f;
	private Vector3 targetPos;
	private float startTime;
	private float speed = 4.0f;
	private Vector3 startPos;

    //private Color[] colors = {Color.red, Color.green, Color.}

	void Start () {
        // Generate a bright color
        particleColor = Color.HSVToRGB(Random.Range(0.0f, 1.0f),
                                       Random.Range(0.0f, 1.0f),
                                       Random.Range(0.5f, 1.0f));
                                       
        trailColor = particleColor;
        trailColor.a = 1.0f;   // Solid trails only
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

	void OnChangeParticleColor (Color newcolor) {
		particleColor = newcolor;
        SetParticleColor(particleColor);
	}

	public void SetParticleColor (Color newcolor)
	{
		// Sets particle start colour
		GameObject p = this.transform.Find("Player Flare").gameObject;

		if (p) {
			ParticleSystem psys = p.GetComponent<ParticleSystem>();

			var main = psys.main;
			main.startColor = newcolor;
		} else {
			Debug.LogError (string.Format("Unable to set colour - object not found."));
		}
	}

    void OnChangeTrailColor(Color newcolor)
    {
        trailColor = newcolor;
        SetTrailColor(trailColor);
    }

    public void SetTrailColor(Color newcolor)
    {
        // Sets particle start colour
        GameObject p = this.transform.Find("Trail").gameObject;

        if (p)
        {
            TrailRenderer trail = p.GetComponent<TrailRenderer>();

            //var main = psys.main;
            trail.startColor = newcolor;
        }
        else
        {
            Debug.LogError(string.Format("Unable to set trail colour - object not found."));
        }
    }
}
