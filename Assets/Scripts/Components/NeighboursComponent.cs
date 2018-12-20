using Unity.Entities;


public struct NeighboursBufferElement : IBufferElementData
{
    public static implicit operator PlantsLookupItem(NeighboursBufferElement e) { return e.Value; }

    public PlantsLookupItem Value;
}
