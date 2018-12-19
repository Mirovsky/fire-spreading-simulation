using UnityEngine;
using Unity.Entities;
using Unity.Collections;

public class PlantFireRendererSystem : ComponentSystem
{
    public struct Group
    {
        public readonly int Length;
        public ComponentArray<MeshRenderer> mesh;

        [ReadOnly]
        public ComponentArray<Fuel> fuel;
        [ReadOnly]
        public ComponentArray<Heat> heat;
    }

    [Inject]
    Group group;

    MaterialPropertyBlock propertyBlock;

    protected override void OnCreateManager()
    {
        propertyBlock = new MaterialPropertyBlock();
    }

    protected override void OnUpdate()
    {
        var alive = Simulation.Settings.alive;
        var dead = Simulation.Settings.dead;
        var onFire = Simulation.Settings.onFire;
        
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
