using OneOf;
using OneOf.Types;
using ThreeInARow.Progression.Statistics.ADT;

namespace ThreeInARow.Progression.Statistics.Implementation.Statistics;

public class TotalScore(IScoreTracker scoreTracker) : IStatistic
{
    public string Name => "Total Score";

    public OneOf<string, None> Description => new None();

    public OneOf<string, None> Unit => new None();

    public OneOf<string, NotEnoughData> Value => scoreTracker.CurrentScore.ToString();
}