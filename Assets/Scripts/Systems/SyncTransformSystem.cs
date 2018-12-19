using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;


public class SyncTransformSystem : ComponentSystem
{
    public struct Data
    {
        [ReadOnly]
        public Position position;
        [ReadOnly]
        public Rotation rotation;

        public Transform outputTransform;
    }

    protected override void OnUpdate()
    {
        foreach (var entity in GetEntities<Data>()) {
            float3 pos = entity.position.Value;
            float4 rot = entity.rotation.Value;

            entity.outputTransform.position = pos;
            entity.outputTransform.rotation = new Quaternion(rot.x, rot.y, rot.z, rot.w);

            var localPos = entity.outputTransform.localPosition;
            localPos.y += .5f;
            entity.outputTransform.localPosition = localPos;
        }

        Enabled = false;
    }
}
