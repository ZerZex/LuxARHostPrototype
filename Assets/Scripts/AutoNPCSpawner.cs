using UnityEngine;
using UnityEngine.Networking;

public class AutoNPCSpawner : NetworkBehaviour {
	public GameObject autoNPCPrefab;
	public int numberOfNPCs;
	public int spawnGroupSize = 10;  // How many to spawn at once
	public float waitBeforeSpawn = 5.0f;
	public float spawnGroupWait = 5.0f;

	private int currentNPCCount = 0;

	public override void OnStartServer()
	{
		// Rather than spawn all at once, we do a few at a time.
		InvokeRepeating ("SpawnNPCs", waitBeforeSpawn, spawnGroupWait);
	}

	void SpawnNPCs () {
		// Randomly spawns a set number of NPC's until we have reached a limit
		for (int i = 0; i < spawnGroupSize; i++) {
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
			currentNPCCount++;
		}
		if (currentNPCCount >= numberOfNPCs) {
			CancelInvoke ("SpawnNPCs");
		}
	}
}


