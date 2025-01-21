namespace ThreeInARow.Board;

public class EmojiRegistry
{
    private readonly Dictionary<Guid, string> _emojiIdToEmoji = new ();

    public Guid RegisterGameEmoji(string emoji)
    {
        var id = Guid.NewGuid();
        _emojiIdToEmoji[id] = emoji;

        return id;
    }

    public string Render(Guid emojiId) => _emojiIdToEmoji[emojiId];

    public Guid RandomGameEmoji => _emojiIdToEmoji.Keys.ElementAt(Random.Shared.Next(0, _emojiIdToEmoji.Count));
}