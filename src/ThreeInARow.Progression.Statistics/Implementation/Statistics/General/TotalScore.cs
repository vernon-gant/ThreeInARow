using OneOf;
using OneOf.Types;
using ThreeInARow.Infrastructure.ValueObjects;
using ThreeInARow.Progression.Statistics.ADT;

namespace ThreeInARow.Progression.Statistics.Implementation.Statistics.General;

public class TotalScore(IScoreTracker scoreTracker) : IStatistic
{
    public NonEmptyString Name => "Total Score".ToNonEmptyString();

    public Optional<NonEmptyString> Description => new None();

    public Optional<NonEmptyString> Unit => new None();

    public OneOf<NonEmptyString, NotEnoughData> Value => scoreTracker.CurrentScore.ToNonEmptyString();
}