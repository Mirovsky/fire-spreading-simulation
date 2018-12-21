# Fire Spreading Simulation

Simple fire spreading simulation in Unity. Using Unity 2018.3. Using new Unity ECS implementation with hybrid approach also with some other preview packages.

## Controls

| Controls | Action |
|---|---|
| WASD / Arrow Keys / Middle Mouse | Movement |
| Click | Action button |
| ScrollWheen | Zoom |


## Implementation

### Packages used:
- Entities
- Collections
- Mathemathics
- Jobs
- Burst

Implementation is quite simple. Using QuadTree for spacial queries for neighbor plants in certain radius. This is done only on two occasions. Either by generating field of plants, then the query is run for each plant, or when adding/removing single plant. Then the query is done for single plant and all neighbors are updated with newly added entity.

QuadTree was used because of simplicity. Initially I was looking into creating mesh from plants using Delaunay Triangulation, but implementation with some at least optimal adding and removing elements proved to be difficult in given time span. This can be optimized further. Using M-Tree data structure could be beneficial as well, since single plant needs to know about surrounding plants in circle.

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
