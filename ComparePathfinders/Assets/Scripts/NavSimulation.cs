using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Diagnostics;

public class NavSimulation : MonoBehaviour {

	public NavMeshAgent agent;
	public Transform target;
	public WorldGenerator worldGenerator;

	public int seed = 0;

	float[,] world;

	public int numberOfSimulations;
	private Vector3[] startPositions;
	private Vector3[] targetPositions;
	// Use this for initialization

	void Start () {

		Random.InitState(seed);
		world = worldGenerator.World;

		prepareSimulations(world.GetLength(0), world.GetLength(1));
		runSimulations();
		
	}

	void runSimulations() {
		Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();

		for(int i=0;i<numberOfSimulations;i++) {
			agent.transform.position = startPositions[i];
			target.transform.position = targetPositions[i];
			NavMeshPath path = new NavMeshPath();
			if(agent.CalculatePath(target.position, path)) {
				// print("Path calculated between " + startPositions[i].ToString() + " and " + targetPositions[i].ToString() + "!");
			} else {
				// print("Path could not be found between " + startPositions[i].ToString() + " and " + targetPositions[i].ToString() + "!");
			}
		}
		watch.Stop();
		print("The simulation took: " + watch.ElapsedMilliseconds + " milliseconds");
	}

	void prepareSimulations(int xSize, int zSize) {
		startPositions = new Vector3[numberOfSimulations];
		targetPositions = new Vector3[numberOfSimulations];

		for(int i=0;i<numberOfSimulations;i++) {
			// Start
			int x = Random.Range(0, xSize-1);
			int z = Random.Range(0, zSize-1);
			startPositions[i] = new Vector3(x, world[x,z], z);

			// Target
			x = Random.Range(0, xSize-1);
			z = Random.Range(0, zSize-1);
			targetPositions[i] = new Vector3(x, world[x,z], z);
		}
	}
}
