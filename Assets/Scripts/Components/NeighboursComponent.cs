using UnityEngine;
using Unity.Entities;


public struct NeighboursBufferElement : IBufferElementData
{
    public Entity entity;
    public Position position;
}


public class NeighboursComponent : MonoBehaviour, IQuadTreeObject
{
    public Entity entity;
    public Position position;


    public Vector2 GetPosition()
    {
        return new Vector2(position.Value.x, position.Value.z);
    }
}
