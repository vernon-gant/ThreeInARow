using ThreeInARow.ValueObjects;

namespace ThreeInARow.Progression.Statistics.ADT;

public interface IScoreTracker
{
    PositiveInt CurrentScore { get; }

    void IncrementScore(PositiveInt increment);

    void ResetScore();
}