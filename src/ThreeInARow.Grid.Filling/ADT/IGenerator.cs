using OneOf;
using ThreeInARow.Grid.ADT;

namespace ThreeInARow.Grid.Filling.ADT;

public interface IGenerator<TElement> where TElement : IEquatable<TElement>
{
    void Generate(IReadableGrid<TElement> grid);

    OneOf<Queue<TElement>, ColumnIndexOutOfBounds, NotGeneratedYet> ForColumn(int columnIndex);

    void Reset();
}

public struct NotGeneratedYet;