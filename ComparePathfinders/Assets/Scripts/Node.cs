using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Node {
	float gScore;	// Default infinity with the exception of the start node.
	public float GScore { get { return gScore; } set { gScore = value; } }
	float fScore;	// Estimated cost from this node to the end. (for the first node its completely heuristic.)
	public float FScore { get { return fScore; } set { fScore = value; } }

	ArrayList neighbors;
	public ArrayList Neighbors { get { return neighbors; } }

	Vector3 position3D;
	public Vector3 Position3D { get { return position3D; } }

	int x;
	public int X { get { return x; } }
	int y;
	public int Y { get { return y; } }

	public Node(Vector3 position3D) {
		neighbors = new ArrayList();

		gScore = float.MaxValue;
		fScore = float.MaxValue;

		this.position3D = position3D;
		x = (int)position3D.x;
		y = (int)position3D.z;	// Since we convert from 3d to 2d z-coord becomes y-coord
	}

	public void AddNeighbor(Node neighbor) {
		neighbors.Add(neighbor);
	}
}