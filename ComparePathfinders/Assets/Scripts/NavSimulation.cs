using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Diagnostics;

public class NavSimulation : MonoBehaviour {

	public NavMeshAgent agent;
	public Transform target;
	public WorldGenerator worldGenerator;
	public WorldGeneratorTiled worldGeneratorTiled;

	public int seed = 0;

	int[,] world;

	public int numberOfSimulations;
	private Vector3[] startPositions;
	private Vector3[] targetPositions;
	// Use this for initialization

	Stopwatch builtInSimulationTimer = new System.Diagnostics.Stopwatch();
	Stopwatch aStarSingleThreadedSimulationTimer = new System.Diagnostics.Stopwatch();
	
	Stopwatch aStarSingleThreadedSimulationTimerOLD = new System.Diagnostics.Stopwatch();
	int simulationsRunSoFar = 0;

	bool builtInSimulatorFinished = false;
	bool aStarSingleThreadedFinished = false;

	AStarPathfinder aStar;

	void Start() {

		Random.InitState(seed);
		world = worldGeneratorTiled.World;
		aStar = new AStarPathfinder(world);
		
		prepareSimulations(world.GetLength(0), world.GetLength(1));
	}

	void Update() {
		if(!aStarSingleThreadedFinished)
			runAStarSimulationOnTiled();
		else if(!builtInSimulatorFinished)
			runBuiltInSimulationOnTiled();

		if(builtInSimulatorFinished && aStarSingleThreadedFinished) {
			print("The built in simulation took: " + builtInSimulationTimer.ElapsedTicks + " ticks. (" + builtInSimulationTimer.ElapsedMilliseconds + " ms)\n" + 
					"The A* single threaded simulation took: " + aStarSingleThreadedSimulationTimer.ElapsedTicks + " ticks. (" + aStarSingleThreadedSimulationTimer.ElapsedMilliseconds + " ms)");
		}

		/*if(!builtInSimulatorFinished) {
			runBuiltInSimulation();
		} else if (!aStarSingleThreadedFinished) {
			runAStarSingleThreadedSimulation();
		}
		
		if(builtInSimulatorFinished && aStarSingleThreadedFinished) {
			print("The built in simulation took: " + builtInSimulationTimer.ElapsedTicks + " ticks. (" + builtInSimulationTimer.ElapsedMilliseconds + " ms)\n" + 
					"The A* single threaded simulation took: " + aStarSingleThreadedSimulationTimer.ElapsedTicks + " ticks. (" + aStarSingleThreadedSimulationTimer.ElapsedMilliseconds + " ms)");
		}*/
	}

	// This seems to work
	void runBuiltInSimulationOnTiled() {
		Stopwatch frameWatch = System.Diagnostics.Stopwatch.StartNew();
		
		if(simulationsRunSoFar < numberOfSimulations) {
			while(simulationsRunSoFar < numberOfSimulations && frameWatch.ElapsedMilliseconds < 1) {
				agent.transform.position = startPositions[simulationsRunSoFar];
				target.transform.position = targetPositions[simulationsRunSoFar];
				
				builtInSimulationTimer.Start();
				NavMeshPath path = new NavMeshPath();
				if(agent.CalculatePath(target.position, path)) {
//					print("Found built in.");
					//print("Path calculated between " + startPositions[simulationsRunSoFar].ToString() + " and " + targetPositions[simulationsRunSoFar].ToString() + "!");
				} else {
//					print("Not found built in.");
					//print("Path could not be found between " + startPositions[simulationsRunSoFar].ToString() + " and " + targetPositions[simulationsRunSoFar].ToString() + "!");
				}
				builtInSimulationTimer.Stop();
				
				++simulationsRunSoFar;
			}
		} else {
			builtInSimulatorFinished = true;
			simulationsRunSoFar = 0;
		}
	}

	void runAStarSimulationOnTiled() {
		Stopwatch frameWatch = System.Diagnostics.Stopwatch.StartNew();

		if(simulationsRunSoFar < numberOfSimulations) {
			while(simulationsRunSoFar < numberOfSimulations && frameWatch.ElapsedMilliseconds < 1) {
				agent.transform.position = startPositions[simulationsRunSoFar];
				target.transform.position = targetPositions[simulationsRunSoFar];

				aStarSingleThreadedSimulationTimer.Start();
				aStar.Setup(agent.transform.position, target.transform.position);
				if(aStar.CalculatePath()) {
//					print("Found aStar.");
				} else {
//					print("Not found aStar.");
				}
				aStarSingleThreadedSimulationTimer.Stop();

				++simulationsRunSoFar;				
			}
		} else {
			aStarSingleThreadedFinished = true;
			simulationsRunSoFar = 0;
		}
	}



/*
	void runAStarSingleThreadedSimulation() {
		Stopwatch frameWatch = System.Diagnostics.Stopwatch.StartNew();

		if(simulationsRunSoFar < numberOfSimulations) {
			while(simulationsRunSoFar < numberOfSimulations && frameWatch.ElapsedMilliseconds < 1) {
				agent.transform.position = startPositions[simulationsRunSoFar];
				target.transform.position = targetPositions[simulationsRunSoFar];

				aStarSingleThreadedSimulationTimer.Start();
				aStar.Setup(agent.transform.position, target.transform.position);
				if(aStar.CalculatePath()) {
				// print("WE FOUND SOMETHING WITH A*");
				} else {
				//	print("we did not found anything with A* :'(");
				}
				aStarSingleThreadedSimulationTimer.Stop();

				++simulationsRunSoFar;
			}
		} else {
			aStarSingleThreadedFinished = true;
			simulationsRunSoFar = 0;
		}
	}

	void runBuiltInSimulation() {
		Stopwatch frameWatch = System.Diagnostics.Stopwatch.StartNew();
		
		if(simulationsRunSoFar < numberOfSimulations) {
			while(simulationsRunSoFar < numberOfSimulations && frameWatch.ElapsedMilliseconds < 1) {
				agent.transform.position = startPositions[simulationsRunSoFar];
				target.transform.position = targetPositions[simulationsRunSoFar];
				
				builtInSimulationTimer.Start();
				NavMeshPath path = new NavMeshPath();
				if(agent.CalculatePath(target.position, path)) {
					//print("Path calculated between " + startPositions[simulationsRunSoFar].ToString() + " and " + targetPositions[simulationsRunSoFar].ToString() + "!");
				} else {
					//print("Path could not be found between " + startPositions[simulationsRunSoFar].ToString() + " and " + targetPositions[simulationsRunSoFar].ToString() + "!");
				}
				builtInSimulationTimer.Stop();
				
				++simulationsRunSoFar;
			}
		} else {
			builtInSimulatorFinished = true;
			simulationsRunSoFar = 0;
		}
	}
*/
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
