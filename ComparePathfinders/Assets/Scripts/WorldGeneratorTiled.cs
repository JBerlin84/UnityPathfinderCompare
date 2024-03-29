﻿// File: WorldGeneratorTiled.cs
// Description: Generates a tiled world.
// Date: 2018-01-27
// Written by: Jimmy Berlin

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates a tile world.
/// </summary>
public class WorldGeneratorTiled : MonoBehaviour {

	public string seed;
	public bool randomize;

	[Range(0,1)]
	public float fillPercentage;
	public int worldWidth;
	public int worldHeight;

	public Transform ground;
	public Transform navigationFloor;
	public Transform obstacle;

	protected int[,] world;	// 1 is wall, 0 is passable.
    /// <summary>
    /// Returns world matrix
    /// </summary>
    public int[,] World {
		get { return world; }
	}

    /// <summary>
    /// Creates the tile world.
    /// </summary>
	void Awake() {
		if(randomize) {
			Random.InitState((int)System.DateTime.Now.Ticks);
		} else {
			Random.InitState(seed.GetHashCode());
		}

		world = new int[worldWidth, worldHeight];

		for(int x = 0;x<worldWidth;x++) {
			for(int y=0;y<worldHeight;y++) {
				world[x,y] = 0;
			}
		}

		int obstacleCount = (int)(worldWidth*worldHeight*fillPercentage);
		for(int i=0;i<obstacleCount;i++) {
			int xPos = Random.Range(0,worldWidth);
			int yPos = Random.Range(0,worldHeight);

			// If position is not taken.
			if(world[xPos,yPos] == 0) {
				if(CheckConnectivity(xPos, yPos)) {
					world[xPos, yPos] = 1;
				}
			}
		}
	}

	/// <summary>
	/// Initialize the world.
	/// </summary>
	void Start () {
		Transform parent = new GameObject("Map").transform;

		for (int i=0;i<worldWidth;i++) {
			for(int j=0;j<worldHeight;j++) {
				float xPos = i;
				float zPos = j;
				Transform o = Instantiate(world[i,j] == 0 ? ground : obstacle, new Vector3(xPos,0f,zPos), Quaternion.identity) as Transform;
				o.parent = parent;
			}
		}
	}
	
    /// <summary>
    /// Checks the connectivity of the world. We dont want any "islands" in the world.
    /// </summary>
    /// <param name="xPos">max xPos</param>
    /// <param name="yPos">max yPos</param>
    /// <returns>true if the world is connected.</returns>
	bool CheckConnectivity(int xPos, int yPos) {
		bool existFree = false;
		Vector2 firstFree = new Vector2(-1f, -1f);
		//find first free slot.
		for(int x=0;x<worldWidth;x++) {
			for(int y=0;y<worldHeight;y++) {
				if(world[x,y] == 0) {
					firstFree = new Vector2(x,y);
					existFree = true;
					break;
				}
			}
			if(existFree) {			// Compiler sais that this is unreachable, which it is not.
				break;
			}
		}

		if(!existFree) {
			return false;
		}

		int currentFreeCount = 0;
		for(int x=0;x<worldWidth;x++) {
			for(int y=0;y<worldHeight;y++) {
				if(world[x,y] == 0) {
					++currentFreeCount;
				}
			}
		}

		bool[,] mapFlags = new bool[worldWidth, worldHeight];	// Helper variable to theck all the available tiles.
		Queue<Vector2> queue = new Queue<Vector2>();
		queue.Enqueue(firstFree);

		int accessibleTileCount = 1;
		mapFlags[(int)firstFree.x, (int)firstFree.y] = true;
		mapFlags[xPos, yPos] = true;

		while(queue.Count > 0) {
			Vector2 tile = queue.Dequeue();

			for(int x=-1;x<=1;x++) {
				for(int y=-1;y<=1;y++) {
					int neighbourX = (int)tile.x+x;
					int neighbourY = (int)tile.y+y;

					// Only check nondiagonal ones.
					if(x==0 || y==0) {
						if (neighbourX >= 0 && neighbourX < worldWidth && neighbourY >= 0 && neighbourY < worldHeight) {
							if (!mapFlags [neighbourX, neighbourY] && world[neighbourX, neighbourY] == 0) {
								mapFlags [neighbourX, neighbourY] = true;
								queue.Enqueue (new Vector2 (neighbourX, neighbourY));
								accessibleTileCount++;
							}
						}
					}
				}
			}
		}

		return accessibleTileCount == currentFreeCount - 1;
	}
}
