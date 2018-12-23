using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;


public struct SpawnItemData
{
    public float3 position;
    public float4 rotation;
}

public abstract class PlantSpawner : ISettingsInjectable
{
    protected GameObject plantPrefab;

    protected EntityManager manager;
    protected SimulationSettings settings;
    protected PlantsDatabase database;

    protected Terrain terrain;
    protected TerrainData terrainData;
    protected Vector3 terrainSize;
    protected Vector3 terrainOrigin;


    public void InjectSettings(SimulationSettings s)
    {
        manager = World.Active.GetOrCreateManager<EntityManager>();

        settings = s;
        database = s.plantsDatabase;
        
        terrain = settings.simulationTerrain;
        terrainData = terrain.terrainData;
        terrainSize = terrainData.size;
        terrainOrigin = terrain.transform.position;
    }
    
    protected void ComputePlacement(ref SpawnItemData item)
    {
        var height = terrain.SampleHeight(item.position);
        var normal = terrainData.GetInterpolatedNormal((item.position.x - terrainOrigin.x) / terrainSize.x, (item.position.z - terrainOrigin.z) / terrainSize.z);
        var rotation = Quaternion.FromToRotation(Vector3.up, normal);

        item.position.y = height + terrainOrigin.y;
        item.rotation = new float4 { x = rotation.x, y = rotation.y, z = rotation.z, w = rotation.w };
    }

    protected void SetComponentData(Entity entity, SpawnItemData item)
    {
        manager.SetComponentData(entity, new Position { Value = item.position });
        manager.SetComponentData(entity, new Rotation { Value = item.rotation });
        manager.AddBuffer<NeighboursBufferElement>(entity);
    }
}
