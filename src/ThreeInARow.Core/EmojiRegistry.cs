namespace ThreeInARow.Core;

public class EmojiRegistry
{
    private readonly List<string> _emojis = new ();
    private readonly Dictionary<int, string> _elements = new ();

    public const string DESTROYED_EMOJI = "💥";
    public const string EMPTY_EMOJI = "🟦";

    public void Register(string emoji)
    {
        _emojis.Add(emoji);
    }

    public string GetFor(int elementId)
    {
        return _elements.GetValueOrDefault(elementId, EMPTY_EMOJI);
    }

    public void Add(int elementId, string emoji)
    {
        _elements[elementId] = emoji;
    }

    public void Destroy(int elementId)
    {
        _elements[elementId] = DESTROYED_EMOJI;
    }

    public void Clear(int elementId)
    {
        _elements.Remove(elementId);
    }

    public string RandomEmoji() => _emojis[Random.Shared.Next(0, _emojis.Count)];
}