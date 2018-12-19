using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;


[UpdateBefore(typeof(FirePropagationSystem))]
public class FuelConsumptionSystem : ComponentSystem
{
    public struct Group
    {
        public readonly int Length;

        public ComponentArray<Fuel> fuel;

        [ReadOnly]
        public ComponentArray<Heat> heat;
    }

    [Inject]
    Group group;

    protected override void OnUpdate()
    {
        if (!Simulation.isRuning) return;

        for (var i = 0; i < group.Length; i++) {
            if (group.heat[i].heat > group.heat[i].combustionThreshold) {
                group.fuel[i].fuel -= group.heat[i].radiationRate * Time.deltaTime;
                group.fuel[i].fuel = math.max(group.fuel[i].fuel, 0f);
            }
        }
    }
}
