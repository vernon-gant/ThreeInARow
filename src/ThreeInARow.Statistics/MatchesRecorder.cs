using System.Text;

using MediatR;

using ThreeInARow.Board;
using ThreeInARow.GameEvents;

namespace ThreeInARow.Statistics;

public class MatchesRecorder(EmojiRegistry emojiRegistry) : StatisticUnit, INotificationHandler<MatchFound>
{
    private readonly Dictionary<Guid, int> _emojiMatches = new ();

    public string Name => "Matches Made";

    public string Description => "The number of matches for each emoji.";

    public string Value
    {
        get
        {
            var sb = new StringBuilder();

            foreach (var emojiId in _emojiMatches.Keys)
            {
                sb.AppendLine($"{emojiRegistry.Render(emojiId)}: {_emojiMatches[emojiId]}");
            }

            return sb.ToString();
        }
    }

    public Task Handle(MatchFound notification, CancellationToken cancellationToken)
    {
        var currentCount = _emojiMatches.GetValueOrDefault(notification.FoundMatch.EmojiId, 0);

        _emojiMatches[notification.FoundMatch.EmojiId] = currentCount + 1;

        return Task.CompletedTask;
    }
}