using ThreeInARow.Infrastructure.ADT;
using ThreeInARow.Infrastructure.ValueObjects;

namespace ThreeInARow.Progression.Events.Events;

public record ElementsCleared<TElement> : IEvent
{
    public required NonEmptyList<TElement> Elements { get; init; }
}