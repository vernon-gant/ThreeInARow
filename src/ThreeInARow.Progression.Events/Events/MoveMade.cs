using ThreeInARow.Infrastructure.ADT;

namespace ThreeInARow.Progression.Events.Events;

public record MoveMade : IEvent
{
    public required bool ProducedMatch { get; init; }
}