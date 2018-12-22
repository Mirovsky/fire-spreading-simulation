using UnityEngine;
using Unity.Entities;
using Unity.Collections;

public class PlantFireRendererSystem : ComponentSystem, ISettingsInjectable
{
    public struct Group
    {
        public readonly int Length;
        public ComponentArray<MeshRenderer> mesh;

        [ReadOnly]
        public ComponentDataArray<Fuel> fuel;
        [ReadOnly]
        public ComponentDataArray<Heat> heat;
    }

    [Inject]
    Group group;

    SimulationSettings settings;
    MaterialPropertyBlock propertyBlock;

    public void InjectSettings(SimulationSettings s)
    {
        settings = s;
    }

    protected override void OnCreateManager()
    {
        propertyBlock = new MaterialPropertyBlock();
    }

    protected override void OnUpdate()
    {
        var alive = settings.alive;
        var dead = settings.dead;
        var onFire = settings.onFire;
        
        for (var i = 0; i < group.Length; i++) {
            group.mesh[i].GetPropertyBlock(propertyBlock);

            var color = group.fuel[i].fuel <= 0 ?
                Color.Lerp(dead, onFire, group.heat[i].heat / group.heat[i].maximumHeat) :
                Color.Lerp(alive, onFire, group.heat[i].heat / group.heat[i].combustionThreshold);

            propertyBlock.SetColor("_Color", color);

            group.mesh[i].SetPropertyBlock(propertyBlock);
        }
    }
}
