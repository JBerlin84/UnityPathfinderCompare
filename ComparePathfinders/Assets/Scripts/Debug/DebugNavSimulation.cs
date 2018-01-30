using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DebugWorldGeneratorTiled))]
public class DebugNavSimulation : MonoBehaviour {

	public Vector3 start;
	public Vector3 goal;

	DebugWorldGeneratorTiled worldGeneratorTiled;
	int[,] world;
	AStarPathfinder aStar;

	bool run;

	void Awake() {
		worldGeneratorTiled = GetComponent(typeof(DebugWorldGeneratorTiled)) as DebugWorldGeneratorTiled;


		PriorityQueue<int> testQueue = new PriorityQueue<int> ();


		for (int i = 0; i < 25; i++) {
			Node n = new Node (Vector3.one);
			n.FScore = Random.Range (0, 25);
			testQueue.Add (n);
		}



		while (testQueue.Count > 0)
			Debug.Log (testQueue.Remove ());

		Debug.Log (s);

		run = false;
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
