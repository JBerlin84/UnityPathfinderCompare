using UnityEngine;
using System.Diagnostics;

public class ComputerInformation : MonoBehaviour {

	string deviceType;
	string operatingSystem;
	string logicalCores;
	string processorType;
	string processorFrequency;
	string systemMemorySize;
	string graphicsDeviceVendor;
	string graphicsDeviceName;
	string graphicsDeviceType;
	string graphicsMemorySize;

	Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();

	private float time;
	public float measureFrequency = 1;

	void Awake () {
		deviceType = SystemInfo.deviceType.ToString();
		operatingSystem = SystemInfo.operatingSystem.ToString();
		logicalCores = SystemInfo.processorCount.ToString();
		processorType = SystemInfo.processorType.ToString();
		processorFrequency = SystemInfo.processorFrequency.ToString();
		systemMemorySize = SystemInfo.systemMemorySize.ToString();
		graphicsDeviceVendor = SystemInfo.graphicsDeviceVendor.ToString();
		graphicsDeviceName = SystemInfo.graphicsDeviceName.ToString();
		graphicsDeviceType = SystemInfo.graphicsDeviceType.ToString();
		graphicsMemorySize = SystemInfo.graphicsMemorySize.ToString();

		print(ToString());
		time = Time.time;
	}

	void Update() {
		if(Time.time > time + 1) {
			print(printMeasurementStatistics());
			time = Time.time;
		}
	}

	public override string ToString() {
		string reply = 	"Device type: " + deviceType + "\n" +
						"Operating system: " + operatingSystem + "\n" +
						"Processor type: " + processorType + "\n" +
						"Logical cores: " + logicalCores + "\n" +
						"Processor frequency: " + processorFrequency + "\n" +
						"System memory size: " + systemMemorySize + "\n" +
						"Graphics device vendor: " + graphicsDeviceVendor + "\n" +
						"Graphics device name: " + graphicsDeviceName + "\n" +
						"Graphics device type: " + graphicsDeviceType + "\n" +
						"Graphics memory size: " + graphicsMemorySize + "\n"; 					
		return reply;
	}

	public string printMeasurementStatistics() {
		string reply = "Memory used: " + currentProcess.WorkingSet64;
		return reply;
	}
}
