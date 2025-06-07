namespace ThreeInARow.Grid.Tests;

public abstract class GridTestsBase<TGrid>
{
    protected TGrid _grid = default!;

    protected abstract TGrid CreateGrid(string?[,] gridData);

    protected string?[,] EmptyGrid(int rows, int columns) => new string?[rows, columns];
}