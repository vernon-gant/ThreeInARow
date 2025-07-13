using ThreeInARow.Progression.Statistics.ADT;
using ThreeInARow.ValueObjects;

namespace ThreeInARow.Progression.Statistics.Implementation;

public class DefaultScoreTracker : IScoreTracker
{
    public PositiveInt CurrentScore { get; private set; } = new();

    public void IncrementScore(PositiveInt increment) => CurrentScore += increment;

    public void ResetScore() => CurrentScore = new PositiveInt();
}