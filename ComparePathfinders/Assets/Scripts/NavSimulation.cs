using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Diagnostics;
using System.Threading;

public class NavSimulation : MonoBehaviour {

	public NavMeshAgent agent;
	public Transform target;
	public WorldGeneratorTiled worldGeneratorTiled;

	public int seed = 0;

	int[,] world;

	public int numberOfSimulations;
	public int numberOfSimultaneousAgents;
	private Vector3[,] startPositions;
	private Vector3[,] targetPositions;

	private NavMeshAgent[] agents;
	private Transform[] targets;

	// Use this for initialization

//	Stopwatch builtInSimulationTimer = new System.Diagnostics.Stopwatch();
//	Stopwatch aStarSingleThreadedSimulationTimer = new System.Diagnostics.Stopwatch();
	
	int simulationsRunSoFar = 0;

//	bool builtInSimulatorFinished = false;
//	bool aStarSingleThreadedFinished = false;

	AStarPathfinder aStar;
	AStarPathfinder[] aStars;

	private GameState gameState;

	TestManager testManager;

	Semaphore indexSemaphore;
	int index;
	int coreCount;

	void Start() {
		gameState = GameState.BASE_LINE;
		Random.InitState(seed);
		world = worldGeneratorTiled.World;
		aStar = new AStarPathfinder(world);
		testManager = GameObject.FindObjectOfType<TestManager>();
		coreCount = SystemInfo.processorCount;

		aStars = new AStarPathfinder[numberOfSimultaneousAgents];
		for(int i=0;i<numberOfSimultaneousAgents;i++) {
			aStars[i] = new AStarPathfinder(world);
		}
		
		prepareSimulations(world.GetLength(0), world.GetLength(1));

		indexSemaphore = new Semaphore(0,1);
	}

	void Update() {

		// if(gameState == GameState.BASE_LINE)
		//	; // Don't do anything.
		if(gameState == GameState.BUILT_IN_PATHFINDER)
			runBuiltInSimulationOnTiled();
		else if(gameState == GameState.A_STAR_PATH_FINDER_SINGLE_THREAD)
			runAStarSimulationOnTiled();
		else if(gameState == GameState.A_STAR_PATH_FINDER_MULTI_THREAD)
			runAStarSimulationOnTiledMultiThread();
		//	gameState = GameState.CLEAN_UP;
		else if(gameState == GameState.CLEAN_UP) {
			//print("The built in simulation took: " + builtInSimulationTimer.ElapsedTicks + " ticks. (" + builtInSimulationTimer.ElapsedMilliseconds + " ms)\n" + 
			//		"The A* single threaded simulation took: " + aStarSingleThreadedSimulationTimer.ElapsedTicks + " ticks. (" + aStarSingleThreadedSimulationTimer.ElapsedMilliseconds + " ms)");

//			testManager.SetState(gameState);
			Destroy(gameObject); // TODO: Should we do this just for fun? :P
		}
	}

	// This seems to work
	void runBuiltInSimulationOnTiled() {
		// Stopwatch frameWatch = System.Diagnostics.Stopwatch.StartNew();
		
		if(simulationsRunSoFar < numberOfSimulations) {
			//while(simulationsRunSoFar < numberOfSimulations && frameWatch.ElapsedMilliseconds < 1) {
			for(int i=0;i<numberOfSimultaneousAgents;i++) {
				agents[i].transform.position = startPositions[i,simulationsRunSoFar];
				targets[i].transform.position = targetPositions[i,simulationsRunSoFar];
				//agent.transform.position = startPositions[i,simulationsRunSoFar];
				//target.transform.position = targetPositions[i,simulationsRunSoFar];
				
				//builtInSimulationTimer.Start();
				NavMeshPath path = new NavMeshPath();
				if(agents[i].CalculatePath(target.position, path)) {
//					print("Found built in.");
					//print("Path calculated between " + startPositions[simulationsRunSoFar].ToString() + " and " + targetPositions[simulationsRunSoFar].ToString() + "!");
				} else {
//					print("Not found built in.");
					//print("Path could not be found between " + startPositions[simulationsRunSoFar].ToString() + " and " + targetPositions[simulationsRunSoFar].ToString() + "!");
				}
				//builtInSimulationTimer.Stop();
				
//				++simulationsRunSoFar;
			}
			++simulationsRunSoFar;
		} else {
			//builtInSimulatorFinished = true;
			//gameState = GameState.A_STAR_PATH_FINDER_SINGLE_THREAD;
			gameState = GameState.A_STAR_PATH_FINDER_MULTI_THREAD;
//			testManager.SetState(gameState);
			simulationsRunSoFar = 0;
		}
	}

	void runAStarSimulationOnTiled() {
		// Stopwatch frameWatch = System.Diagnostics.Stopwatch.StartNew();

		if(simulationsRunSoFar < numberOfSimulations) {
			//while(simulationsRunSoFar < numberOfSimulations && frameWatch.ElapsedMilliseconds < 1) {
			for(int i=0;i<numberOfSimultaneousAgents;i++) {
				agents[i].transform.position = startPositions[i,simulationsRunSoFar];
				targets[i].transform.position = targetPositions[i,simulationsRunSoFar];
				//agent.transform.position = startPositions[0,simulationsRunSoFar];
				//target.transform.position = targetPositions[0,simulationsRunSoFar];

				//aStarSingleThreadedSimulationTimer.Start();
				aStar.Setup(agents[i].transform.position, targets[i].transform.position);
				if(aStar.CalculatePath()) {
//					print("Found aStar.");
				} else {
//					print("Not found aStar.");
				}
				//aStarSingleThreadedSimulationTimer.Stop();

				//++simulationsRunSoFar;
			}
			++simulationsRunSoFar;
		} else {
			//aStarSingleThreadedFinished = true;
			//gameState = GameState.A_STAR_PATH_FINDER_MULTI_THREAD;
			// TODO: Multithreaded here.
			gameState = GameState.A_STAR_PATH_FINDER_MULTI_THREAD;
//			testManager.SetState(gameState);
			simulationsRunSoFar = 0;
		}
	}

	void runAStarSimulationOnTiledMultiThread() {
		// Stopwatch frameWatch = System.Diagnostics.Stopwatch.StartNew();

		if(simulationsRunSoFar < numberOfSimulations) {
			//while(simulationsRunSoFar < numberOfSimulations && frameWatch.ElapsedMilliseconds < 1) {
			for(int i=0;i<numberOfSimultaneousAgents;i++) {
				agents[i].transform.position = startPositions[i,simulationsRunSoFar];
				targets[i].transform.position = targetPositions[i,simulationsRunSoFar];
			}
				//agent.transform.position = startPositions[0,simulationsRunSoFar];
				//target.transform.position = targetPositions[0,simulationsRunSoFar];

			index = 0;
			for(int i=0;i<coreCount;i++) {
				Thread thread = new Thread(new ThreadStart(MultiThreadHelperFunction));
				thread.Start();
			}
				/*
				//aStarSingleThreadedSimulationTimer.Start();
				aStar.Setup(agents[i].transform.position, targets[i].transform.position);
				if(aStar.CalculatePath()) {
//					print("Found aStar.");
				} else {
//					print("Not found aStar.");
				}
				//aStarSingleThreadedSimulationTimer.Stop();

				//++simulationsRunSoFar;
				*/
			//}
			++simulationsRunSoFar;
		} else {
			//aStarSingleThreadedFinished = true;
			//gameState = GameState.A_STAR_PATH_FINDER_MULTI_THREAD;
			// TODO: Multithreaded here.
			gameState = GameState.CLEAN_UP;
//			testManager.SetState(gameState);
			simulationsRunSoFar = 0;
		}
	}

	void MultiThreadHelperFunction() {
		//for(int i=0;i<numberOfSimultaneousAgents;i++) {
		while(index < numberOfSimultaneousAgents) {
			//agents[i].transform.position = startPositions[i,simulationsRunSoFar];
			//targets[i].transform.position = targetPositions[i,simulationsRunSoFar];
			//agent.transform.position = startPositions[0,simulationsRunSoFar];
			//target.transform.position = targetPositions[0,simulationsRunSoFar];

			//Thread thread = new Thread(new ThreadStart(MultiThreadHelperFunction));
			/*
			//aStarSingleThreadedSimulationTimer.Start();*/
			
			//aStar.Setup(agents[i].transform.position, targets[i].transform.position);
			int i=0;
			indexSemaphore.WaitOne();
			i = index++;
			indexSemaphore.Release();
			aStars[i].Setup(startPositions[i,simulationsRunSoFar], targetPositions[i,simulationsRunSoFar]);

			if(aStars[i].CalculatePath()) {
//					print("Found aStar.");
			} else {
//					print("Not found aStar.");
			}
			//aStarSingleThreadedSimulationTimer.Stop();

			//++simulationsRunSoFar;
		}
	}

	void prepareSimulations(int xSize, int zSize) {
		startPositions = new Vector3[numberOfSimultaneousAgents, numberOfSimulations];
		targetPositions = new Vector3[numberOfSimultaneousAgents, numberOfSimulations];
		agents = new NavMeshAgent[numberOfSimultaneousAgents];
		targets = new Transform[numberOfSimultaneousAgents];

		for(int i=0;i<numberOfSimultaneousAgents;i++) {
			agents[i] = Instantiate(agent);
			targets[i] = Instantiate(target);
		}

		for(int i=0;i<numberOfSimultaneousAgents;i++) {
			for(int j=0;j<numberOfSimulations;j++) {
				// Start
				int x = Random.Range(0, xSize-1);
				int z = Random.Range(0, zSize-1);
				startPositions[i,j] = new Vector3(x, 0, z);

				// Target
				x = Random.Range(0, xSize-1);
				z = Random.Range(0, zSize-1);
				targetPositions[i,j] = new Vector3(x, 0, z);
			}
		}
	}

	// TODO: This should take the state.
	public void SetState(GameState state) {
		gameState = state;
	}

	public GameState GetState() {
		return gameState;
	}
}
