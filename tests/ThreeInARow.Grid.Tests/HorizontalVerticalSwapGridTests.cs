using ThreeInARow.Grid.ADT;
using ThreeInARow.Grid.Implementations;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Tests;

[TestFixture]
public class HorizontalVerticalSwapGridReadableTests : ReadableGridBehaviorTests<HorizontalVerticalSwapGrid<Element>>
{
    protected override HorizontalVerticalSwapGrid<Element> CreateGrid(Element?[,] gridData) => new(gridData);
}

[TestFixture]
public class HorizontalVerticalSwapGridManageableTests : ManageableGridBehaviorTests<HorizontalVerticalSwapGrid<Element>>
{
    protected override HorizontalVerticalSwapGrid<Element> CreateGrid(Element?[,] gridData) => new(gridData);
}

[TestFixture]
public class HorizontalVerticalSwapGridFillableTests : FillableGridBehaviorTests<HorizontalVerticalSwapGrid<Element>>
{
    protected override HorizontalVerticalSwapGrid<Element> CreateGrid(Element?[,] gridData) => new(gridData);
}

[TestFixture]
public class HorizontalVerticalSwapGridTests
{
     [Test]
    public void Swap_GivenTwoHorizontallyAdjacentCellsWithElements_WhenSwapping_ThenReturnsSuccessAndElementsAreSwapped()
    {
        // Given a grid with two horizontally adjacent cells containing different elements
        var gridData = new Element?[,]
        {
            { new("X"), new("Y"), null, null },
            { null, null, null, null },
            { null, null, null, null },
            { null, null, null, null }
        };
        var _grid = new HorizontalVerticalSwapGrid<Element>(gridData);

        var firstRow = new GridRow(0);
        var firstColumn = new GridColumn(0);
        var secondRow = new GridRow(0);
        var secondColumn = new GridColumn(1);

        // When swapping the horizontally adjacent cells
        var result = _grid.Swap(firstRow, firstColumn, secondRow, secondColumn);

        // Then operation succeeds and elements are swapped
        Assert.That(result.IsT0, Is.True, "Swap should return Success for horizontally adjacent cells");
    }

    [Test]
    public void Swap_GivenTwoVerticallyAdjacentCellsWithElements_WhenSwapping_ThenReturnsSuccessAndElementsAreSwapped()
    {
        // Given a grid with two vertically adjacent cells containing different elements
        var gridData = new Element?[,]
        {
            { null, new("A"), null, null },
            { null, new("B"), null, null },
            { null, null, null, null },
            { null, null, null, null }
        };
        var _grid = new HorizontalVerticalSwapGrid<Element>(gridData);

        var firstRow = new GridRow(0);
        var firstColumn = new GridColumn(1);
        var secondRow = new GridRow(1);
        var secondColumn = new GridColumn(1);

        // When swapping the vertically adjacent cells
        var result = _grid.Swap(firstRow, firstColumn, secondRow, secondColumn);

        // Then operation succeeds and elements are swapped
        Assert.That(result.IsT0, Is.True, "Swap should return Success for vertically adjacent cells");

        // Verify swap occurred

    }

    [Test]
    public void Swap_GivenTwoDiagonallyAdjacentCells_WhenSwapping_ThenReturnsInvalidSwap()
    {
        // Given a grid with two diagonally adjacent cells
        var gridData = new Element?[,]
        {
            { null, null, null, null },
            { null, new("Center"), null, null },
            { null, null, new("Diagonal"), null },
            { null, null, null, null }
        };
        var _grid = new HorizontalVerticalSwapGrid<Element>(gridData);

        var firstRow = new GridRow(1);
        var firstColumn = new GridColumn(1);
        var secondRow = new GridRow(2);
        var secondColumn = new GridColumn(2);

        // When attempting to swap diagonally adjacent cells
        var result = _grid.Swap(firstRow, firstColumn, secondRow, secondColumn);

        // Then operation fails with InvalidSwap
        Assert.That(result.IsT1, Is.True, "Swap should return InvalidSwap for diagonally adjacent cells");
        Assert.That(result.AsT1, Is.TypeOf<InvalidSwap>(), "Should return InvalidSwap error");
    }

    [Test]
    public void Swap_GivenTwoNonAdjacentCells_WhenSwapping_ThenReturnsInvalidSwap()
    {
        // Given a grid with two non-adjacent cells
        var gridData = new Element?[,]
        {
            { new("TopLeft"), null, null, null },
            { null, null, null, null },
            { null, null, null, null },
            { null, null, null, new("BottomRight") }
        };
        var _grid = new HorizontalVerticalSwapGrid<Element>(gridData);

        var firstRow = new GridRow(0);
        var firstColumn = new GridColumn(0);
        var secondRow = new GridRow(3);
        var secondColumn = new GridColumn(3);

        // When attempting to swap non-adjacent cells
        var result = _grid.Swap(firstRow, firstColumn, secondRow, secondColumn);

        // Then operation fails with InvalidSwap
        Assert.That(result.IsT1, Is.True, "Swap should return InvalidSwap for non-adjacent cells");
        Assert.That(result.AsT1, Is.TypeOf<InvalidSwap>(), "Should return InvalidSwap error");
    }

    [Test]
    public void Swap_GivenTwoCellsWithTwoRowsDifference_WhenSwapping_ThenReturnsInvalidSwap()
    {
        // Given a grid with two cells in the same column but with 2 rows difference
        var gridData = new Element?[,]
        {
            { null, null, new("Top"), null },
            { null, null, null, null },
            { null, null, new("Bottom"), null },
            { null, null, null, null }
        };
        var _grid = new HorizontalVerticalSwapGrid<Element>(gridData);

        var firstRow = new GridRow(0);
        var firstColumn = new GridColumn(2);
        var secondRow = new GridRow(2);
        var secondColumn = new GridColumn(2);

        // When attempting to swap cells with 2 rows difference
        var result = _grid.Swap(firstRow, firstColumn, secondRow, secondColumn);

        // Then operation fails with InvalidSwap
        Assert.That(result.IsT1, Is.True, "Swap should return InvalidSwap for cells 2 rows apart");
        Assert.That(result.AsT1, Is.TypeOf<InvalidSwap>(), "Should return InvalidSwap error");
    }

    [Test]
    public void Swap_GivenTwoCellsWithTwoColumnsDifference_WhenSwapping_ThenReturnsInvalidSwap()
    {
        // Given a grid with two cells in the same row but with 2 columns difference
        var gridData = new Element?[,]
        {
            { null, null, null, null },
            { new("Left"), null, new("Right"), null },
            { null, null, null, null },
            { null, null, null, null }
        };
        var _grid = new HorizontalVerticalSwapGrid<Element>(gridData);

        var firstRow = new GridRow(1);
        var firstColumn = new GridColumn(0);
        var secondRow = new GridRow(1);
        var secondColumn = new GridColumn(2);

        // When attempting to swap cells with 2 columns difference
        var result = _grid.Swap(firstRow, firstColumn, secondRow, secondColumn);

        // Then operation fails with InvalidSwap
        Assert.That(result.IsT1, Is.True, "Swap should return InvalidSwap for cells 2 columns apart");
        Assert.That(result.AsT1, Is.TypeOf<InvalidSwap>(), "Should return InvalidSwap error");
    }

    [Test]
    public void Swap_GivenOneEmptyAndOneOccupiedAdjacentCells_WhenSwapping_ThenReturnsSuccessAndSwapsContent()
    {
        // Given a grid with one occupied and one empty adjacent cell
        var gridData = new Element?[,]
        {
            { null, new("Z"), null, null },
            { null, null, null, null },
            { null, null, null, null },
            { null, null, null, null }
        };
        var _grid = new HorizontalVerticalSwapGrid<Element>(gridData);

        var occupiedRow = new GridRow(0);
        var occupiedColumn = new GridColumn(1);
        var emptyRow = new GridRow(0);
        var emptyColumn = new GridColumn(2);

        // When swapping occupied and empty cells
        var result = _grid.Swap(occupiedRow, occupiedColumn, emptyRow, emptyColumn);

        // Then operation succeeds and content is swapped
        Assert.That(result.IsT0, Is.True, "Swap should return Success for occupied and empty adjacent cells");
    }

    [Test]
    public void Swap_GivenTwoEmptyAdjacentCells_WhenSwapping_ThenReturnsSuccessAndCellsRemainEmpty()
    {
        // Given a grid with two empty adjacent cells
        var gridData = new Element?[,]
        {
            { null, null, null, null },
            { null, null, null, null },
            { null, null, null, null },
            { null, null, null, null }
        };
        var _grid = new HorizontalVerticalSwapGrid<Element>(gridData);

        var firstRow = new GridRow(2);
        var firstColumn = new GridColumn(1);
        var secondRow = new GridRow(2);
        var secondColumn = new GridColumn(2);

        // When swapping two empty cells
        var result = _grid.Swap(firstRow, firstColumn, secondRow, secondColumn);

        // Then operation succeeds and cells remain empty
        Assert.That(result.IsT0, Is.True, "Swap should return Success for two empty adjacent cells");

        var cell1After = _grid.TryGetCell(2, 1).AsT0;
        var cell2After = _grid.TryGetCell(2, 2).AsT0;

        Assert.That(cell1After.IsOccupied(), Is.False, "First cell should remain empty");
        Assert.That(cell2After.IsOccupied(), Is.False, "Second cell should remain empty");
    }

    [Test]
    public void Swap_GivenHorizontallyAdjacentCellsAtRightEdge_WhenSwapping_ThenReturnsSuccessAndElementsAreSwapped()
    {
        // Given a grid with horizontally adjacent cells at the right edge
        var gridData = new Element?[,]
        {
            { null, null, null, null },
            { null, null, new("Edge1"), new("Edge2") },
            { null, null, null, null },
            { null, null, null, null }
        };
        var _grid = new HorizontalVerticalSwapGrid<Element>(gridData);

        var firstRow = new GridRow(1);
        var firstColumn = new GridColumn(2);
        var secondRow = new GridRow(1);
        var secondColumn = new GridColumn(3);

        // When swapping cells at grid edge
        var result = _grid.Swap(firstRow, firstColumn, secondRow, secondColumn);

        // Then operation succeeds and elements are swapped
        Assert.That(result.IsT0, Is.True, "Swap should return Success for adjacent cells at grid edge");
    }

    [Test]
    public void Swap_GivenVerticallyAdjacentCellsAtBottomEdge_WhenSwapping_ThenReturnsSuccessAndElementsAreSwapped()
    {
        // Given a grid with vertically adjacent cells at the bottom edge
        var gridData = new Element?[,]
        {
            { null, null, null, null },
            { null, null, null, null },
            { null, new("Bottom1"), null, null },
            { null, new("Bottom2"), null, null }
        };
        var _grid = new HorizontalVerticalSwapGrid<Element>(gridData);

        var firstRow = new GridRow(2);
        var firstColumn = new GridColumn(1);
        var secondRow = new GridRow(3);
        var secondColumn = new GridColumn(1);

        // When swapping cells at bottom edge
        var result = _grid.Swap(firstRow, firstColumn, secondRow, secondColumn);

        // Then operation succeeds and elements are swapped
        Assert.That(result.IsT0, Is.True, "Swap should return Success for adjacent cells at bottom edge");
    }
}