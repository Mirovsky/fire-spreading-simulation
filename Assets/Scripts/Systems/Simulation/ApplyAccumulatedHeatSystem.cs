using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Burst;


[UpdateAfter(typeof(CooldownSystem))]
public class ApplyAccumulatedHeatSystem : JobComponentSystem
{
    public struct ApplyAccumulatedHeatJob : IJobProcessComponentData<Heat, HeatAccumulator>
    {

        [BurstCompile]
        public void Execute(ref Heat heat, ref HeatAccumulator heatAccumulator)
        {
            heat.heat += heatAccumulator.accumulatedHeat;
            heat.heat = math.clamp(heat.heat, 0f, heat.maximumHeat);
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        if (!Simulation.isRuning) return inputDeps;

        var job = new ApplyAccumulatedHeatJob {};

        return job.Schedule(this, inputDeps);
    }
}
