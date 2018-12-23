using UnityEngine;
using UnityEngine.Jobs;
using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;


public class SyncTransformSystem : JobComponentSystem
{
    public struct SyncTransformJob : IJobParallelForTransform
    {
        [ReadOnly]
        public ComponentDataArray<Position> positions;
        [ReadOnly]
        public ComponentDataArray<Rotation> rotations;

        public void Execute(int i, TransformAccess transform)
        {
            var pos = positions[i].Value;
            var rot = rotations[i].Value;

            pos.y += .5f;

            transform.position = pos;
            transform.rotation = new Quaternion(rot.x, rot.y, rot.z, rot.w);
        }
    }

    public struct Data
    {
        public readonly int Length;

        [ReadOnly]
        public ComponentDataArray<Position> positions;
        [ReadOnly]
        public ComponentDataArray<Rotation> rotations;

        public TransformAccessArray transforms;
    }

    [Inject]
    Data data;

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var syncJob = new SyncTransformJob
        {
            positions = data.positions,
            rotations = data.rotations
        };

        return syncJob.Schedule(data.transforms, inputDeps);
    }
}
