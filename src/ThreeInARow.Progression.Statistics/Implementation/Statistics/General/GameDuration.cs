using OneOf;
using ThreeInARow.Infrastructure.ADT;
using ThreeInARow.Progression.Statistics.ADT;
using ThreeInARow.ValueObjects;

namespace ThreeInARow.Progression.Statistics.Implementation.Statistics.General;

public class GameDuration(IGameTimer gameTimer) : IStatistic
{
    public NonEmptyString Name => "Game Duration".ToNonEmptyString();

    public Optional<NonEmptyString> Description => "Total time from start to end of the game".ToNonEmptyString();

    public Optional<NonEmptyString> Unit => "mm:ss".ToNonEmptyString();

    public OneOf<NonEmptyString, NotEnoughData> Value => gameTimer.ElapsedGameTime.Match<OneOf<NonEmptyString, NotEnoughData>>(time => $"{time.Minutes:D2}:{time.Seconds:D2}".ToNonEmptyString(), _ => new NotEnoughData());
}