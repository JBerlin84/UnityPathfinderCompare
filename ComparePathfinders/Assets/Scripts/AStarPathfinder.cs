using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinder {

	Queue<Node> closedSet;
	ArrayList openSet;	// should start with one node.

	Node[,] map;
	Node[,] cameFrom;

	int xDim;
	int yDim;


	Node startNode;
	Node goalNode;

	int totalLookupsInCalculate = 0;

	public AStarPathfinder(float[,] world) {
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
		// TODO: This needs to be better. Only adds fourn eighbors now.
		// TODO: Do i want this here or in Node?
		for(int x=0;x<xDim;x++) {
			for(int y=0;y<yDim;y++) {
				if(x>0)
					map[x,y].AddNeighbor(map[x-1,y]);
				if(x<xDim-1)
					map[x,y].AddNeighbor(map[x+1,y]);
				if(y>0)
					map[x,y].AddNeighbor(map[x,y-1]);
				if(y<yDim-1)
					map[x,y].AddNeighbor(map[x,y+1]);
			}
		}

		Setup();
	}

	public void Setup(Vector3 start = new Vector3(), Vector3 goal = new Vector3()) {
		closedSet = new Queue<Node>();
		openSet = new ArrayList();
		cameFrom = new Node[xDim, yDim];

		startNode = map[(int)start.x, (int)start.z];
		goalNode = map[(int)goal.x, (int)goal.z];

		// Rest g- and fScore
		for(int x=0;x<xDim;x++) {
			for(int y=0;y<yDim;y++) {
				map[x,y].GScore = float.MaxValue;
				map[x,y].FScore = float.MaxValue;
			}
		}

		startNode.GScore = 0;
		startNode.FScore = Vector3.Distance(start, goal);
	}

	public bool CalculatePath() {
		openSet.Add(startNode);

		while(openSet.Count > 0) {
			Node current = findLowestFScoreInOpenSet();
			if(current == goalNode) {
				// TODO: Implement ReconstructPath(cameFrom, current);
				return true;
			}

			openSet.Remove(current);
			closedSet.Enqueue(current);

			foreach(Node neighbour in current.Neighbors) {
				if(closedSet.Contains(neighbour)) {
					continue;
				}

				if(!openSet.Contains(neighbour)) {
					openSet.Add(neighbour);
				}

				float tentative_gScore = current.GScore + Vector3.Distance(current.Position3D, neighbour.Position3D);
				if(tentative_gScore >= neighbour.GScore) {
					continue;	// This is not a better path
				}

				cameFrom[neighbour.X, neighbour.Y] = current;
				neighbour.GScore = tentative_gScore;
				neighbour.FScore = neighbour.GScore + Vector3.Distance(neighbour.Position3D, goalNode.Position3D);
			}
		}

		return false;
	}

	private Node findLowestFScoreInOpenSet() {
		Node node = (Node)openSet[0];
		foreach (Node n in openSet) {
			totalLookupsInCalculate++;
			if(n.FScore < node.FScore) {
				node = n;
			}
		}
		return node;
	}

	public string TotalLookupsInCalculate() {
		return "The number of total lookups in this was: " + totalLookupsInCalculate;
	}
}