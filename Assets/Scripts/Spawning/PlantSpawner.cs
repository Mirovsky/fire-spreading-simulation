using System;
using UnityEngine;
using Unity.Mathematics;


public abstract class PlantSpawner
{
    protected GameObject plant;

    protected SimulationSettings settings;
    protected SpawnedPlantsManager plantsManager;
    protected Terrain terrain;
    protected TerrainData terrainData;
    protected Vector3 terrainSize;
    protected Vector3 terrainOrigin;

    public PlantSpawner(Terrain t, SpawnedPlantsManager p, SimulationSettings s)
    {
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

    protected void SetComponentData(GameObject plantEntity, Tuple<float3, float4> positionRotation)
    {
        var position = plantEntity.GetComponent<Position>().Value = positionRotation.Item1;
        var rotation = plantEntity.GetComponent<Rotation>().Value = positionRotation.Item2;
    }
}
