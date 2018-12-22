using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Jobs;


[UpdateBefore(typeof(FirePropagationSystem))]
public class FuelConsumptionSystem : JobComponentSystem, ISettingsInjectable
{
    public struct FuelConsumptionJob : IJobProcessComponentData<Fuel, Heat>
    {
        public float deltaTime;

        [BurstCompile]
        public void Execute(ref Fuel fuel, ref Heat heat)
        {
            if (heat.heat > heat.combustionThreshold && fuel.fuel > 0)
            {
                fuel.fuel -= heat.radiationRate * deltaTime;
                fuel.fuel = math.max(fuel.fuel, 0f);
            }
        }
    }


    SimulationSettings settings;

    public void InjectSettings(SimulationSettings s)
    {
        settings = s;
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        if (!settings.isRunning) return inputDeps;

        var job = new FuelConsumptionJob
        {
            deltaTime = Time.deltaTime
        };

        return job.Schedule(this, inputDeps);
    }
}
