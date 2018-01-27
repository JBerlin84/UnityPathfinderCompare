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

	int threadIndex;
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
		world = worldGeneratorTiled.World;
		aStar = new AStarPathfinder(world);
		coreCount = SystemInfo.processorCount;

		aStars = new AStarPathfinder[numberOfSimultaneousAgents];
		for(int i=0;i<numberOfSimultaneousAgents;i++) {
			aStars[i] = new AStarPathfinder(world);
		}
		
		prepareSimulations(world.GetLength(0), world.GetLength(1));
	}

	void Update() {
		if(gameState == GameState.BUILT_IN_PATHFINDER)
			runBuiltInSimulationOnTiled();
		else if(gameState == GameState.A_STAR_PATH_FINDER_SINGLE_THREAD)
			runAStarSimulationOnTiled();
		else if(gameState == GameState.A_STAR_PATH_FINDER_MULTI_THREAD)
			runAStarSimulationOnTiledMultiThread();
	}

	// This seems to work
	void runBuiltInSimulationOnTiled() {
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
		// TODO: We are probably too fast here. Make sure were never having more than a certain ammount of threads?
		// Just for graphics. Might actually destroy a little bit. But is the same for every part.
		for(int i=0;i<numberOfSimultaneousAgents;i++) {
			agents[i].transform.position = startPositions[i,simulationsRunSoFar];
			targets[i].transform.position = targetPositions[i,simulationsRunSoFar];
		}

		// Actual calculation
		threadIndex = -1;
		// Start threads
		Thread[] thread = new Thread[coreCount];
		for(int i=0;i<coreCount;i++) {
			thread[i] = new Thread(new ThreadStart(MultiThreadHelperFunction));
			thread[i].Start();
		}
		// Join threads. (Could be omitted if we were sure the computer could handle it.)
		for(int i=0;i<coreCount;i++) {
		 	thread[i].Join();				// halverar prestandan.
		}
		++simulationsRunSoFar;
		if(simulationsRunSoFar > numberOfSimulations) {
			simulationsRunSoFar = 0;
		}
	}

	void MultiThreadHelperFunction() {
		int myIndex = Interlocked.Increment(ref threadIndex);

		int range = numberOfSimultaneousAgents / coreCount;
		int min = range*myIndex;
		int max = min + range;

		for(int i=min;i<max;i++) {
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
				int x = Random.Range(0, xSize);
				int z = Random.Range(0, zSize);
				startPositions[i,j] = new Vector3(x, 0, z);

				// Target
				x = Random.Range(0, xSize);
				z = Random.Range(0, zSize);
				targetPositions[i,j] = new Vector3(x, 0, z);
			}
		}
	}

	public void SetState(GameState state) {
		gameState = state;
	}
}
