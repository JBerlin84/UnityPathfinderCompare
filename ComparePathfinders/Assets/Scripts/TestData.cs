// File: TestData.cs
// Description: Datatype for storing test data during the simulation.
// Date: 2018-01-27
// Written by: Jimmy Berlin

using System.Collections.Generic;

/// <summary>
/// Datatupe for storing test data during the simulation.
/// </summary>
public class TestData {
	Dictionary<string, Dictionary<GameState, List<int>>> recordedData;

    /// <summary>
    /// Creates empty object.
    /// </summary>
	public TestData() {
		recordedData = new Dictionary<string, Dictionary<GameState, List<int>>>();
	}

    /// <summary>
    /// Record new data from simulation.
    /// </summary>
    /// <param name="simulationState">Current state of simulation.</param>
    /// <param name="gameState">Current state of game.</param>
    /// <param name="fps">Current fps to store.</param>
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

    /// <summary>
    /// Returns a nicely formatted version of the data.
    /// </summary>
    /// <returns>string representation of data.</returns>
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
