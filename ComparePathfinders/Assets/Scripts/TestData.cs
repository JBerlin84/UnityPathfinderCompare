using System.Collections.Generic;

public class TestData {
	/*
	private List<int> baselineFPS = new List<int>();
	private List<int> builtInFPS = new List<int>();
	private List<int> aStarSingleThreadFPS = new List<int>();
	private List<int> aStarMultiThreadFPS = new List<int>();

	//public List<int> BaselineFPS { get { return baselineFPS; } }
	*/

	Dictionary<string, Dictionary<GameState, List<int>>> recordedData;

	public TestData() {
		recordedData = new Dictionary<string, Dictionary<GameState, List<int>>>();
	}

	public void Record(string simulationState, GameState gameState, int fps) {

		if(!recordedData.ContainsKey(simulationState))
			recordedData.Add(simulationState, new Dictionary<GameState, List<int>>());
		Dictionary<GameState, List<int>> d;
		recordedData.TryGetValue(simulationState, out d);
		
		if(!d.ContainsKey(gameState))
			d.Add(gameState, new List<int>());
		List<int> l;
		d.TryGetValue(gameState, out l);

		l.Add(fps);
	}

	public override string ToString() {
		string s = "";

		foreach(KeyValuePair<string, Dictionary<GameState, List<int>>> simulationStateData in recordedData) {
			foreach(KeyValuePair<GameState, List<int>> gameStateData in simulationStateData.Value) {
				s += simulationStateData.Key + ":" + gameStateData.Key + ":\n";
				foreach(int i in gameStateData.Value) {
					s += i + "; ";
				}
				s += "\n";
			}
		}

		return s;		
	}

}
