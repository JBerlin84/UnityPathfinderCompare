using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour {

	private enum State {
		BASE_LINE,
		BUILT_IN_PATHFINDER,
		A_STAR_PATH_FINDER_SINGLE_THREAD,
		A_STAR_PATH_FINDER_MULTI_THREAD
	}

	private float recordFrequency;
	private float nextRecordTime;

	public float baseLineTest;
	private float baseLineTestTime;

	private State currentState;

	void Awake () {
		QualitySettings.vSyncCount = 0;
		currentState = State.BASE_LINE;
	}

	// Use this for initialization
	void Start () {
		nextRecordTime = Time.time + recordFrequency;
		baseLineTestTime = Time.time + baseLineTest;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time >= nextRecordTime) {
			// Record the framerate
		}

		// Base line test is finished.
		if(currentState == State.BASE_LINE && Time.time > baseLineTestTime) {
			print("We no longer do the base line test.");
			currentState = State.BUILT_IN_PATHFINDER;
		}


	}
}
