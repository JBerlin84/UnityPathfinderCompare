using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stresser : MonoBehaviour {

	private Vector3 position;

	public GameObject box;

	public int boxCount;
	private int currentBoxCount;

	// Use this for initialization
	void Start () {
		position = transform.position;
		for(int i=0;i<boxCount;i++) {
			Vector3 spawnPos = new Vector3(position.x+Random.Range(-4,4), position.y+Random.Range(-4,4), position.z+Random.Range(-4,4));
			Instantiate(box, spawnPos, Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if(currentBoxCount < boxCount) {
			Instantiate(box, transform.position, Quaternion.identity);
			currentBoxCount++;
		}*/

		transform.Rotate(0,0,0.5f);
	}
}
