using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FPSCounter))]
[RequireComponent(typeof(ComputerInformation))]
public class TestManager : MonoBehaviour {

	public NavSimulation simulator;

	public float recordFrequency;
	private float nextRecordTime;

	public float baseLineTest;
	private float baseLineTestTime;

	private GameState gameState;

	private TestData testData = new TestData();
	private FPSCounter fpsCounter;
	private ComputerInformation computerInformation;
	private int coreCount;

	void Awake () {
		fpsCounter = gameObject.GetComponent(typeof(FPSCounter)) as FPSCounter;
		computerInformation = gameObject.GetComponent(typeof(ComputerInformation)) as ComputerInformation;

		QualitySettings.vSyncCount = 0;
		gameState = GameState.BASE_LINE;
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
			// testData.BaselineFPS.Add(fps);	// TODO: Make sure were recording the data in the correct place.
			testData.Record(gameState, fps);
		}

		// Base line test is finished.
		if(gameState == GameState.BASE_LINE && Time.time > baseLineTestTime) {
			gameState = GameState.BUILT_IN_PATHFINDER;
			simulator.SetState(gameState);
		} else if (gameState == GameState.CLEAN_UP) {
			SimulationFinished();
		}
	}

	public void SetState(GameState gameState) {
		this.gameState = gameState;
	}

	public void SimulationFinished() {
		print("We are now finished and are packing up everything!!!");
		print(testData.ToString());
	}
}
