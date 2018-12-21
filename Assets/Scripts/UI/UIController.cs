using UnityEngine;
using Unity.Entities;


public class UIController : MonoBehaviour
{
    public enum LMBTools
    {
        NO_TOOL,
        ADD_PLANT,
        REMOVE_PLANT,
        TOGGLE_FIRE,
        SELECT_PLANT
    }

    [SerializeField]
    PlantsInfoController plantsInfoController;

    [SerializeField]
    LMBTools currentTool = LMBTools.SELECT_PLANT;


    SpawnedPlantsManager plantsManager;
    SyncTransformSystem syncTransforms;
    SinglePlantSpawner singleSpawner;
    FieldPlantSpawner fieldSpawner;

    LayerMask terrainMask;
    LayerMask plantsMask;

    Camera mainCamera;

    GameObject selectedPlant = null;

    public void Generate()
    {
        Clear();

        fieldSpawner.Spawn();

        syncTransforms.Enabled = true;
    }

    public void Clear()
    {
        fieldSpawner.Clear();
    }

    public void LightUp()
    {
        var origins = Random.Range(1, 3);

        var count = plantsManager.Count();
        /* for (var i = 0; i < origins; i++) {
            var plant = plantsManager.Get(Random.Range(0, count - 1));

            var heat = plant.GetComponent<Heat>();
            heat.heat = heat.maximumHeat; 
        } */
    }

    public void Simulate(bool simulate)
    {
        Simulation.isRuning = simulate;
    }

    public void ToggleTool(LMBTools tool)
    {
        this.currentTool = tool;
    }

    public void WindSpeedChange(float speed)
    {
        Simulation.windSpeed = speed / 100f;
    }

    public void WindDirectionChange(float direction)
    {
        var radianDirection = direction * Mathf.Deg2Rad;

        Simulation.windDirection.x = Mathf.Cos(radianDirection);
        Simulation.windDirection.y = Mathf.Sin(radianDirection);
    }

    public void Quit()
    {
        Application.Quit();
    }


    void Start()
    {
        plantsManager = Simulation.PlantsManager;

        singleSpawner = new SinglePlantSpawner(Simulation.Settings.simulationTerrain, plantsManager, Simulation.Settings);
        fieldSpawner = new FieldPlantSpawner(Simulation.Settings.simulationTerrain, plantsManager, Simulation.Settings);

        syncTransforms = World.Active.GetOrCreateManager<SyncTransformSystem>();

        mainCamera = Camera.main;

        terrainMask = LayerMask.NameToLayer("Terrain");
        plantsMask = LayerMask.NameToLayer("Plants");

        WindSpeedChange(100f);
        WindDirectionChange(0);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            HandleToolInteraction();
        }
    }

    void HandleToolInteraction()
    {
        switch (currentTool) {
            case LMBTools.ADD_PLANT:
                AddPlantHandler();
                break;
            case LMBTools.REMOVE_PLANT:
                RemovePlantHandler();
                break;
            case LMBTools.TOGGLE_FIRE:
                ToggleFireHandler();
                break;
            case LMBTools.SELECT_PLANT:
                SelectPlantHandler();
                break;
        }
    }

    void AddPlantHandler()
    {
        Vector3 point;

        if (GetTerrainPoint(out point)) {
            singleSpawner.Spawn(point.x, point.z);

            syncTransforms.Enabled = true;
        }
    }

    void RemovePlantHandler()
    {
        GameObject plant;

        if (GetPlant(out plant)) {
            
        }
    }

    void ToggleFireHandler()
    {
        GameObject plant;
        if (GetPlant(out plant)) {
            var heat = plant.GetComponent<Heat>();
            var accumulator = plant.GetComponent<HeatAccumulator>();

            if (accumulator.accumulatedHeat > 0) {
                accumulator.accumulatedHeat = 0;
            } else {
                accumulator.accumulatedHeat = heat.maximumHeat;
            }
        }
    }

    void SelectPlantHandler()
    {
        GameObject plant;

        if (GetPlant(out plant)) {
            plantsInfoController.UpdatePlant(plant);
        }
    }

    bool GetTerrainPoint(out Vector3 point)
    {
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit terrainHit;
        
        if (Physics.Raycast(cameraRay, out terrainHit, Mathf.Infinity, 1<<terrainMask)) {
            point = terrainHit.point;

            return true;
        }

        point = Vector3.one;
        return false;
    }

    bool GetPlant(out GameObject plant)
    {
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit plantHit;
        
        if (Physics.Raycast(cameraRay, out plantHit, Mathf.Infinity, 1<<plantsMask)) {
            plant = plantHit.collider.gameObject;

            return true;
        }

        plant = null;
        return false;
    }

}
