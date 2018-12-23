using UnityEngine;
using Unity.Entities;
using UnityEngine.UI;


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

    public LMBTools currentTool = LMBTools.SELECT_PLANT;

    [SerializeField]
    SimulationSettings settings;

    [SerializeField]
    PlantsInfoController plantsInfoController;

    [SerializeField]
    ToolToggle toolsToggle;

    [Header("Visuals")]
    [SerializeField]
    Image bgImage;


    EntityManager manager;
    PlantsDatabase database;

    SyncTransformSystem syncTransforms;
    SinglePlantSpawner singleSpawner;
    FieldPlantSpawner fieldSpawner;

    Camera mainCamera;
    LayerMask terrainMask;
    LayerMask plantsMask;

    Color notSimulatingBackground = new Color(0.2641509f, 0.2641509f, 0.2641509f, 0.8f);
    Color simulatingBackground = new Color(0.4f, 0, 0, 0.8f);


    public void Generate()
    {
        Clear();

        fieldSpawner.Spawn(settings.plantsCount);
    }

    public void Clear()
    {
        fieldSpawner.RemoveAll();
    }

    public void LightUp()
    {
        var origins = Random.Range(1, 3);

        for (var i = 0; i < origins; i++) {
            var plant = database.GetRandom();
            var entity = plant.GetComponent<GameObjectEntity>().Entity;

            var heat = manager.GetComponentData<Heat>(entity);
            var accumulator = manager.GetComponentData<HeatAccumulator>(entity);
            accumulator.accumulatedHeat = heat.maximumHeat;
            manager.SetComponentData(entity, accumulator);
        }
    }

    public void PlantsCountChange(float plants)
    {
        settings.plantsCount = (int)plants;

        Debug.Log(Mathf.Log(plants, 2));
    }

    public void Simulate(bool simulate)
    {
        settings.isRunning = simulate;

        currentTool = LMBTools.SELECT_PLANT;
        toolsToggle.EnableInteractions(!simulate);

        bgImage.color = simulate ? simulatingBackground : notSimulatingBackground;
    }

    public void ToggleTool(LMBTools tool)
    {
        currentTool = tool;
    }

    public void WindSpeedChange(float speed)
    {
        settings.windSpeed = speed / 100f;
    }

    public void WindDirectionChange(float direction)
    {
        var radianDirection = direction * Mathf.Deg2Rad;

        settings.windDirection.x = Mathf.Cos(radianDirection);
        settings.windDirection.y = Mathf.Sin(radianDirection);
    }

    public void Quit()
    {
        Application.Quit();
    }


    void Start()
    {
        manager = World.Active.GetOrCreateManager<EntityManager>();
        syncTransforms = World.Active.GetOrCreateManager<SyncTransformSystem>();

        singleSpawner = settings.singleSpawner;
        fieldSpawner = settings.fieldSpawner;

        mainCamera = Camera.main;

        terrainMask = LayerMask.NameToLayer("Terrain");
        plantsMask = LayerMask.NameToLayer("Plants");

        WindSpeedChange(100f);
        WindDirectionChange(0);

        database = settings.plantsDatabase;
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
            plantsInfoController.ClearSelection();
            singleSpawner.Spawn(point.x, point.z);
        }
    }

    void RemovePlantHandler()
    {
        GameObject plant;

        if (GetPlant(out plant)) {
            plantsInfoController.ClearSelection();
            singleSpawner.Remove(plant);
        }
    }

    void ToggleFireHandler()
    {
        GameObject plant;

        if (GetPlant(out plant)) {
            var entity = plant.GetComponent<GameObjectEntity>().Entity;

            var heat = manager.GetComponentData<Heat>(entity);
            var accumulator = manager.GetComponentData<HeatAccumulator>(entity);
            
            if (accumulator.accumulatedHeat > 0) {
                accumulator.accumulatedHeat = 0;
            } else {
                accumulator.accumulatedHeat = heat.maximumHeat;
            }
            
            manager.SetComponentData(entity, accumulator);
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
