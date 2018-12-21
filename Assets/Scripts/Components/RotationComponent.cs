using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;

[System.Serializable]
public struct Rotation : IComponentData
{
    public float4 Value;
}

public class RotationComponent : ComponentDataWrapper<Rotation> {}
