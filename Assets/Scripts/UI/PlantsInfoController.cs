using UnityEngine;
using UnityEngine.UI;

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

    LineRenderer debugLines;

    GameObject currentPlant;
    Transform currentPosition;
    Fuel currentFuel;
    HeatAccumulator currentAccumulator;
    Neighbors currentNeighbors;
    Heat currentHeat;

    public void UpdatePlant(GameObject plant)
    {
        currentPlant = plant;
        currentPosition = plant.transform;
        currentNeighbors = plant.GetComponent<Neighbors>();
        currentFuel = plant.GetComponent<Fuel>();
        currentHeat = plant.GetComponent<Heat>();

        plantName.text = currentPlant.name;

        UpdateDebugRays();
    }

    void Start()
    {
        debugLines = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (currentPlant == null) return;

        heat.text = currentHeat.heat.ToString();
        fuel.text = currentFuel.fuel.ToString();
        neighbors.text = currentNeighbors.neighbors.Count.ToString();

        debugLines.enabled = drawRays;
    }

    void UpdateDebugRays()
    {
        var count = currentNeighbors.neighbors.Count;

        debugLines.positionCount = count * 2;
        var vertices = new Vector3[count * 2];

        var currentVertexPos = 0;
        for (var i = 0; i < count; i++) {
            vertices[currentVertexPos] = currentPosition.position;
            vertices[currentVertexPos + 1] = currentNeighbors.neighbors[i].position.Value;

            currentVertexPos += 2;
        }

        debugLines.SetPositions(vertices);

        debugLines.enabled = drawRays;
    }
}
