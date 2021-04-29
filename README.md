# CQRS + Event Sourcing + .NET 5.0

```
CQRS stands for "Command and Query Responsiblity Segregation"
Where segregation means separation.
```

This project contains basic understanding of how **CQRS + Event Sourcing** works togather.  
Add :star: if this repository helps you.

## Features

- Commands and Handlers
- Domain Models (Aggregates)
- Aggregate Repository
- Event Sourcing
- Events and Handlers
- ReadModels
- Query Controllers
- Command and Event handlers auto register
- Unit of Work for aggregates
- Class for sequential GUIDs (CombGuid.cs)
- Snapshots and Snapshot repository

## Tips:

- Inherit domain model with `AggregateRoot` or `SnapshotAggregateRoot<Snapshot>`.
- Domain model must have parameterless constructor with `private` access modifier.
- Add `private` modifier to set of all properties in domain model.
- Domain has no dependecies except Contracts.
- All business logic and validation should be written in Domain models.
- Contracts contains Command and Event classes, no business logic should be written in Contracts.
- All database updates are done from `EventHandler` only.
- `QueryController` is featching data from thin data layer.

