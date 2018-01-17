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
	new private ParticleSystem particleSystem = null;

	// Helper objects
	private TestData testData = new TestData();

	// Helper variables
	private float nextRecordTime;
	private float baseLineTest;

	[Header("Global settings")]
	public float gameStateTime;
	public float recordFrequency;

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
	public SimulationStateSettings[] settings = new SimulationStateSettings[4];


	void Awake () {
		fpsCounter = gameObject.GetComponent(typeof(FPSCounter)) as FPSCounter;
		computerInformation = gameObject.GetComponent(typeof(ComputerInformation)) as ComputerInformation;

		QualitySettings.vSyncCount = 0;		// For the love of God, make sure we dont have V-Sync on!!!!!!!
	}

	// Use this for initialization
	void Start () {
		stressLoader = Instantiate(stressLoaderPrefab, stressLoaderSpawnPosition, Quaternion.identity) as Stresser;
		stressLoader.gameObject.SetActive(false);
		particleSystem = Instantiate(particleSystemPrefab, particleSystemSpawnPosition, Quaternion.identity) as ParticleSystem;
		particleSystem.gameObject.SetActive(false);

		// nextRecordTime = Time.time + recordFrequency;
		// baseLineTest = Time.time + gameStateTime;
	}
	
	// Update is called once per frame
	void Update () {
		


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

	// public void SetState(GameState gameState) {
	// 	this.gameState = gameState;
	// }

	// public void SimulationFinished() {
	// 	print("We are now finished and are packing up everything!!!");
	// 	print(testData.ToString());
	// }
}
