# CQRS + ES + DDD + .NET 5.0

<img src="https://raw.githubusercontent.com/NilavPatel/Todo.CQRS/main/Docs/cqrs-banner.png">

`CQRS` stands for **Command and Query Responsibility Segregation**  
`ES` stands for **Event Sourcing**  
`DDD` stands for **Domain Driven Design**  

## Features

- Commands and handlers
- Domain models (Aggregates)
- Aggregate repository
- Event sourcing using [EventStore](https://www.eventstore.com/)
- Domain events and handlers (Projections)
- Integration events and handlers (Cross module projections)
- Read models
- Query controllers
- Unit of work, Read repository, Write repository
- Auto registration of commands, events and their handlers
- Unit of work for multiple aggregates
- Snapshots, Snapshot repository, Snapshot storage in eventstore db
- Checkpoint, Checkpoint repository, Checkpoint storage in eventstore db
- Class for sequential GUIDs
- Entity tracking for write repository and no tracking for read repository

## Notes:

- Inherit domain model with `AggregateRoot` or `SnapshotAggregateRoot<Snapshot>`.
- Domain model must have a parameterless constructor with a `private` access modifier.
- Set all setters with `private` modifier to all properties in the domain model.
- `Domain` layer has no dependencies except `Contracts` layer.
- All business logic and validation should be written in domain models.
- `Contracts` contain Command and Event classes, no business logic should be written in `Contracts`.
- All database updates are done from `EventHandler` only.
- Use `IReadRepository` for query controllers.
- Use sequential Guids `CombGuid.NewGuid()` instead of `Guid.NewGuid()`.
- Use `IUnitOfWork` to update data in event handlers.
- Start background processor before starting web app
- AggregateVersion start from 1, where in EventStore event version start from 0.

## [How to configure Eventstore?](https://github.com/NilavPatel/Todo.CQRS/blob/main/Docs/EventStore_Config.md)

## Diagrams:

![CQRS](https://raw.githubusercontent.com/NilavPatel/Todo.CQRS/main/Docs/CQRS.png)
![AggregateRepository-GetAggregate](https://raw.githubusercontent.com/NilavPatel/Todo.CQRS/main/Docs/AggregateRepository-GetAggregate.png)
