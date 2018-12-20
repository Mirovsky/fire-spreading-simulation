using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;


public class FirePropagationSystem : JobComponentSystem
{
    public struct FirePropagationJob : IJobParallelFor
    {
        public float deltaTime;
        public float neightbourSize;
        public float windSpeed;
        public float2 windDirection;

        [WriteOnly]
        public ComponentDataArray<HeatAccumulator> heatAccumulators;

        [ReadOnly]
        public ComponentDataArray<Position> positions;
        [ReadOnly]
        public ComponentDataArray<Heat> heat;
        [ReadOnly]
        public ComponentDataArray<Fuel> fuel;
        [ReadOnly]
        public BufferArray<NeighboursBufferElement> neighbours;

        [BurstCompile]
        public void Execute(int index)
        {
            var accumulator = new HeatAccumulator { accumulatedHeat = 0 };

            if (fuel[index].fuel > 0)
            {
                if (heat[index].heat > heat[index].combustionThreshold)
                {
                    accumulator.accumulatedHeat += heat[index].radiationRate * deltaTime;
                }

                var neighborsLength = neighbours[index].Length;
                var position = positions[index].Value;
                var currentNeighbours = neighbours[index];

                for (var j = 0; j < neighborsLength; j++)
                {
                    var currentHeat = currentNeighbours[j].Value.heat;

                    if (currentHeat.heat > currentHeat.combustionThreshold)
                    {
                        var neighbourPos = currentNeighbours[j].Value.position.Value;

                        var distance = math.distance(neighbourPos, position);
                        var direction = position - neighbourPos;
                        var windAngle = ComputeWindAngle(direction);

                        var rateOfFire = currentHeat.radiationRate * deltaTime;

                        var distanceAcc = (1f - distance / neightbourSize);
                        var windAcc = ((1f - windAngle / 180f * windSpeed) * 2 - 1);

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

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        if (!Simulation.isRuning) return inputDeps;

        var job = new FirePropagationJob {
            deltaTime = Time.deltaTime,
            neightbourSize = Simulation.Settings.neighborSize,
            windSpeed = Simulation.windSpeed,
            windDirection = Simulation.windDirection,

            heatAccumulators = group.heatAccumulator,
            positions = group.position,
            heat = group.heat,
            fuel = group.fuel,
            neighbours = group.neighbours
        };

        return job.Schedule(group.Length, 32, inputDeps);
    }
}
