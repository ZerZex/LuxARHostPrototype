using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

	public GameObject prefab;

	// Keep hold of list of players as we create them.
	private List<GameObject> players;

	// Use this for initialization
	void Start () {
		if (prefab == null) {
			Debug.LogError ("Prefab not found.");
		}

		players = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {

		// If space key hit, then spawn a new player at mouse position
		if (Input.GetKeyDown (KeyCode.Space)) {
			RaycastHit hit;

			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 100)) {
				GameObject newPlayer = Spawn (hit.point);

				if (newPlayer) {
					players.Add (newPlayer);
					Debug.Log ("Player " + players.Count + " created.");
					// Randomise particle start colour
					SetPlayerColour (newPlayer, Random.ColorHSV (0f, 1f, 1f, 1f, 0.5f, 1f));
				}
			}
		}

		// Change target location if T key hit
		if (Input.GetKeyDown(KeyCode.T)) {
			RaycastHit hit;

			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {

				//players.ForEach 
				foreach (GameObject go in players) {
					PlayerMove p = go.GetComponent<PlayerMove>(); 
					if (p){
						p.goal.position = hit.point;
						p.ResetMotion ();
					}
				}
			}
		}
	}

	GameObject Spawn (Vector3 spawnPos)
	{
		if (prefab) {
			Debug.Log ("Instantiating player...");
			GameObject newPlayer = GameObject.Instantiate (prefab, spawnPos, Quaternion.identity);

			newPlayer.GetComponent<PlayerMove>().goal = (Transform) GameObject.Find ("Target").transform;

			if (newPlayer.GetComponent<PlayerMove>().goal == null) {
				Debug.LogError ("Target not found for new player");
			}
			return newPlayer;
		} else {
			Debug.LogError ("Player Prefab not found");
			return null;
		}
	}

	void SetPlayerColour (GameObject player, Color newcolour)
	{
		// Randomise particle start colour
		// Note - this should be a method on the Player, ideally, since is peeking inside the
		//        prefab, and thus is poor encapsulation. - TODO: refactor
		GameObject p = player.transform.Find("Player Flare").gameObject;

		if (p) {
			ParticleSystem psys = p.GetComponent<ParticleSystem>();

			var main = psys.main;
			main.startColor = newcolour;
		} else {
			Debug.LogError ("Unable to set colour - Flare object not found.");
		}
	}
}
