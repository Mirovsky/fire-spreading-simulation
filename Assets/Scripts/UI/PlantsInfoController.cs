using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;

[RequireComponent(typeof(LineRenderer))]
public class PlantsInfoController : MonoBehaviour
{
    [SerializeField]
    Text plantName;
    [SerializeField]
    Text heat;
    [SerializeField]
    Text fuel;
    [SerializeField]
    Text neighbors;
    [SerializeField]
    Toggle drawRays;

    EntityManager manager;
    LineRenderer debugLines;

    GameObject currentPlant;

    Transform currentPosition;
    Fuel currentFuel;
    HeatAccumulator currentAccumulator;
    DynamicBuffer<NeighboursBufferElement> currentNeighbors;
    Heat currentHeat;

    public void ClearSelection()
    {
        currentPlant = null;
        debugLines.enabled = false;
    }

    public void UpdatePlant(GameObject plant)
    {
        var entity = plant.GetComponent<GameObjectEntity>().Entity;

        currentNeighbors = manager.GetBuffer<NeighboursBufferElement>(entity);

        currentPlant = plant;
        currentPosition = plant.transform;

        plantName.text = currentPlant.name;
        UpdateDebugRays();
    }

    void Start()
    {
        manager = World.Active.GetOrCreateManager<EntityManager>();
        debugLines = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (currentPlant == null) return;

        var entity = currentPlant.GetComponent<GameObjectEntity>().Entity;

        if (!manager.Exists(entity)) {
            currentPlant = null;
            debugLines.enabled = false;

            return;
        };

        currentFuel = manager.GetComponentData<Fuel>(entity);
        currentHeat = manager.GetComponentData<Heat>(entity);

        heat.text = currentHeat.heat.ToString();
        fuel.text = currentFuel.fuel.ToString();
        neighbors.text = currentNeighbors.Length.ToString();

        debugLines.enabled = drawRays;
    }

    void UpdateDebugRays()
    {
        var count = currentNeighbors.Length;

        debugLines.positionCount = count * 2;
        var vertices = new Vector3[count * 2];

        var currentVertexPos = 0;
        for (var i = 0; i < count; i++) {
            var entityPosition = manager.GetComponentData<Position>(currentNeighbors[i].entity).Value;

            vertices[currentVertexPos] = currentPosition.position;
            vertices[currentVertexPos + 1] = entityPosition;

            currentVertexPos += 2;
        }

        debugLines.SetPositions(vertices);

        debugLines.enabled = drawRays;
    }
}
