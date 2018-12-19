using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;


[UpdateAfter(typeof(CooldownSystem))]
public class ApplyAccumulatedHeatSystem : ComponentSystem
{
    public struct Group
    {
        public readonly int Length;
        public ComponentArray<Heat> heat;
        public ComponentArray<HeatAccumulator> heatAccumulator;
    }

    [Inject]
    private Group group;

    protected override void OnUpdate()
    {
        if (!Simulation.isRuning) return;

        for (var i = 0; i < group.Length; i++) {
            group.heat[i].heat += group.heatAccumulator[i].accumulatedHeat;
            group.heat[i].heat = math.clamp(group.heat[i].heat, 0f, group.heat[i].maximumHeat);
            group.heatAccumulator[i].accumulatedHeat = 0;
        }
    }
}
