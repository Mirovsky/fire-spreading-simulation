using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;


[UpdateAfter(typeof(FirePropagationSystem))]
public class CooldownSystem : JobComponentSystem
{
    public struct CooldownJob : IJobProcessComponentData<HeatAccumulator>
    {
        public float cooldownRate;

        [BurstCompile]
        public void Execute(ref HeatAccumulator heatAccumulator)
        {
            heatAccumulator.accumulatedHeat -= cooldownRate;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        if (!Simulation.isRuning) return inputDeps;

        var job = new CooldownJob
        {
            cooldownRate = Simulation.Settings.cooldownRate * Time.deltaTime
        };

        return job.Schedule(this, inputDeps);
    }
}
