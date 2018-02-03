// File: AStarPathfinder.cs
// Description: Algoritm for calculating the path between two pints in an tile-based world
// Date: 2018-01-27
// Written by: Jimmy Berlin

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A* pathfinder for tile-world.
/// </summary>
public class AStarPathfinder {
	private bool debug;

	Hashtable closedSet;
	ArrayList openSet;

	Node[,] map;

	int xDim;
	int yDim;

	Node startNode;
	Node goalNode;

	float epsilon;

    /// <summary>
    /// Configures the A* pathfinder for the world you provide.
    /// </summary>
    /// <param name="world">matrix of the world the path-finder should work on.</param>
	public AStarPathfinder(int[,] world, float epsilon = 0.99f, bool debug = false) {
		this.debug = debug;
		this.epsilon = epsilon;

		float[,] tempWorld = new float[world.GetLength(0), world.GetLength(1)];
		for(int x=0;x<world.GetLength(0); x++) {
			for(int y=0;y<world.GetLength(1);y++) {
				tempWorld[x,y] = (float)world[x,y];
			}
		}

		PreConfig(tempWorld);
	}

    /// <summary>
    /// Configures the A* pathfinder for the world you provide.
    /// </summary>
    /// <param name="world">matrix of the world the path-finder should work on.</param>
    public AStarPathfinder(float[,] world) {
		PreConfig(world);
	}

    /// <summary>
    /// Configure the algorithm for the world
    /// </summary>
    /// <param name="world">matrix of the world that the path-finder should world on.</param>
	private void PreConfig(float[,] world) {
		xDim = world.GetLength(0);
		yDim = world.GetLength(1);

		// Create the map to use for calculations;
		map = new Node[xDim, yDim];
		// Fill with nodes.
		for(int x=0;x<xDim;x++) {
			for(int y=0;y<yDim;y++) {
				map[x,y] = new Node(new Vector3(x, world[x,y], y));
			}
		}

		// Adds all the neighbors.
		// TODO: Do i want this here or in Node?		
		for(int x=0;x<xDim;x++) {
			for(int y=0;y<yDim;y++) {
				if(map[x,y].Elevation == 0) {	// Make sure that we can walk on the tile that we are.
					// Up, Down, left, right
					if(x>0 && map[x-1,y].Elevation == 0)
						map[x,y].AddNeighbor(map[x-1,y]);
					if(x<xDim-1 && map[x+1,y].Elevation == 0)
						map[x,y].AddNeighbor(map[x+1,y]);
					if(y>0 && map[x,y-1].Elevation == 0)
						map[x,y].AddNeighbor(map[x,y-1]);
					if(y<yDim-1 && map[x,y+1].Elevation == 0)
						map[x,y].AddNeighbor(map[x,y+1]);
					
					
					// Diagonally
					if(x>0 && y>0 && map[x-1,y-1].Elevation == 0)
						map[x,y].AddNeighbor(map[x-1,y-1]);
					if(x>0 && y<yDim-1 && map[x-1,y+1].Elevation == 0)
						map[x,y].AddNeighbor(map[x-1,y+1]);
					if(x<xDim-1 && y>0 && map[x+1,y-1].Elevation == 0)
						map[x,y].AddNeighbor(map[x+1,y-1]);
					if(x<xDim-1 && y<yDim-1 && map[x+1,y+1].Elevation == 0)
						map[x,y].AddNeighbor(map[x+1,y+1]); 
						
				}
			}
		}

		Setup();
	}

    /// <summary>
    /// Setup the pathfinder before calculating path
    /// </summary>
    /// <param name="start">start position</param>
    /// <param name="goal">target position</param>
	public void Setup(Vector3 start = new Vector3(), Vector3 goal = new Vector3()) {
		closedSet = new Hashtable();
		openSet = new ArrayList();

		startNode = map[(int)start.x, (int)start.z];
		goalNode = map[(int)goal.x, (int)goal.z];

		// Restore g- and f-Score
		for(int x=0;x<xDim;x++) {
			for(int y=0;y<yDim;y++) {
				map[x,y].GScore = float.MaxValue;
				map[x,y].FScore = float.MaxValue;
			}
		}

		startNode.GScore = 0;
		startNode.FScore = Vector3.Distance(start, goal);
	}

    /// <summary>
    /// Calculates the path
    /// </summary>
    /// <returns>true if a path was found, false otherwise.</returns>
	public bool CalculatePath(out ArrayList path) {
		path = null;
		openSet.Add(startNode);

		while(openSet.Count > 0) {
			Node current = findLowestFScoreInOpenSet();
			if(debug) CreateCurrentCube (current.Position3D);

			if(current == goalNode) {
				ReconstructPath(out path);
				return true;
			}
			
			openSet.Remove(current);
			closedSet.Add(current, current);

			foreach(Node neighbour in current.Neighbors) {
				if(closedSet.Contains(neighbour)) {
					continue;
				}

				if(!openSet.Contains(neighbour)) {
					openSet.Add(neighbour);
					if (debug) CreateNeighbourCube (neighbour.Position3D);
				}

				
				float tentative_gScore = current.GScore + Vector3.Distance(current.Position3D, neighbour.Position3D); //Distance to neighbour
				if(tentative_gScore >= neighbour.GScore) {
					continue;	// This is not a better path
				}

				neighbour.From = current;
				neighbour.GScore = tentative_gScore;
				neighbour.FScore = neighbour.GScore + Vector3.Distance(neighbour.Position3D, goalNode.Position3D)*epsilon; // Epsilon is here. Change to try different optimality.
			}
		}
		return false;
	}

	/// <summary>
	/// Finds the lowest f-score in the open set.
    /// Slow ass implementation. Works in O(n).
	/// </summary>
	/// <returns>Node with lowest f-score</returns>
	private Node findLowestFScoreInOpenSet() {
		Node lowest = (Node)openSet[0];

		foreach (Node n in openSet) {
			if (n.FScore < lowest.FScore) {
				lowest = n;
			}
		}

		return lowest;
	}

	/// <summary>
	/// Rebuilds the path from the calculated shit.
	/// </summary>
	/// <returns>Node with lowest f-score</returns>
	private void ReconstructPath(out ArrayList path) {
		path = new ArrayList();
		Node c = goalNode;
		while(c != startNode) {
			path.Add(c);
			c = c.From;
		}
	}


	private void CreateCurrentCube(Vector3 position) {
		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		Renderer r = cube.GetComponent<Renderer> ();
		r.material.color = Color.blue;
		cube.transform.position = position;
	}

	private void CreateNeighbourCube(Vector3 position) {
		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		Renderer r = cube.GetComponent<Renderer> ();
		r.material.color = Color.green;
		cube.transform.position = position;
		cube.transform.localScale = Vector3.one * 0.7f;
	}
}