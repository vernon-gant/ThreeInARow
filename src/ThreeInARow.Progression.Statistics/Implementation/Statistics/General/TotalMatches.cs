using OneOf;
using OneOf.Types;
using ThreeInARow.Infrastructure.ADT;
using ThreeInARow.Infrastructure.ValueObjects;
using ThreeInARow.Progression.Events.Events;
using ThreeInARow.Progression.Statistics.ADT;

namespace ThreeInARow.Progression.Statistics.Implementation.Statistics.General;

public class TotalMatches<TElement> : IStatistic, IEventHandler<MatchFound<TElement>> where TElement : IEquatable<TElement>
{
    private int _totalMatches;

    public NonEmptyString Name => "Total Matches".ToNonEmptyString();

    public Optional<NonEmptyString> Description => "Counts all matches including cascades".ToNonEmptyString();

    public Optional<NonEmptyString> Unit => new None();

    public OneOf<NonEmptyString, NotEnoughData> Value => _totalMatches.ToString().ToNonEmptyString();

    public void Handle(MatchFound<TElement> notification) => _totalMatches++;
}