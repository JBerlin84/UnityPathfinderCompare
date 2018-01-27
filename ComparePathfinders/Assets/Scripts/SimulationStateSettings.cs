// File: SimulationStateSettings.cs
// Description: Information about simulation state.
// Date: 2018-01-27
// Written by: Jimmy Berlin

using System;

/// <summary>
/// State information for simulation state.
/// </summary>
[Serializable]
public class SimulationStateSettings {
    public string settingsName;
    public bool useStressLoader;
    public int stressLoaderMagnitude;
    public bool useParticleSystem;
    public int particleSystemMagnitude;
}