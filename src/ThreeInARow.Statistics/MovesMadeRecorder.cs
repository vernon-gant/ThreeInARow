using MediatR;

using ThreeInARow.GameEvents;

namespace ThreeInARow.Statistics;

public class MovesMadeRecorder : StatisticUnit, INotificationHandler<MoveMade>
{
    private int _movesMade = 0;

    public string Name { get; } = "Moves Made";

    public string Description { get; } = "The number of moves made by the player.";

    public string Value => _movesMade.ToString();

    public Task Handle(MoveMade notification, CancellationToken cancellationToken)
    {
        _movesMade++;

        return Task.CompletedTask;
    }
}