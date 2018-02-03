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

	//////////////////////////////////////////////////
	//some variables for debugging and displaying.
	[Header("For debugging and testing")]
	public float epsilon = 0.99f;
	public bool debugDisplay = true;
    public Weighting weighting = Weighting.None;
    public float fillrate = 0.5f;
	///////////////////////////////////////////////////

	void Awake() {
		worldGeneratorTiled = GetComponent(typeof(DebugWorldGeneratorTiled)) as DebugWorldGeneratorTiled;

		run = true;
	}

	void Start() {
		world = worldGeneratorTiled.World;
		aStar = new AStarPathfinder (world, epsilon, weighting, fillrate, debugDisplay);
	}
	
	// Update is called once per frame
	void Update () {
		if (run) {
			aStar.Setup (start, goal);
			ArrayList path;
			aStar.CalculatePath (out path);

			if(debugDisplay) {
				foreach(Node n in path) {
					CreateCube(n.Position3D);
				}
			}

			run = false;
		}
	}

	void CreateCube(Vector3 position) {
		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		Renderer r = cube.GetComponent<Renderer> ();
		r.material.color = Color.yellow;
		cube.transform.position = position;
		cube.transform.localScale = Vector3.one * 1.2f;
	}
}
