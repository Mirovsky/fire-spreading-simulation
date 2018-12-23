using UnityEngine;
using Unity.Mathematics;


public class SimulationSettings : MonoBehaviour
{
    [Header("Terrain Settings")]
    public Terrain simulationTerrain;
    [Range(10, 15000)]
    public int plantsCount;
    public float neighborSize;

    [Header("Plants Settings")]
    public GameObject plantsPrefab;

    [Header("Render Settings")]
    public Material plantMaterial;
    public Color alive;
    public Color dead;
    public Color onFire;

    [Header("Fire Settings")]
    public float cooldownRate;


    [HideInInspector]
    public bool isRunning = false;
    [HideInInspector]
    public PlantsDatabase plantsDatabase;
    [HideInInspector]
    public SinglePlantSpawner singleSpawner;
    [HideInInspector]
    public FieldPlantSpawner fieldSpawner;
    [HideInInspector]
    public float2 windDirection = new float2(0, 0);
    [HideInInspector]
    public float windSpeed = 0f;


    void Awake()
    {
        plantsDatabase = new PlantsDatabase(this);
        singleSpawner = new SinglePlantSpawner();
        fieldSpawner = new FieldPlantSpawner();

        singleSpawner.InjectSettings(this);
        fieldSpawner.InjectSettings(this);
    }
}
