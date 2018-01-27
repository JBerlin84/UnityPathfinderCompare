// File: ComputerInformation.cs
// Description: Collects as much information about your computer as possible.
// Date: 2018-01-27
// Written by: Jimmy Berlin

using System;
using UnityEngine;

/// <summary>
/// Collects as much information about your computer as possible
/// </summary>
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

	private float time;
	public float measureFrequency = 1;

    /// <summary>
    /// Unitys version of constructor-sort of.
    /// </summary>
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
	}

    /// <summary>
    /// Returns string with all the computer information neatly formatted
    /// </summary>
    /// <returns>String of computer information</returns>
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
}
