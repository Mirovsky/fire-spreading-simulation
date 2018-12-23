# Fire Spreading Simulation

Simple fire spreading simulation in Unity. Using Unity 2018.3. Using new Unity ECS implementation with hybrid approach also with some other preview packages.

## Controls

| Controls | Action |
|---|---|
| WASD / Arrow Keys / Middle Mouse | Movement |
| Click | Action button |
| ScrollWheen | Zoom |


## Implementation

### Preview packages used:
- Entities
- Collections
- Mathemathics
- Jobs
- Burst

Code is using new Unity ECS and data oriented approach.

Implementation is quite simple. Using QuadTree for spatial queries for neighbour plants in certain rectangle area. This is done only on two occasions. Either by generating field of plants, then the query is run for each plant. When adding/removing single plant query is done for new plant and all neighbours are updated with newly added entity. Removing is done in a similar fashion, item is removed from QuadTree and for each of his neighbours is run the spatial query again.

QuadTree was used because of its simplicity. Initially I was looking into creating mesh from plants using Delaunay triangulation, but implementation with some at least optimal adding and removing elements proved to be difficult in given timespan. This can be optimized further. Using M-Tree data structure could be beneficial as well, since single plant needs to know about surrounding plants in circle.

All timeconsuming parts runs on worker threads.


### Systems
- **FirePropagationSystem**
  - takes care of calculating spreading of fire to neighbor plants
- **CooldownSystem**
  - cooldowns plants by some fixed amount
- **FuelConsumptionSystem**
  - subtracts fixed amount of fuel from plant when on fire
- **ApplyAccumulatedHeatSystem**
  - takes all accumulated heat and applies it to each plant
  - enables use of multithreading for FirePropagationSystem and minimizing chance of accidental race conditions
g
