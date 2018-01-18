using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Diagnostics;
using System.Threading;

[RequireComponent(typeof(WorldGeneratorTiled))]
public class NavSimulation : MonoBehaviour {

	
	[Header("Remember to have a properly baked navigation mesh floor\n when using this script!!")]
	
	[Space]
	[Space]

	public NavMeshAgent agent;
	public Transform target;
	private WorldGeneratorTiled worldGeneratorTiled;

	public int seed = 0;
	public bool randomize;

	int[,] world;

	public int numberOfSimulations;
	public int numberOfSimultaneousAgents;
	private Vector3[,] startPositions;
	private Vector3[,] targetPositions;

	private NavMeshAgent[] agents;
	private Transform[] targets;

	// Use this for initialization
	
	int simulationsRunSoFar = 0;

	AStarPathfinder aStar;
	AStarPathfinder[] aStars;

	private GameState gameState;

	Semaphore indexSemaphore;
	int index;
	int coreCount;

	void Awake() {
		worldGeneratorTiled = GetComponent(typeof(WorldGeneratorTiled)) as WorldGeneratorTiled;
	}

	void Start() {
		if(randomize) {
			Random.InitState((int)System.DateTime.Now.Ticks);
		} else {
			Random.InitState(seed.GetHashCode());
		}
		//gameState = GameState.BASE_LINE;
		world = worldGeneratorTiled.World;
		aStar = new AStarPathfinder(world);
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
		//else if(gameState == GameState.CLEAN_UP) {
			//print("The built in simulation took: " + builtInSimulationTimer.ElapsedTicks + " ticks. (" + builtInSimulationTimer.ElapsedMilliseconds + " ms)\n" + 
			//		"The A* single threaded simulation took: " + aStarSingleThreadedSimulationTimer.ElapsedTicks + " ticks. (" + aStarSingleThreadedSimulationTimer.ElapsedMilliseconds + " ms)");

			//Destroy(gameObject); // TODO: Should we do this just for fun? :P
		//}
	}

	// This seems to work
	void runBuiltInSimulationOnTiled() {
		print("built in simulation on tiled");
		for(int i=0;i<numberOfSimultaneousAgents;i++) {
			agents[i].transform.position = startPositions[i,simulationsRunSoFar];
			targets[i].transform.position = targetPositions[i,simulationsRunSoFar];
			NavMeshPath path = new NavMeshPath();
			agents[i].CalculatePath(target.position, path);
		}
		++simulationsRunSoFar;
		simulationsRunSoFar %= numberOfSimulations;
	}

	void runAStarSimulationOnTiled() {
		print("A* singlethreaded simulation on tiled");
		for(int i=0;i<numberOfSimultaneousAgents;i++) {
			agents[i].transform.position = startPositions[i,simulationsRunSoFar];
			targets[i].transform.position = targetPositions[i,simulationsRunSoFar];

			aStar.Setup(agents[i].transform.position, targets[i].transform.position);
			aStar.CalculatePath();
		}
		++simulationsRunSoFar;
		simulationsRunSoFar %= numberOfSimulations;
	}

	void runAStarSimulationOnTiledMultiThread() {
		print("A* multithreaded simulation on tiled");
		// Just for graphics. Might actually destroy a little bit. But is the same for every part.
		for(int i=0;i<numberOfSimultaneousAgents;i++) {
			agents[i].transform.position = startPositions[i,simulationsRunSoFar];
			targets[i].transform.position = targetPositions[i,simulationsRunSoFar];
		}

		// Actual calculation
		index = 0;
		// Start threads
		Thread[] thread = new Thread[coreCount];
		for(int i=0;i<coreCount;i++) {
			thread[i] = new Thread(new ThreadStart(MultiThreadHelperFunction));
			thread[i].Start();
		}
		// Join threads. (Could be omitted if we were sure the computer could handle it.)
		// There is a bug here preventing me form joining. Probably while down below. Some threads never reach index = number of simultaneous agents.
		// for(int i=0;i<coreCount;i++) {
		// 	thread[i].Join();
		// }
		++simulationsRunSoFar;
		simulationsRunSoFar %= numberOfSimulations;
	}

	void MultiThreadHelperFunction() {
		while(index < numberOfSimultaneousAgents) {
			int i=0;
			indexSemaphore.WaitOne();
			i = index++;
			indexSemaphore.Release();

			aStars[i].Setup(startPositions[i,simulationsRunSoFar], targetPositions[i,simulationsRunSoFar]);
			aStars[i].CalculatePath();
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
}
