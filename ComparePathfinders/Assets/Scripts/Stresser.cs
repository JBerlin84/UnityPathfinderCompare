// File: Stresser.cs
// Description: Stress loader to force caluclations and increase the load on the cpu/gpu during simulations.
// Date: 2018-01-27
// Written by: Jimmy Berlin

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stress loader to force calculations and increase the load on the cpu/gpu during simulations.
/// </summary>
public class Stresser : MonoBehaviour {

	private Vector3 position;

	public GameObject box;
	public int boxCount;
	private int currentBoxCount;
	public Vector3 boxScale;
	public bool randomizeScale;
	public float randomRange;

	/// <summary>
	/// Initializes the stress loader.
	/// </summary>
	void Start () {
		position = transform.position;
		for(int i=0;i<boxCount;i++) {
			SpawnBox();
		}
		currentBoxCount = boxCount;
	}
	
	/// <summary>
	/// Fixed update is used for physics caluclations.
	/// </summary>
	void FixedUpdate () {
		transform.Rotate(0,0,-1f);

		if(currentBoxCount < boxCount) {
			for(int i=currentBoxCount;i<boxCount;i++) {
				SpawnBox();
			}
			currentBoxCount = boxCount;
		}
	}

    /// <summary>
    /// Spawns little boxes in the big box to increase load on the computer.
    /// </summary>
	private void SpawnBox() {
		Vector3 spawnPos = new Vector3(position.x+Random.Range(-4,4), position.y+Random.Range(-4,4), position.z+Random.Range(-4,4));
		GameObject newBox = Instantiate(box, spawnPos, Quaternion.identity);

		float xSize = randomizeScale ? boxScale.x + Random.Range(-randomRange, randomRange): boxScale.x;
		float ySize = randomizeScale ? boxScale.y + Random.Range(-randomRange, randomRange): boxScale.y;
		float zSize = randomizeScale ? boxScale.z + Random.Range(-randomRange, randomRange): boxScale.z;
		newBox.transform.localScale = new Vector3(xSize, ySize, zSize);
	}

    /// <summary>
    /// Set how many small boxes you want in the big one.
    /// Increasing the number during runtime will increase the number of small boxes.
    /// Reducing the number does NOT remove boxes.
    /// </summary>
    /// <param name="boxCount">Number of small boxes you want to have.</param>
	public void SetBoxCount(int boxCount) {
		this.boxCount = boxCount;
	}
}
