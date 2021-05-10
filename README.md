# CQRS + ES + DDD + .NET 5.0

`CQRS` stands for **Command and Query Responsibility Segregation**

`ES` stands for **Event Sourcing**

`DDD` stands for **Domain Driven Design**

## Features

- Commands and Handlers
- Domain Models (Aggregates)
- Aggregate Repository
- Event Sourcing
- Events and Handlers
- Read models
- Query Controllers
- Command and Event handler's auto dependency register
- Unit of Work for multiple aggregates
- Class for sequential GUIDs
- Snapshots and Snapshot repository

## Tips:

- Inherit domain model with `AggregateRoot` or `SnapshotAggregateRoot<Snapshot>`.
- Domain model must have a parameterless constructor with a `private` access modifier.
- Set all setters with `private` modifier to all properties in the domain model.
- `Domain` layer has no dependencies except `Contracts` layer.
- All business logic and validation should be written in domain models.
- `Contracts` contain Command and Event classes, no business logic should be written in `Contracts`.
- All database updates are done from `EventHandler` only.
- `QueryController` is fetching data from the thin data layer.
- Use sequential Guids `CombGuid.NewGuid()` instead of `Guid.NewGuid()`

## Diagrams:

![CQRS](https://raw.githubusercontent.com/NilavPatel/Todo.CQRS/main/Diagrams/CQRS.jpg)
