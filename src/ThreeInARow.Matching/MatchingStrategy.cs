using ThreeInARow.Matching;

namespace ThreeInARow.Core;

public interface MatchingStrategy
{
    List<Match> FindMatches(Guid[,] emojiIds);

    bool HasPotentialMatch(Guid[,] emojiIds);
}