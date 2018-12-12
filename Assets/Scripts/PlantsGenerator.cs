using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class PlantsGenerator : MonoBehaviour
{
    [SerializeField, Range(100f, 100000f)]
    float plantsAmount = 100f;

    [SerializeField]
    List<GameObject> plantsTemplates;

    Terrain terrain;
    TerrainData terrainData;

    void Start()
    {
        terrain = GetComponent<Terrain>();
        terrainData = terrain.terrainData;

        GeneratePlants();
    }

    void GeneratePlants()
    {
        var terrainSize = terrainData.size;

        Debug.Log(terrainSize);

        var origin = transform.position;
        for (var i = 0; i < plantsAmount; i++) {
            var plantX = Random.Range(0f, 1f);
            var plantZ = Random.Range(0f, 1f);

            var plantXWorld = (int)(plantX * terrainSize.x);
            var plantZWorld = (int)(plantZ * terrainSize.z);

            var height = terrain.SampleHeight(new Vector3(plantXWorld, 0, plantZWorld));
            var normal = terrainData.GetInterpolatedNormal(plantX, plantZ);

            var rotation = Quaternion.FromToRotation(Vector3.up, normal);

            var selectedPlant = plantsTemplates[Random.Range(0, plantsTemplates.Count - 1)];

            var plantPosition = new Vector3(plantXWorld, height, plantZWorld) + origin;
            Instantiate(selectedPlant, plantPosition, rotation);
        }
    }
}
