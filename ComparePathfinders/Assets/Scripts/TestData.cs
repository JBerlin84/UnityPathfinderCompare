using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestData {
	private List<int> baselineFPS = new List<int>();
	private List<int> builtInFPS = new List<int>();
	private List<int> aStarSingleThreadFPS = new List<int>();
	private List<int> aStarMultiThreadFPS = new List<int>();

	public List<int> BaselineFPS { get { return baselineFPS; } }
}
