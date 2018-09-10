using UnityEngine;
using UnityEngine.Networking;

public class AutoNPCSpawner : NetworkBehaviour {
	public GameObject autoNPCPrefab;
	public int numberOfNPCs;

	public override void OnStartServer()
	{
		for (int i = 0; i < numberOfNPCs; i++) {
			var spawnPosition = new Vector3 (
				                    Random.Range (-40.0f, 40.0f),
				                    0.0f,
				                    Random.Range (-40.0f, 40.0f));
			var spawnRotation = Quaternion.Euler (
				                    0.0f,
				                    Random.Range (0, 180),
				                    0.0f);
			var npc = (GameObject.Instantiate (autoNPCPrefab, spawnPosition, spawnRotation));
			NetworkServer.Spawn (npc);
		}
	}
}


