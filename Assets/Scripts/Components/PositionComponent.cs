using Unity.Mathematics;
using Unity.Entities;


[System.Serializable]
public struct Position : IComponentData
{
    public float3 Value;
}

public class PositionComponent : ComponentDataWrapper<Position> {}
