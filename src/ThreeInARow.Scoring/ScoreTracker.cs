namespace ThreeInARow.Core;

public class ScoreTracker
{
    public PositiveInt TotalScore { get; private set; } = new ();

    public void Add(PositiveInt positiveInt) => TotalScore += positiveInt;

    public void Multiply(MultiplyFactor factor) => TotalScore = TotalScore.Multiply(factor);
}