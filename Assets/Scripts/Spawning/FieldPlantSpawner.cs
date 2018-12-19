using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class FieldPlantSpawner : PlantSpawner
{
    public FieldPlantSpawner(Terrain t, SpawnedPlantsManager p, SimulationSettings s) : base(t, p, s)
    {
        settings = s;
    }

    public void Spawn()
    {
        // TODO: Object pooling maybe?
        for (var i = 0; i < settings.plantsCount; i++) {
            var plant = UnityEngine.Object.Instantiate(settings.plantsPrefab);
            var positionRotation = ComputePlantPositionAndRotation();

            SetComponentData(plant, positionRotation);

            plantsManager.Add(plant);
        }

        plantsManager.drawDebug = true;

        var distance = settings.neighborSize;
        for (var i = 0; i < settings.plantsCount; i++) {
            var go = plantsManager.Get(i);

            var pos = go.GetComponent<Position>().Value;
            var neighbors = go.GetComponent<Neighbors>();
            
            neighbors.neighbors = new List<PlantsLookupItem>(
                plantsManager.GetSurroudings((int)(pos.x - distance / 2), (int)(pos.z - distance / 2), (int)distance, (int)distance)
            );
        }
    }

    public void Clear()
    {
        plantsManager.Clear();
    }

    Tuple<float3, float4> ComputePlantPositionAndRotation()
    {
        var plantXWorld = UnityEngine.Random.Range(terrainOrigin.x, terrainOrigin.x + terrainSize.x);
        var plantZWorld = UnityEngine.Random.Range(terrainOrigin.z, terrainOrigin.z + terrainSize.z);

        return ComputePlantPositionAndRotation(plantXWorld, plantZWorld);
    }

}

