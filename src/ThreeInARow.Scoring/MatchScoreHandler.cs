using MediatR;

using ThreeInARow.Core;
using ThreeInARow.GameEvents;

namespace ThreeInARow.Scoring;

public class MatchScoreHandler(ScoreTracker scoreTracker) : INotificationHandler<MatchFound>
{
    private const int POINTS_PER_ELEMENT = 10;

    public Task Handle(MatchFound notification, CancellationToken cancellationToken)
    {
        var points = new PositiveInt(POINTS_PER_ELEMENT * notification.FoundMatch.Count);
        scoreTracker.Add(points);

        return Task.CompletedTask;
    }
}