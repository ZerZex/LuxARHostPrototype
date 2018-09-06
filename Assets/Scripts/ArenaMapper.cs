using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ArenaMapper : NetworkBehaviour {

	[SyncVar]
	public Vector2 corner0;
	[SyncVar]
	public Vector2 corner1;

	private float latitudeSize;
	private float longitudeSize;
	private Bounds geometryBounds;

	// Use this for initialization
	void Start () {
		// Need to get bounding box of Arena geometry, and map
		// to the GPS coodinates

		latitudeSize = Mathf.Abs (corner1.x - corner0.x);
		longitudeSize = Mathf.Abs (corner1.y - corner0.y);

		Renderer rend = GetComponent<Renderer> ();
		geometryBounds = rend.bounds;

		Debug.Log (string.Format ("Gmin = {0}, {1}, {2}", geometryBounds.min.x, geometryBounds.min.y, geometryBounds.min.z));
		Debug.Log (string.Format ("Gmax = {0}, {1}, {2}", geometryBounds.max.x, geometryBounds.max.y, geometryBounds.max.z));
		Debug.Log (string.Format ("GPS size = {0}, {1}", latitudeSize, longitudeSize));
	}
	
	public Vector3 GPStoArenaCoordinates (float latitude, float longitude)
	{
		// Returns location in arena game coordinates
		float x;
		float z;

		x = ((latitude - corner0.x) * (geometryBounds.size.x / latitudeSize)) + geometryBounds.min.x;
		z = ((longitude - corner0.y) * (geometryBounds.size.z / longitudeSize)) + geometryBounds.min.z;

		return new Vector3 (x, 0, z);
	}
}
