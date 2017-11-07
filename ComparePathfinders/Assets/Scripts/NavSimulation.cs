using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavSimulation : MonoBehaviour {

	public NavMeshAgent agent;
	public Transform target;
	public WorldGenerator worldGenerator;

	// Use this for initialization
	void Start () {
		float[,] world = worldGenerator.World;

		// Position agent and target
		int xSize = world.GetLength(0);
		int zSize = world.GetLength(1);
		print("xSize: " + xSize);
		print("zSize: " + zSize);

		agent.transform.position = new Vector3(1, world[1,1], 1);
		target.transform.position = new Vector3(xSize-2, world[xSize-2, zSize-2], zSize-2);
	}
}
