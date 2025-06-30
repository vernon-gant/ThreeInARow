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

    public OneOf<string, NotEnoughData> Value => gameTimer.ElapsedGameTime.Match<OneOf<string, NotEnoughData>>(time => $"{time.Minutes:D2}:{time.Seconds:D2}", _ => new NotEnoughData());
}