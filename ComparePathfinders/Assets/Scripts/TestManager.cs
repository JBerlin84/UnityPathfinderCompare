using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FPSCounter))]
[RequireComponent(typeof(ComputerInformation))]
public class TestManager : MonoBehaviour {

	// private objects.
	private FPSCounter fpsCounter;
	private ComputerInformation computerInformation;
	private GameState gameState;
	private Stresser stressLoader = null;
	private ParticleSystem pSystem = null;

	// Helper objects
	private TestData testData;

	// Helper variables
	private float nextRecordTime;
	private float nextGameStateTime;			// used to keep track of time of each game state. (several within each stress load.)
	private int currentSimulationState;

	[Header("Global settings")]
	public float gameStateTime;
	public float recordFrequency;
	public int threadMultiplier = 1;

	[Header("Game objects")]
	public NavSimulation navigationSimulator;
	[Space]
	public Stresser stressLoaderPrefab;
	public Vector3 stressLoaderSpawnPosition;
	public Vector3 stressLoaderSpawnRotation;
	[Space]
	public ParticleSystem particleSystemPrefab;
	public Vector3 particleSystemSpawnPosition;
	public Vector3 particleSystemSpawnRotation;
	


	[Header("Test state settings")]
	public SimulationStateSettings[] simulationStateSettings = new SimulationStateSettings[4];


	void Awake () {
		fpsCounter = gameObject.GetComponent(typeof(FPSCounter)) as FPSCounter;
		computerInformation = gameObject.GetComponent(typeof(ComputerInformation)) as ComputerInformation;
		testData = new TestData();

		QualitySettings.vSyncCount = 0;			// For the love of God, make sure V-Sync is off!!!!!!!
		//Application.targetFrameRate = 200;	// We shouldn't have to use this when v-sync is off.
	}

	// Use this for initialization
	void Start () {
		if(QualitySettings.vSyncCount > 0)
			QualitySettings.vSyncCount = 0;

		stressLoader = Instantiate(stressLoaderPrefab, stressLoaderSpawnPosition, Quaternion.Euler(stressLoaderSpawnRotation)) as Stresser;
		stressLoader.gameObject.SetActive(false);
		pSystem = Instantiate(particleSystemPrefab, particleSystemSpawnPosition, Quaternion.Euler(particleSystemSpawnRotation)) as ParticleSystem;
		pSystem.gameObject.SetActive(false);

		currentSimulationState = 0;

		// Initialize first state
		SetupSimulationState();

		nextGameStateTime = Time.time + gameStateTime;
		gameState = GameState.BASE_LINE;
	}
	
	// Update is called once per frame
	void Update () {
		// Don't do anything more if were finished. Save here and exit appliation.
		if(currentSimulationState >= simulationStateSettings.Length) {
			Time.timeScale = 0;
			SaveDataToFile();
			Application.Quit();	// Exit everything
		// quit application;
		}

		if(Time.time > nextRecordTime) {
			testData.Record(simulationStateSettings[currentSimulationState].settingsName, gameState, fpsCounter.getFPS());
			nextRecordTime = Time.time + recordFrequency;
		}

		// Update game state
		if(Time.time > nextGameStateTime) {
			gameState++;
			
			if((int)gameState >= Enum.GetNames(typeof(GameState)).Length) {
				gameState = 0;
				currentSimulationState++;
				if(currentSimulationState >= simulationStateSettings.Length)
					return;		// make sure were not getting index out of range.
				
				SetupSimulationState();
			}

			navigationSimulator.SetState(gameState);
			nextGameStateTime = Time.time + gameStateTime;
		}






		// if(Time.time >= nextRecordTime) {
		// 	int fps = fpsCounter.getFPS();
		// 	// testData.BaselineFPS.Add(fps);	// TODO: Make sure were recording the data in the correct place.
		// 	testData.Record(gameState, fps);
		// }

		// // Base line test is finished.
		// if(gameState == GameState.BASE_LINE && Time.time > baseLineTest) {
		// 	gameState = GameState.BUILT_IN_PATHFINDER;
		// 	navigationSimulator.SetState(gameState);
		// } else if (gameState == GameState.CLEAN_UP) {
		// 	SimulationFinished();
		// }
	}

	private void SetupSimulationState() {
		// print("use stress loader: " + simulationStateSettings[currentSimulationState].useStressLoader + ", magnitude: " + simulationStateSettings[currentSimulationState].stressLoaderMagnitude + "\n" +
		// 		"use particle system: " + simulationStateSettings[currentSimulationState].useParticleSystem + ", magnitude: " + simulationStateSettings[currentSimulationState].stressLoaderMagnitude);
		stressLoader.gameObject.SetActive(simulationStateSettings[currentSimulationState].useStressLoader);
		if(stressLoader.gameObject.activeSelf) {
			stressLoader.SetBoxCount(simulationStateSettings[currentSimulationState].stressLoaderMagnitude);
		}

		pSystem.gameObject.SetActive(simulationStateSettings[currentSimulationState].useParticleSystem);
		if(pSystem.gameObject.activeSelf) {
			ParticleSystem.MainModule main = pSystem.main;	// unity does not allow me to change this myself.
			main.maxParticles = simulationStateSettings[currentSimulationState].particleSystemMagnitude;
		}
	}

	private void SaveDataToFile() {
		string data = computerInformation.ToString() + "\n" + testData.ToString();
		string filename = "test.txt";
		System.IO.File.WriteAllText(filename, data);
		print("data saved!!");
	}

	// public void SimulationFinished() {
	// 	print("We are now finished and are packing up everything!!!");
	// 	print(testData.ToString());
	// }
}
