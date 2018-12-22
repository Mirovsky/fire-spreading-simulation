using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;


public class SinglePlantSpawner : PlantSpawner
{
    public void Spawn(float worldX, float worldZ)
    {
        var plant = Object.Instantiate(settings.plantsPrefab);
        var item = new SpawnItemData { position = new float3 { x = worldX, y = 0, z = worldZ } };

        ComputePlacement(ref item);
        var entity = plant.GetComponent<GameObjectEntity>().Entity;

        SetComponentData(entity, item);

        database.Add(plant);

        AddNeighbors(plant);
    }

    public void Remove(GameObject plant)
    {
        var entity = plant.GetComponent<GameObjectEntity>().Entity;

        database.Remove(plant);

        var size = settings.neighborSize;
        var buffer = manager.GetBuffer<NeighboursBufferElement>(entity);
        for (var i = 0; i < buffer.Length; i++) {
            var position = manager.GetComponentData<Position>(buffer[i].entity);
            var neighboursBuffer = manager.GetBuffer<NeighboursBufferElement>(buffer[i].entity);
            neighboursBuffer.Clear();

            var newNeighbours = database.GetNeighbours((int)(position.Value.x - size), (int)(position.Value.z - size), (int)(size * 2), (int)(size * 2));
            for (var e = 0; e < newNeighbours.Count; e++) {
                if (buffer[i].entity != newNeighbours[e].entity) {
                    neighboursBuffer.Add(new NeighboursBufferElement { entity = newNeighbours[e].entity });
                }
            }
        }
        
        Object.Destroy(plant);
    }

    void AddNeighbors(GameObject plant)
    {
        var entity = plant.GetComponent<GameObjectEntity>().Entity;
        var size = settings.neighborSize;

        var position = manager.GetComponentData<Position>(entity);

        var neighbourComponent = plant.GetComponent<NeighboursComponent>();
        neighbourComponent.entity = entity;
        neighbourComponent.position = position;

        var neighbours = database.GetNeighbours((int)(position.Value.x - size), (int)(position.Value.z - size), (int)(size * 2), (int)(size * 2));

        var buffer = manager.GetBuffer<NeighboursBufferElement>(entity);
        for (var e = 0; e < neighbours.Count; e++) {
            if (entity != neighbours[e].entity) {
                buffer.Add(new NeighboursBufferElement { entity = neighbours[e].entity, position = neighbours[e].position });

                var neighboursBuffer = manager.GetBuffer<NeighboursBufferElement>(neighbours[e].entity);
                neighboursBuffer.Add(new NeighboursBufferElement { entity = entity });
            }
        }
    }
}
