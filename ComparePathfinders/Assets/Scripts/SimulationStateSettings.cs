using System;

[Serializable]
public class SimulationStateSettings {
    public string settingsName;
    public bool useStressLoader;
    public int stressLoaderMagnitude;
    public bool useParticleSystem;
    public int particleSystemMagnitude;
}