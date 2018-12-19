using UnityEngine;
using Unity.Entities;


[UpdateAfter(typeof(FirePropagationSystem))]
public class CooldownSystem : ComponentSystem
{
    public struct Group
    {
        public readonly int Length;
        public ComponentArray<HeatAccumulator> heatAccumulator;
    }

    [Inject]
    Group group;

    protected override void OnUpdate()
    {
        if (!Simulation.isRuning) return;

        for (var i = 0; i < group.Length; i++) {
            group.heatAccumulator[i].accumulatedHeat -= Simulation.Settings.cooldownRate * Time.deltaTime;
        }
    }
}
