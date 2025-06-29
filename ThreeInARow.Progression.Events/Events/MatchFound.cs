using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Infrastructure.ADT;

namespace ThreeInARow.Progression.Events.Events;

public record MatchFound<TElement> : IEvent where TElement : IEquatable<TElement>
{
    public required IMatch<TElement> Match { get; init; }

    public required bool IsCascade { get; init; }
}