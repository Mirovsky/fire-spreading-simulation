using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Burst;


[BurstCompile]
public class FirePropagationSystem : ComponentSystem
{
    public struct Group
    {
        public readonly int Length;

        public ComponentArray<HeatAccumulator> heatAccumulator;

        [ReadOnly]
        public ComponentArray<Heat> heat;
        [ReadOnly]
        public ComponentArray<Position> position;
        [ReadOnly]
        public ComponentArray<Fuel> fuel;
        [ReadOnly]
        public ComponentArray<Neighbors> neighbors;
    }

    [Inject]
    Group group;

    protected override void OnUpdate()
    {
        if (!Simulation.isRuning) return;

        var maxDistance = Simulation.Settings.neighborSize;
        for (var i = 0; i < group.Length; i++) {
            var neighborsLength = group.neighbors[i].neighbors.Count;
            var position = group.position[i].Value;

            if (group.fuel[i].fuel > 0) {
                var neighbors = group.neighbors[i].neighbors;

                if (group.heat[i].heat > group.heat[i].combustionThreshold) {
                    group.heatAccumulator[i].accumulatedHeat += group.heat[i].radiationRate * Time.deltaTime;
                }

                for (var j = 0; j < neighborsLength; j++) {
                    if (neighbors[j].heat.heat > neighbors[j].heat.combustionThreshold) {
                        var neighborPos = group.neighbors[i].neighbors[j].position.Value;

                        var distance = math.distance(neighborPos, position);
                        var direction = position - neighborPos;
                        var windAngle = ComputeWindAngle(direction);

                        var rateOfFire = group.neighbors[i].neighbors[j].heat.radiationRate * Time.deltaTime;

                        var distanceAcc = (1f - distance / maxDistance);
                        var windAcc = ((1f - windAngle / 180f * Simulation.windSpeed) * 2 - 1);

                        group.heatAccumulator[i].accumulatedHeat += (rateOfFire * distanceAcc) + (rateOfFire * windAcc);
                    }
                }
            }
        }
    }

    float ComputeWindAngle(float3 fireDirection)
    {
        var vec1 = new float2(fireDirection.x, fireDirection.z);
        return Vector2.Angle(Simulation.windDirection, vec1);
    }
}
