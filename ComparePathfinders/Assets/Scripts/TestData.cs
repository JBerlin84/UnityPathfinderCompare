using System.Collections.Generic;

public class TestData {
	private List<int> baselineFPS = new List<int>();
	private List<int> builtInFPS = new List<int>();
	private List<int> aStarSingleThreadFPS = new List<int>();
	private List<int> aStarMultiThreadFPS = new List<int>();

	//public List<int> BaselineFPS { get { return baselineFPS; } }

	public void Record(GameState gameState, int fps) {
		switch(gameState) {
			case GameState.BASE_LINE:
				baselineFPS.Add(fps);
				break;
			case GameState.BUILT_IN_PATHFINDER:
				builtInFPS.Add(fps);
				break;
			case GameState.A_STAR_PATH_FINDER_SINGLE_THREAD:
				aStarSingleThreadFPS.Add(fps);
				break;
			case GameState.A_STAR_PATH_FINDER_MULTI_THREAD:
				aStarMultiThreadFPS.Add(fps);
				break;
		}
	}

	public override string ToString() {
		string s = "";
		s += "Baseline: " + Concatenate(baselineFPS) + "\n";
		s += "Built in: " + Concatenate(builtInFPS) + "\n";
		s += "A* Singlethread: " + Concatenate(aStarSingleThreadFPS) + "\n";
		s += "A* Multithread: " + Concatenate(aStarMultiThreadFPS);
		return s;
	}

	private string Concatenate(List<int> list) {
		string s = "";
		for(int i=0;i<list.Count;i++) {
			if(i > 0)
				s+=", ";
			s += list[i];
		}
		return s;
	}
}
