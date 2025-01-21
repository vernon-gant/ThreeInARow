using ThreeInARow.Core;

namespace ThreeInARow.Matching;

public class Match(Guid emojiId, HashSet<Position> elements)
{
    private readonly HashSet<Position> _elements = elements;

    public List<Position> Elements => _elements.ToList();

    public Guid EmojiId => emojiId;

    public int Count => _elements.Count;

    public List<int> Columns => _elements.Select(e => e.Column).ToList();

    public bool Intersects(Match match) => _elements.Any(e => match._elements.Any(m => e == m));

    public Match Merge(Match match)
    {
        var elements = new HashSet<Position>(_elements);
        elements.UnionWith(match._elements);

        return new Match(emojiId, elements);
    }

    public bool Contains(Match match) => match._elements.All(e => _elements.Contains(e));

    public bool IsVertical() => _elements.All(e => e.Column == _elements.First().Column);

    public bool IsHorizontal() => _elements.All(e => e.Row == _elements.First().Row);
}