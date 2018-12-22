using UnityEngine;
using Unity.Entities;


public sealed class Simulation
{
    public static SimulationSettings settings;
    
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void InitializeAtSceneLoad()
    {
        var settingsObject = GameObject.Find("Settings");
        settings = settingsObject.GetComponent<SimulationSettings>();
        
        World.Active.GetOrCreateManager<CooldownSystem>().InjectSettings(settings);
        World.Active.GetOrCreateManager<FirePropagationSystem>().InjectSettings(settings);
        World.Active.GetOrCreateManager<FuelConsumptionSystem>().InjectSettings(settings);
        World.Active.GetOrCreateManager<PlantFireRendererSystem>().InjectSettings(settings);
    }
}
