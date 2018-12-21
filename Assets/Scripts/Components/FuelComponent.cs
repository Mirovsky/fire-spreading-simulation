using Unity.Entities;


[System.Serializable]
public struct Fuel : IComponentData
{
    public float fuel;
}

public class FuelComponent : ComponentDataWrapper<Fuel> {}