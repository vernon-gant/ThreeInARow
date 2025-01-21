using ThreeInARow.Matching;

namespace ThreeInARow.Core;

public class VerticalMatching(int minimalMatchLength) : MatchingStrategy
{
    public List<Match> FindMatches(Guid[,] emojiIds)
    {
        var matches = new List<Match>();

        for (var column = 0; column < emojiIds.GetLength(1); column++)
        {
            for (var row = 0; row <= emojiIds.GetLength(0) - minimalMatchLength; row++)
            {
                var currentWindow = Enumerable.Range(row, minimalMatchLength).Select(r => (r, column)).ToArray();
                var emoji = emojiIds[currentWindow[0].r, currentWindow[0].column];
                var allSame = currentWindow.Select(p => emojiIds[p.r, p.column]).All(e => e == emoji);

                if (!allSame) continue;

                var elementViews = currentWindow.Select(p => new Position(p.r, p.column)).ToHashSet();
                matches.Add(new Match(emoji, elementViews));
            }
        }

        return matches;
    }

    public bool HasPotentialMatch(Guid[,] emojiIds)
    {
        var windowSize = minimalMatchLength + 1;

        for (var column = 0; column < emojiIds.GetLength(1); column++)
        {
            for (var row = 0; row <= emojiIds.GetLength(0) - windowSize; row++)
            {
                var currentWindow = Enumerable.Range(row, windowSize).Select(r => emojiIds[r, column]).ToArray();
                var emojisIdOccurrence = currentWindow.GroupBy(e => e).ToDictionary(g => g.Key, g => g.Count());

                if (emojisIdOccurrence.Count == 2) return true;
            }
        }

        return false;
    }
}