using Unity.Entities;


[System.Serializable]
public struct HeatAccumulator : IComponentData
{
    public float accumulatedHeat;
}

public class HeatAccumulatorComponent : ComponentDataWrapper<HeatAccumulator> {}
