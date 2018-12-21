using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;


public class SinglePlantSpawner : PlantSpawner
{
    public SinglePlantSpawner(Terrain t, SpawnedPlantsManager p, SimulationSettings s) : base(t, p, s) {}

    public void Spawn(float worldX, float worldZ)
    {
        var plant = Object.Instantiate(Simulation.Settings.plantsPrefab);
        var positionRotation = ComputePlantPositionAndRotation(worldX, worldZ);
        var entity = plant.GetComponent<GameObjectEntity>().Entity;

        SetComponentData(entity, positionRotation);

        var lookup = plantsManager.Add(entity);
        // AddNeighbors(plant, worldX, worldZ, lookup);
    }

    public void Remove(GameObject plant)
    {
        // var neighbors = plant.GetComponent<Neighbors>().neighbors;

        /* for (var i = 0; i < neighbors.Count; i++) {
            var neighborNeighbors = neighbors[i].position.gameObject.GetComponent<Neighbors>().neighbors;

            var index = FindNeighbor(neighborNeighbors, plant);
            if (index != -1) {
                neighborNeighbors.RemoveAt(index);
            }
        } */
    }

    void AddNeighbors(GameObject plant, float x, float z, PlantsLookupItem lookup)
    {
        var distance = settings.neighborSize;
       /* var neighborsComponent = plant.GetComponent<Neighbors>();
        neighborsComponent.neighbors = new List<PlantsLookupItem>(
            plantsManager.GetSurroudings((int)(x - distance / 2), (int)(z - distance / 2), (int)distance, (int)distance)
        );

        Debug.Log(neighborsComponent.neighbors.Count);

        /* for (var i = 0; i < neighborsComponent.neighbors.Count; i++) {
            var neighborNeightbors = neighborsComponent.neighbors[i].position.gameObject.GetComponent<Neighbors>().neighbors;

            // Could potentially slowdown game.
            // Would be a good idea to implement some limit on number of neighbors.
            neighborNeightbors.Add(lookup);
        } */
    }

    // Prone to break with this chaining.
    int FindNeighbor(List<PlantsLookupItem> neighborNeighbors, GameObject toBeFound)
    {
        for (var j = 0; j < neighborNeighbors.Count; j++) {
            /* if (neighborNeighbors[j].position.gameObject == toBeFound) {
                return j;
            } */
        }

        return -1;
    }
}
