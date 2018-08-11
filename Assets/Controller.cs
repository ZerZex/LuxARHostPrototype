using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

	public GameObject prefab;

	// Use this for initialization
	void Start () {
		if (prefab == null) {
			Debug.LogError ("Prefab not found.");
		}
	}
	
	// Update is called once per frame
	void Update () {

		// If right mouse button, then spawn a new player
		if (Input.GetMouseButtonDown (2)) {
			RaycastHit hit;

			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 100)) {
			
				if (prefab) {
					Debug.Log ("Instantiating player...");
					GameObject newPlayer = GameObject.Instantiate (prefab, hit.point, Quaternion.identity);
	
					newPlayer.GetComponent<PlayerMove>().goal = (Transform) GameObject.Find ("Target").transform;

					if (newPlayer.GetComponent<PlayerMove>().goal == null) {
						Debug.LogError ("Target not found for new player");
					}
	//				newPlayer.SetActive (true);
				} else {
					Debug.LogError ("Player Prefab not found");
				}
			}
		}
	}
}
