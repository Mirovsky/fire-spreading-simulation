using System.Collections.Generic;
using UnityEngine;


// Move to its own file
public class PlantsLookupItem : IQuadTreeObject
{
    public Position position;
    public Heat heat;

    public PlantsLookupItem(Position p, Heat h)
    {
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

    List<GameObject> plants;
    QuadTree<PlantsLookupItem> plantsLookup;
    int totalPlants;


    public void Initialize()
    {
        var terrainOrigin = Simulation.Settings.simulationTerrain.transform.position;
        var terrainSize = Simulation.Settings.simulationTerrain.terrainData.size;

        var quadTreeRect = new Rect(terrainOrigin.x, terrainOrigin.z, terrainSize.x, terrainSize.z);

        plants = new List<GameObject>(Simulation.Settings.plantsCount);
        plantsLookup = new QuadTree<PlantsLookupItem>(10, quadTreeRect);
    }

    public PlantsLookupItem Add(GameObject gameObject)
    {
        totalPlants++;
        plants.Add(gameObject);

        var lookupItem = new PlantsLookupItem(gameObject.GetComponent<Position>(), gameObject.GetComponent<Heat>());
        plantsLookup.Insert(lookupItem);

        return lookupItem;
    }

    public GameObject Get(int i)
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

        for (var i = 0; i < plants.Count; i++) {
            Destroy(plants[i]);
        }

        plants.Clear();
    }

    void OnDrawGizmos()
    {
        if (drawDebug) {
            plantsLookup.DrawDebug(10f);
        }    
    }
}
