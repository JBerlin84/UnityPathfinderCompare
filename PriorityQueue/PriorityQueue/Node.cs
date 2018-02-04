// File: Node.cs
// Description: Node used in the A*-pathfinding algorithm
// Date: 2018-01-27
// Written by: Jimmy Berlin

using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEngine;

/// <summary>
/// Node used in the A*-pathfinding algorithm
/// </summary>
class Node : IComparable {
	float gScore;	// Default infinity with the exception of the start node.
    /// <summary>
    /// return g-score of node.
    /// </summary>
	public float GScore { get { return gScore; } set { gScore = value; } }
	float fScore;	// Estimated cost from this node to the end. (for the first node its completely heuristic.)
    /// <summary>
    /// Return f-score of node.
    /// </summary>
	public float FScore { get { return fScore; } set { fScore = value; } }

    Node from;
    /// <summary>
    /// Return which node we came from.
    /// </summary>
    public Node From { get { return from; } set { from = value; } }

	ArrayList neighbors;
    /// <summary>
    /// Fetch all accessible neighbors of the node.
    /// </summary>
	public ArrayList Neighbors { get { return neighbors; } }

#if Unity
	Vector3 position3D;
    /// <summary>
    /// Returns Vector3 of position in the world
    /// </summary>
	public Vector3 Position3D { get { return position3D; } }
    /// <summary>
    /// Returns Vector3 of position in the world with height set to 1.
    /// </summary>
	public Vector3 ElevatedPosition3D { get { return new Vector3(X, 1, Y); } }
#endif

	int x;
    /// <summary>
    /// Returns x-position of node
    /// </summary>
	public int X { get { return x; } }
	int y;
    /// <summary>
    /// Returns y-position of node
    /// </summary>
	public int Y { get { return y; } }

#if Unity
    /// <summary>
    /// Returns elevation of current node.
    /// </summary>
	public float Elevation{ get {return position3D.y; } }
#endif

#if Unity
    /// <summary>
    /// Create node from Vector3
    /// </summary>
    /// <param name="position3D">Position of node</param>
	/*public Node(Vector3 position3D) {
		neighbors = new ArrayList();

		gScore = float.MaxValue;
		fScore = float.MaxValue;

		this.position3D = position3D;
		x = (int)position3D.x;
		y = (int)position3D.z;	// Since we convert from 3d to 2d z-coord becomes y-coord
	}*/
#endif

    /// <summary>
    /// Adds accessible neighbor to node.
    /// </summary>
    /// <param name="neighbor">neighbor to add.</param>
	public void AddNeighbor(Node neighbor) {
		neighbors.Add(neighbor);
	}

    /// <summary>
    /// CompareTo. returns whether the compared object is smaller or larger than this.
    /// </summary>
    /// <param name="obj">object to compare to</param>
    /// <returns>-1 if this node has lower f-score than the provided. 0 if equal f-score. 1 if the provided node has higher f-score.</returns>
	int IComparable.CompareTo(object obj) {
		if(fScore < (obj as Node).FScore) {
			return -1;
		} else if(fScore > (obj as Node).FScore) {
			return 1;
		} else {
			return 0;
		}
	}

	public override string ToString() {
		return fScore.ToString ();
	}
}