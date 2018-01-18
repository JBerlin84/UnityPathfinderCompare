using System.Collections.Generic;

public class TestData {
	Dictionary<string, Dictionary<GameState, List<int>>> recordedData;

	public TestData() {
		recordedData = new Dictionary<string, Dictionary<GameState, List<int>>>();
	}

	public void Record(string simulationState, GameState gameState, int fps) {

		if(!recordedData.ContainsKey(simulationState))
			recordedData.Add(simulationState, new Dictionary<GameState, List<int>>());
		Dictionary<GameState, List<int>> dictionary;
		recordedData.TryGetValue(simulationState, out dictionary);
		
		if(!dictionary.ContainsKey(gameState))
			dictionary.Add(gameState, new List<int>());
		List<int> list;
		dictionary.TryGetValue(gameState, out list);

		list.Add(fps);
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
