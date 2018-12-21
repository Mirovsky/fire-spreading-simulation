using Unity.Entities;


[System.Serializable]
public struct Heat : IComponentData
{
    public float combustionThreshold;

    public float radiationRate;
    public float heat;
    public float maximumHeat;
}

public class HeatComponent : ComponentDataWrapper<Heat> {}
