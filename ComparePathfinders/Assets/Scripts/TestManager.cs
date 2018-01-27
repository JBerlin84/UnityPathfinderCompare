// File: TestManager.cs
// Description: The brain of the simulation.
// Date: 2018-01-27
// Written by: Jimmy Berlin

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The brain of the simulation. Handles all the states and everything.
/// </summary>
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
	private float warmupTimeFinish;
	private float nextGameStateTime;			// used to keep track of time of each game state. (several within each stress load.)
	private int currentSimulationState;
	private bool saved;

	[Header("Global settings")]
	public float gameStateTime;
	public float warmupTime;					// used to counteract the fact that the fps-counter "lags" a bit behind.
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

    /// <summary>
    /// Create simulation
    /// </summary>
	void Awake () {
		fpsCounter = gameObject.GetComponent(typeof(FPSCounter)) as FPSCounter;
		computerInformation = gameObject.GetComponent(typeof(ComputerInformation)) as ComputerInformation;
		testData = new TestData();

		QualitySettings.vSyncCount = 0;			// For the love of God, make sure V-Sync is off!!!!!!!
		saved = false;
	}

	/// <summary>
	/// Initialize the simulation.
	/// </summary>
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

		nextRecordTime = Time.time + recordFrequency;
		warmupTimeFinish = Time.time + warmupTime;
	}
	
	/// <summary>
	/// Update states and store data of the simulation.
	/// </summary>
	void Update () {
		// Don't do anything more if were finished. Save here and exit appliation.
		if(currentSimulationState >= simulationStateSettings.Length) {
			Time.timeScale = 0;
			if(!saved) {
				SaveDataToFile();
				saved = true;
			}
			print("Application quit");
			Application.Quit();	// Exit everything
		// quit application;
		}

		if(Time.time > nextRecordTime && Time.time > warmupTimeFinish) {
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
			warmupTimeFinish = Time.time + warmupTime;
		}
	}

    /// <summary>
    /// Sets the simulation state to all participants that needs to know.
    /// </summary>
	private void SetupSimulationState() {
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

    /// <summary>
    /// Save data to file when the simulation is finished.
    /// </summary>
	private void SaveDataToFile() {
		string data = computerInformation.ToString() + "\n" + testData.ToString();
		string filename = "test";
		filename += DateTime.Now.ToString("yyyMMddHHmmss");
		filename += ".txt";
		System.IO.File.WriteAllText(filename, data);
		print("data saved!!");
	}
}
