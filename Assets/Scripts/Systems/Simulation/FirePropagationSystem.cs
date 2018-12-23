using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;


public class FirePropagationSystem : JobComponentSystem, ISettingsInjectable
{
    [BurstCompile]
    public struct FirePropagationJob : IJobParallelFor
    {
        public float deltaTime;
        public float neightbourSize;
        public float windSpeed;
        public float2 windDirection;

        public ComponentDataArray<HeatAccumulator> heatAccumulators;

        [ReadOnly]
        public ComponentDataFromEntity<Heat> entityHeat;
        [ReadOnly]
        public ComponentDataFromEntity<Position> entityPositions;

        [ReadOnly]
        public ComponentDataArray<Heat> heat;
        [ReadOnly]
        public ComponentDataArray<Position> positions;
        [ReadOnly]
        public ComponentDataArray<Fuel> fuel;
        [ReadOnly]
        public BufferArray<NeighboursBufferElement> neighbours;

        public void Execute(int index)
        {
            var accumulator = heatAccumulators[index];

            if (fuel[index].fuel > 0) {
                if (heat[index].heat > heat[index].combustionThreshold) {
                    accumulator.accumulatedHeat += heat[index].radiationRate * deltaTime;
                }

                var currentNeighbours = neighbours[index];
                var neighborsLength = currentNeighbours.Length;
                var position = positions[index].Value;
                
                for (var j = 0; j < neighborsLength; j++) {
                    var currentEntity = currentNeighbours[j].entity;
                    var currentHeat = entityHeat[currentEntity];

                    if (currentHeat.heat > currentHeat.combustionThreshold) {
                        var neighbourPos = entityPositions[currentEntity].Value;

                        var distance = math.distance(neighbourPos, position);
                        var direction = position - neighbourPos;
                        var windAngle = ComputeWindAngle(direction);

                        var rateOfFire = currentHeat.radiationRate * deltaTime;

                        var distanceAcc = (1f - distance / neightbourSize);
                        var windAcc = ((1f - windAngle / 180f) * 2 - 1) * windSpeed;

                        accumulator.accumulatedHeat += (rateOfFire * distanceAcc) + (rateOfFire * windAcc);
                    }
                }
            }

            heatAccumulators[index] = accumulator;
        }

        float ComputeWindAngle(float3 fireDirection)
        {
            var vec1 = new float2(fireDirection.x, fireDirection.z);
            return Vector2.Angle(windDirection, vec1);
        }
    }

    public struct Group
    {
        public readonly int Length;
        
        public ComponentDataArray<HeatAccumulator> heatAccumulator;
        
        [ReadOnly]
        public ComponentDataArray<Heat> heat;
        [ReadOnly]
        public ComponentDataArray<Position> position;
        [ReadOnly]
        public ComponentDataArray<Fuel> fuel;
        [ReadOnly]
        public BufferArray<NeighboursBufferElement> neighbours;
    }


    [Inject]
    Group group;

    [Inject]
    ComponentDataFromEntity<Heat> entityHeat;

    [Inject]
    ComponentDataFromEntity<Position> entityPositions;

    SimulationSettings settings;

    public void InjectSettings(SimulationSettings s)
    {
        settings = s;
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        if (!settings.isRunning) return inputDeps;

        var job = new FirePropagationJob {
            deltaTime = Time.deltaTime,
            neightbourSize = settings.neighborSize,
            windSpeed = settings.windSpeed,
            windDirection = settings.windDirection,

            entityHeat = entityHeat,
            entityPositions = entityPositions,

            heatAccumulators = group.heatAccumulator,
            positions = group.position,
            heat = group.heat,
            fuel = group.fuel,
            neighbours = group.neighbours
        };

        return job.Schedule(group.Length, 32, inputDeps);
    }
}
