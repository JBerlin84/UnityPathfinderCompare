using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FPSCounter))]
public class TestManager : MonoBehaviour {

	public NavSimulation simulator;

	private enum State {
		BASE_LINE,
		BUILT_IN_PATHFINDER,
		A_STAR_PATH_FINDER_SINGLE_THREAD,
		A_STAR_PATH_FINDER_MULTI_THREAD,
		CLEAN_UP
	}

	private float recordFrequency;
	private float nextRecordTime;

	public float baseLineTest;
	private float baseLineTestTime;

	private State currentState;

	private TestData testData = new TestData();
	private FPSCounter fpsCounter;

	void Awake () {
		fpsCounter = gameObject.GetComponent(typeof(FPSCounter)) as FPSCounter;

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
			int fps = fpsCounter.getFPS();
			testData.BaselineFPS.Add(fps);	// TODO: Make sure were recording the data in the correct place.
		}

		// Base line test is finished.
		if(currentState == State.BASE_LINE && Time.time > baseLineTestTime) {
			print("We no longer do the base line test.");
			currentState = State.BUILT_IN_PATHFINDER;
			simulator.SetState();
		}
	}
}
