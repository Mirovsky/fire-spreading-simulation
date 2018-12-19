using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;


public sealed class Simulation
{
    public static SimulationSettings Settings;
    public static SpawnedPlantsManager PlantsManager;

    public static bool isRuning = false;

    public static float2 windDirection = new float2();
    public static float windSpeed = 0f;

    public static void Initialize()
    {
        var entityManager = World.Active.GetOrCreateManager<EntityManager>();
        
        World.Active.GetOrCreateManager<SyncTransformSystem>().Enabled = false;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void InitializeAtSceneLoad()
    {
        var settings = GameObject.Find("Settings");

        Settings = settings.GetComponent<SimulationSettings>();
        PlantsManager = settings.GetComponent<SpawnedPlantsManager>();
        PlantsManager.Initialize();

        Initialize();
    }
}
