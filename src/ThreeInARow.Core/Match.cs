namespace ThreeInARow.Core;

public class Match
{
    public string Emoji { get; }

    public int Count { get; }

    public bool Intersects(Match match)
    {
        return true;
    }

    public Match Merge(Match match)
    {
        return new Match();
    }

    public bool Overlaps(Match match)
    {
        return true;
    }


}