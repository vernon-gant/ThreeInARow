using OneOf;
using OneOf.Types;
using ThreeInARow.Infrastructure.ADT;
using ThreeInARow.Progression.Events.Events;
using ThreeInARow.Progression.Statistics.ADT;

namespace ThreeInARow.Progression.Statistics.Implementation.Statistics.General;

public class TotalMatches<TElement> : IStatistic, IEventHandler<MatchFound<TElement>> where TElement : IEquatable<TElement>
{
    private int _totalMatches;

    public string Name => "Total Matches";

    public OneOf<string, None> Description => "Counts all matches including cascades";

    public OneOf<string, None> Unit => new None();

    public OneOf<string, NotEnoughData> Value => _totalMatches.ToString();

    public void Handle(MatchFound<TElement> notification)
    {
        _totalMatches++;
    }
}