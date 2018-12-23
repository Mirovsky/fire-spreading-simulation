using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;


[UpdateAfter(typeof(FirePropagationSystem))]
public class CooldownSystem : JobComponentSystem, ISettingsInjectable
{
    SimulationSettings settings;

    public struct CooldownJob : IJobProcessComponentData<HeatAccumulator>
    {
        public float cooldownRate;

        [BurstCompile]
        public void Execute(ref HeatAccumulator heatAccumulator)
        {
            heatAccumulator.accumulatedHeat -= cooldownRate;
        }
    }

    
    public void InjectSettings(SimulationSettings s)
    {
        settings = s;
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        if (!settings.isRunning) return inputDeps;

        var job = new CooldownJob
        {
            cooldownRate = settings.cooldownRate * Time.deltaTime
        };

        return job.Schedule(this, inputDeps);
    }
}
