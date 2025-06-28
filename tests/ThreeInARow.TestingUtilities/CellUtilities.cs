using NSubstitute;
using ThreeInARow.Grid.ADT;
using ThreeInARow.Grid.ValueObjects;
using OneOf;

namespace ThreeInARow.TestingUtilities;

public static class CellExtensions
{
    public static Cell<T> CreateTestCell<T>(T content, int row, int column)
    {
        var gridMock = Substitute.For<IReadableGrid<T>>();
        var result = OneOf<CellContent<T>, CellOutOfBounds>.FromT0(content);
        gridMock.ContentAt(row, column).Returns(result);
        var cellResult = Cell<T>.FromGrid(gridMock, row, column);
        return cellResult.AsT0;
    }
}