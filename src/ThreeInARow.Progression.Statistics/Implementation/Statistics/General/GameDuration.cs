using OneOf;
using OneOf.Types;
using ThreeInARow.Infrastructure.ADT;
using ThreeInARow.Progression.Statistics.ADT;

namespace ThreeInARow.Progression.Statistics.Implementation.Statistics.General;

public class GameDuration(IGameTimer gameTimer) : IStatistic
{
    public string Name => "Game Duration";

    public OneOf<string, None> Description => "Total time from start to end of the game";

    public OneOf<string, None> Unit => "mm:ss";

    public OneOf<string, NotEnoughData> Value => !gameTimer.HasGameEnded ? new NotEnoughData() : gameTimer.ElapsedGameTime.Match(t => $"{t.Minutes:D2}:{t.Seconds:D2}", _ => throw new InvalidOperationException("Impossible"));
}