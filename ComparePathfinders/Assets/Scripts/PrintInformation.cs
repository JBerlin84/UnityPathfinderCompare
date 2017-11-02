using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintInformation : MonoBehaviour {

	ComputerInformation info = new ComputerInformation();

	// Use this for initialization
	void Start () {
		print(info.ToString());
	}
}
