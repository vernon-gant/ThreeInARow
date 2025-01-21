using ThreeInARow.Matching;

namespace ThreeInARow.Core;

public class HorizontalMatching(int minimalMatchLength) : MatchingStrategy
{
    public List<Match> FindMatches(Guid[,] emojiIds)
    {
        var matches = new List<Match>();

        for (var row = 0; row < emojiIds.GetLength(0); row++)
        {
            for (var column = 0; column <= emojiIds.GetLength(1) - minimalMatchLength; column++)
            {
                var currentWindow = Enumerable.Range(column, minimalMatchLength).Select(c => (row, c)).ToArray();
                var emoji = emojiIds[row, currentWindow[0].c];
                var allSame = currentWindow.Select(p => emojiIds[p.row, p.c]).All(e => e == emoji);

                if (!allSame) continue;

                var elementViews = currentWindow.Select(p => new Position(p.row, p.c)).ToHashSet();
                matches.Add(new Match(emoji, elementViews));
            }
        }

        return matches;
    }

    public bool HasPotentialMatch(Guid[,] emojiIds)
    {
        var windowSize = minimalMatchLength + 1;

        for (var row = 0; row < emojiIds.GetLength(0); row++)
        {
            for (var column = 0; column <= emojiIds.GetLength(1) - windowSize; column++)
            {
                var currentWindow = Enumerable.Range(column, windowSize).Select(c => emojiIds[row, c]).ToArray();
                var emojisIdOccurrence = currentWindow.GroupBy(e => e).ToDictionary(g => g.Key, g => g.Count());

                if (emojisIdOccurrence.Count == 2) return true;
            }
        }

        return false;
    }
}