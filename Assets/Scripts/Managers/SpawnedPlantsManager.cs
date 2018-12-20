using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;


// Move to its own file
public struct PlantsLookupItem : IQuadTreeObject
{
    public Entity entity;
    public Position position;
    public Heat heat;

    public PlantsLookupItem(Entity e, Position p, Heat h)
    {
        entity = e;
        position = p;
        heat = h;
    }

    public Vector2 GetPosition()
    {
        return new Vector2(position.Value.x, position.Value.z);
    }
}

public class SpawnedPlantsManager : MonoBehaviour
{
    public bool drawDebug = false;

    EntityManager manager;
    List<Entity> plants;
    QuadTree<PlantsLookupItem> plantsLookup;
    int totalPlants;


    public void Initialize()
    {
        var terrainOrigin = Simulation.Settings.simulationTerrain.transform.position;
        var terrainSize = Simulation.Settings.simulationTerrain.terrainData.size;

        var quadTreeRect = new Rect(terrainOrigin.x, terrainOrigin.z, terrainSize.x, terrainSize.z);

        manager = World.Active.GetOrCreateManager<EntityManager>();
        plants = new List<Entity>(Simulation.Settings.plantsCount);
        plantsLookup = new QuadTree<PlantsLookupItem>(10, quadTreeRect);
    }

    public PlantsLookupItem Add(Entity entity)
    {
        totalPlants++;

        var position = manager.GetComponentData<Position>(entity);
        var heat = manager.GetComponentData<Heat>(entity);

        plants.Add(entity);

        var lookup = new PlantsLookupItem(entity, position, heat);
        plantsLookup.Insert(lookup);

        return lookup;
    }

    public Entity Get(int i)
    {
        return plants[i];
    }

    public int Count()
    {
        return totalPlants;
    }
    
    public List<PlantsLookupItem> GetSurroudings(int x, int y, int w, int h)
    {
        var rect = new Rect(x, y, w, h);
    
        return plantsLookup.RetrieveObjectsInArea(rect);
    }

    public void Clear()
    {
        plantsLookup.Clear();

        /* for (var i = 0; i < plants.Count; i++) {
            Destroy(plants[i]);
        } */

        plants.Clear();
    }

    void OnDrawGizmos()
    {
        if (drawDebug) {
            plantsLookup.DrawDebug(10f);
        }    
    }
}
