using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stresser : MonoBehaviour {

	private Vector3 position;

	public GameObject box;
	public int boxCount;
	public Vector3 boxScale;
	public bool randomizeScale;
	public float randomRange;

	// Use this for initialization
	void Start () {
		position = transform.position;
		for(int i=0;i<boxCount;i++) {
			Vector3 spawnPos = new Vector3(position.x+Random.Range(-4,4), position.y+Random.Range(-4,4), position.z+Random.Range(-4,4));
			GameObject newBox = Instantiate(box, spawnPos, Quaternion.identity);

			float xSize = randomizeScale ? boxScale.x + Random.Range(-randomRange, randomRange): boxScale.x;
			float ySize = randomizeScale ? boxScale.y + Random.Range(-randomRange, randomRange): boxScale.y;
			float zSize = randomizeScale ? boxScale.z + Random.Range(-randomRange, randomRange): boxScale.z;
			newBox.transform.localScale = new Vector3(xSize, ySize, zSize);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.Rotate(0,0,1f);
	}
}
