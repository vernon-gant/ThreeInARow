namespace ThreeInARow.Statistics;

public interface StatisticUnit
{
    public string Name { get; }

    public string Description { get; }

    public string Value { get; }
}