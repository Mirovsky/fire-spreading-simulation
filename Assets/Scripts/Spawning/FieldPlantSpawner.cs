using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class FieldPlantSpawner : PlantSpawner
{
    public void Spawn(int amount)
    {
        var spawnedEntities = new Queue<GameObject>();
        for (var i = 0; i < amount; i++) {
            var plant = UnityEngine.Object.Instantiate(settings.plantsPrefab);

            var x = UnityEngine.Random.Range(terrainOrigin.x, terrainOrigin.x + terrainSize.x);
            var z = UnityEngine.Random.Range(terrainOrigin.z, terrainOrigin.z + terrainSize.z);
            
            var item = new SpawnItemData { position = new float3 { x = x, y = 0, z = z } };

            ComputePlacement(ref item);
            var entity = plant.GetComponent<GameObjectEntity>().Entity;

            SetComponentData(entity, item);

            database.Add(plant);
            spawnedEntities.Enqueue(plant);
        }

        var distance = settings.neighborSize;
        while (spawnedEntities.Count > 0) {
            var entity = spawnedEntities.Dequeue().GetComponent<GameObjectEntity>().Entity;

            var pos = manager.GetComponentData<Position>(entity).Value;
            var neighboursBuffer = manager.GetBuffer<NeighboursBufferElement>(entity);

            var neighbours = database.GetNeighbours(
                (int)(pos.x - distance / 2),
                (int)(pos.z - distance / 2),
                (int)distance,
                (int)distance
            );
            
            for (int e = 0; e < neighbours.Count; e++)
            {
                if (entity != neighbours[e].entity) {
                    neighboursBuffer.Add(new NeighboursBufferElement { entity = neighbours[e].entity });
                }
            }
        }
    }

    public void RemoveAll()
    {
        database.Clear();
    }
}

