using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugNavSimulation : MonoBehaviour {

	public Vector3 start;
	public Vector3 goal;

	DebugWorldGeneratorTiled worldGeneratorTiled;
	int[,] world;
	AStarPathfinder aStar;

	bool run;

	void Awake() {
		worldGeneratorTiled = GetComponent(typeof(WorldGeneratorTiled)) as WorldGeneratorTiled;

		run = true;
	}

	void Start() {
		world = worldGeneratorTiled.World;
		aStar = new AStarPathfinder (world);
	}
	
	// Update is called once per frame
	void Update () {
		if (run) {
			aStar.Setup (start, goal);
			aStar.CalculatePath ();

			run = false;
		}
	}
}
