// File: DebugWorldGeneratorTiled.cs
// Description: Generates a tiled world.
// Date: 2018-01-27
// Written by: Jimmy Berlin

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates an easy predefined world for debugging.
/// </summary>
public class DebugWorldGeneratorTiled : WorldGeneratorTiled {
	void Awake() {
		world = new int[,] {
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 1, 1, 1, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
		};
		worldWidth = world.GetLength (0);
		worldHeight = world.GetLength (1);

	}
}
