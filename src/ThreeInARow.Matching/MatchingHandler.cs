using MediatR;

using ThreeInARow.Matching;

namespace ThreeInARow.Core;

public struct HasPotentialMatches : IRequest<bool>
{
    public required Guid[,] EmojiIds { get; init; }
}

public struct FindMatches : IRequest<List<Match>>
{
    public required Guid[,] EmojiIds { get; init; }
}

public class MatchingHandler(List<MatchingStrategy> matchingStrategies) : IRequestHandler<HasPotentialMatches, bool>, IRequestHandler<FindMatches, List<Match>>
{
    public Task<bool> Handle(HasPotentialMatches request, CancellationToken cancellationToken)
    {
        return Task.FromResult(matchingStrategies.Any(strategy => strategy.HasPotentialMatch(request.EmojiIds)));
    }
    public Task<List<Match>> Handle(FindMatches request, CancellationToken cancellationToken)
    {
        var matches = matchingStrategies.SelectMany(strategy => strategy.FindMatches(request.EmojiIds)).ToList();
        var result = new List<Match>();

        while (matches.Count > 0)
        {
            var match = matches.First();
            matches.Remove(match);
            var mergedMatches = new List<Match> { match };

            foreach (var otherMatch in matches)
            {
                if (!match.Intersects(otherMatch)) continue;

                match = match.Merge(otherMatch);
                mergedMatches.Add(otherMatch);
            }

            matches = matches.Except(mergedMatches).ToList();
            result.Add(match);
        }

        return Task.FromResult(result);
    }
}