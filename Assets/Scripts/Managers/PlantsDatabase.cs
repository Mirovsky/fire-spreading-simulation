using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;


public class PlantsDatabase
{
    EntityManager manager;
    List<GameObject> plants;
    QuadTree<NeighboursComponent> plantsLookup;
    

    public PlantsDatabase(SimulationSettings settings)
    {
        var terrainOrigin = settings.simulationTerrain.transform.position;
        var terrainSize = settings.simulationTerrain.terrainData.size;
        
        manager = World.Active.GetOrCreateManager<EntityManager>();
        plants = new List<GameObject>();
        plantsLookup = new QuadTree<NeighboursComponent>(10, new Rect(terrainOrigin.x, terrainOrigin.z, terrainSize.x, terrainSize.z));
    }

    public void Add(GameObject plant)
    {
        var entity = plant.GetComponent<GameObjectEntity>().Entity;
        var position = manager.GetComponentData<Position>(entity);
        var lookup = plant.GetComponent<NeighboursComponent>();
        
        plants.Add(plant);

        lookup.entity = entity;
        lookup.position = position;

        plantsLookup.Insert(lookup);
    }
    
    public GameObject GetRandom()
    {
        return plants[Random.Range(0, plants.Count - 1)];
    }

    public int Count()
    {
        return plants.Count;
    }
    
    public List<NeighboursComponent> GetNeighbours(int x, int y, int w, int h)
    {
        var rect = new Rect(x, y, w, h);
    
        return plantsLookup.RetrieveObjectsInArea(rect);
    }

    public void Remove(GameObject plant)
    {
        var neighbour = plant.GetComponent<NeighboursComponent>();

        plantsLookup.Remove(neighbour);
        plants.Remove(plant);    
    }

    public void Clear()
    {
        plantsLookup.Clear();
        for (var i = 0; i < plants.Count; i++) {
            Object.Destroy(plants[i]);
        }
        plants.Clear();
    }
}
