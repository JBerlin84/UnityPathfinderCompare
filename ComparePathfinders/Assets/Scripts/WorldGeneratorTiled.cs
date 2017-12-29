using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneratorTiled : MonoBehaviour {

	public float fillPercentage;
	public int worldWidth;
	public int worldHeight;

	public Transform ground;
	public Transform navigationFloor;
	public Transform obstacle;

	private int[,] world;	// 0 is wall, 1 is passable.

	void Awake() {
		world = new int[worldWidth, worldHeight];

		for(int i=0;i<worldWidth;i++) {
			for(int j=0;j<worldHeight;j++) {
				world[i,j] = Random.Range(0f,1f) < fillPercentage ? 0 : 1;
			}
		}

		navigationFloor.localScale = new Vector3(worldHeight, worldWidth, 0);
		navigationFloor.position = new Vector3(worldWidth/2 - 0.5f, 0, worldHeight/2 - 0.5f);
	}

	// Use this for initialization
	void Start () {

		Transform parent = new GameObject("Map").transform;

		for (int i=0;i<worldWidth;i++) {
			for(int j=0;j<worldHeight;j++) {
				Transform o = Instantiate(world[i,j] == 0 ? ground : obstacle, new Vector3(i,0,j), Quaternion.identity) as Transform;
				o.parent = parent;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
