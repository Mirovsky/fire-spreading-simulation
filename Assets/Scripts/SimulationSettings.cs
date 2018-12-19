using UnityEngine;


public class SimulationSettings : MonoBehaviour
{
    [Header("Terrain Settings")]
    public Terrain simulationTerrain;
    [Range(1, 10000)]
    public int plantsCount;
    public Material plantMaterial;
    public float neighborSize;

    [Header("Plants Settings")]
    public GameObject plantsPrefab;
    public float fuel;
    public float combustionThreshold;
    public float cooldownRate;

    [Header("Wind Settings")]
    public float maxWindSpeed;

    [Header("Color Settings")]
    public Color alive;
    public Color dead;
    public Color onFire;
}
