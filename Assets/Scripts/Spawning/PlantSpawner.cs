using System;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;


public abstract class PlantSpawner
{
    protected GameObject plantPrefab;

    protected EntityManager manager;
    protected SimulationSettings settings;
    protected SpawnedPlantsManager plantsManager;

    protected Terrain terrain;
    protected TerrainData terrainData;
    protected Vector3 terrainSize;
    protected Vector3 terrainOrigin;


    protected PlantSpawner(Terrain t, SpawnedPlantsManager p, SimulationSettings s)
    {
        manager = World.Active.GetOrCreateManager<EntityManager>();

        plantsManager = p;
        settings = s;
        terrain = t;
        terrainData = terrain.terrainData;
        terrainSize = terrainData.size;
        terrainOrigin = terrain.transform.position;
    }

    protected Tuple<float3, float4> ComputePlantPositionAndRotation(float worldX, float worldZ)
    {
        var height = terrain.SampleHeight(new Vector3(worldX, 0, worldZ));
        var normal = terrainData.GetInterpolatedNormal((worldX - terrainOrigin.x) / terrainSize.x, (worldZ - terrainOrigin.z) / terrainSize.z);

        var objectRotation = Quaternion.FromToRotation(Vector3.up, normal);

        var position = new float3 { x = worldX + terrainOrigin.x, y = height + terrainOrigin.y, z = worldZ + terrainOrigin.z };
        var rotation = new float4 { x = objectRotation.x, y = objectRotation.y, z = objectRotation.z, w = objectRotation.w };

        return new Tuple<float3, float4>(position, rotation);
    }

    protected void SetComponentData(Entity entity, Tuple<float3, float4> positionRotation)
    {
        manager.SetComponentData(entity, new Position { Value = positionRotation.Item1 });
        manager.SetComponentData(entity, new Rotation { Value = positionRotation.Item2 });
        manager.SetComponentData(entity, new HeatAccumulator { });

        manager.AddBuffer<NeighboursBufferElement>(entity);

        /* manager.SetComponentData(entity, new Heat { });
        manager.SetComponentData(entity, new Fuel { });
        manager.SetComponentData(entity, new HeatAccumulator { }); */
    }
}
