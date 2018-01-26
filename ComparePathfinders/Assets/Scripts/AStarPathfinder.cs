using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinder {

	//Queue<Node> closedSet;
	PriorityQueue<Node> closedSet;
	ArrayList openSet;	// should start with one node.
	//Hashtable openSet;

	Node[,] map;
	Node[,] cameFrom;

	int xDim;
	int yDim;


	Node startNode;
	Node goalNode;

	public AStarPathfinder(int[,] world) {
		float[,] tempWorld = new float[world.GetLength(0), world.GetLength(1)];
		for(int x=0;x<world.GetLength(0); x++) {
			for(int y=0;y<world.GetLength(1);y++) {
				tempWorld[x,y] = (float)world[x,y];
			}
		}

		PreConfig(tempWorld);
	}

	public AStarPathfinder(float[,] world) {
		PreConfig(world);
	}

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
		// TODO: This needs to be better. Only adds fourn eighbors now.
		// TODO: Do i want this here or in Node?
		
		// TODO: Some cleanup here!
		
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
					if(x>0 && y<0 && map[x-1,y+1].Elevation == 0)
						map[x,y].AddNeighbor(map[x-1,y+1]);
					if(x<0 && y>0 && map[x+1,y-1].Elevation == 0)
						map[x,y].AddNeighbor(map[x+1,y-1]);
					if(x<0 && y<0 && map[x+1,y+1].Elevation == 0)
						map[x,y].AddNeighbor(map[x+1,y+1]); 
				}
			}
		}

		// Lets try and see if we can add all neighbors in straight lines from our selves that we can see.
		// Check all nodes.
		/*
		for(int x=0;x<xDim;x++) {
			for(int y=0;y<yDim;y++) {
				// all y upwards
				for(int ny = y-1;ny>=0;ny--) {
					if(map[x,ny].Elevation == 0) {
						map[x,y].AddNeighbor(map[x,ny]);
					} else {
						break;
					}
				}
				// all y downwards
				for(int ny = y+1;ny<yDim;ny++) {
					if(map[x,ny].Elevation == 0) {
						map[x,y].AddNeighbor(map[x,ny]);
					} else {
						break;
					}
				}
				// all x left
				for(int nx = x-1;nx>=0;nx--) {
					if(map[nx,y].Elevation == 0) {
						map[x,y].AddNeighbor(map[nx,y]);
					} else {
						break;
					}
				}
				// all x right
				for(int nx = y+1;nx<yDim;nx++) {
					if(map[nx,y].Elevation == 0) {
						map[x,y].AddNeighbor(map[nx,y]);
					} else {
						break;
					}
				}
			}
		}*/

		Setup();
	}

	public void Setup(Vector3 start = new Vector3(), Vector3 goal = new Vector3()) {
		//closedSet = new Queue<Node>();
		closedSet = new PriorityQueue<Node>(xDim*yDim/2);
		openSet = new ArrayList();
		//openSet = new Hashtable();
		cameFrom = new Node[xDim, yDim];

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

	public bool CalculatePath() {
		openSet.Add(startNode);
		//openSet.Add(startNode, startNode);
		//Debug.Log("We start with: " + startNode.Neighbors.Count + " neighbors");

		while(openSet.Count > 0) {
			Node current = findLowestFScoreInOpenSet();
			if(current == goalNode) {
				// TODO: Implement ReconstructPath(cameFrom, current);
				return true;
			}

			// If we dont hit anything, we can be sure that we can move there.
			// get us from 3seconds to 3 ms.
			
//			RaycastHit hit;
//			if(!Physics.Raycast(current.ElevatedPosition3D, (current.ElevatedPosition3D - goalNode.ElevatedPosition3D), out hit)) {
//				return true;
//			}

			openSet.Remove(current);
			//closedSet.Enqueue(current);
			closedSet.Add(current);

			foreach(Node neighbour in current.Neighbors) {
				if(closedSet.Contains(neighbour)) {
					continue;
				}

				if(!openSet.Contains(neighbour)) {
					openSet.Add(neighbour);
					//openSet.Add(neighbour, neighbour);
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
			if(n.FScore < node.FScore) {
				node = n;
			}
		}
		return node;
	}

	/*
	private Node findLowestFScoreInOpenSet() {
		Node node = null;
		foreach (DictionaryEntry ne in openSet) {
			Node n = (Node)ne.Value;
			if(node == null) {
				node = n;
			}
			if(n.FScore < node.FScore) {
				node = n;
			}
		}
		return node;
	}*/
}