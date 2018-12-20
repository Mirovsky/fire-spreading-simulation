using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;


public struct Position : IComponentData
{
    public float3 Value;
}

public class PositionComponent : ComponentDataWrapper<Position> {}
