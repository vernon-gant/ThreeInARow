using FluentAssertions;
using ThreeInARow.Grid.Matching.ADT;
using ThreeInARow.Grid.Matching.Implementations.Matches;
using ThreeInARow.Grid.ValueObjects;
using ThreeInARow.TestingUtilities;
using static ThreeInARow.TestingUtilities.CellExtensions;

namespace ThreeInARow.Grid.Matching.Tests;

[TestFixture]
public class DistinctCellsTests
{
    #region Create Method Tests

    [Test]
    public void GivenEmptyCollection_WhenCreating_ThenReturnsEmptyList()
    {
        // Given
        // When
        var result = DistinctCells<string>.Create([]);

        // Then
        result.ShouldBeOfTypeOneOf<EmptyList>("empty collection should return EmptyList");
    }

    [Test]
    public void GivenCollectionWithDuplicates_WhenCreating_ThenReturnsSameCellsPresent()
    {
        // Given
        var cells = new List<Cell<string>>
        {
            CreateCell("A", 0, 0),
            CreateCell("A", 0, 1),
            CreateCell("A", 0, 0) // Duplicate
        };

        // When
        var result = DistinctCells<string>.Create(cells);

        // Then
        result.ShouldBeOfTypeOneOf<SameCellsPresent>("collection with duplicates should return SameCellsPresent");
    }

    [Test]
    public void GivenCollectionWithUniqueElements_WhenCreating_ThenReturnsDistinctCells()
    {
        // Given
        var cells = new List<Cell<string>>
        {
            CreateCell("A", 0, 0),
            CreateCell("A", 0, 1),
            CreateCell("A", 0, 2)
        };

        // When
        var result = DistinctCells<string>.Create(cells);

        // Then
        result.ShouldBeOfTypeOneOf<DistinctCells<string>>("collection with unique elements should return DistinctCells");
        result.AsT0.Count.Should().Be(3);
    }

    [Test]
    public void GivenSingleElement_WhenCreating_ThenReturnsDistinctCellsWithOneElement()
    {
        // Given
        var cells = new List<Cell<string>> { CreateCell("A", 0, 0) };

        // When
        var result = DistinctCells<string>.Create(cells);

        // Then
        result.ShouldBeOfTypeOneOf<DistinctCells<string>>();
        result.AsT0.Count.Should().Be(1);
    }

    #endregion

    #region Count Property Tests

    [Test]
    public void GivenDistinctCellsWithNElements_WhenGettingCount_ThenReturnsN()
    {
        // Given
        var cells = new List<Cell<string>>
        {
            CreateCell("A", 0, 0),
            CreateCell("A", 1, 1),
            CreateCell("A", 2, 2)
        };
        var distinctCells = DistinctCells<string>.Create(cells).AsT0;

        // When
        var count = distinctCells.Count;

        // Then
        count.Should().Be(3);
    }

    #endregion

    #region Intersects Method Tests

    [Test]
    public void GivenTwoNonIntersectingSets_WhenCheckingIntersection_ThenReturnsFalse()
    {
        // Given
        var cells1 = DistinctCells<string>.Create([
            CreateCell("A", 0, 0),
            CreateCell("A", 0, 1)
        ]).AsT0;

        var cells2 = DistinctCells<string>.Create([
            CreateCell("A", 1, 0),
            CreateCell("A", 1, 1)
        ]).AsT0;

        // When
        var result = cells1.Intersects(cells2);

        // Then
        result.Should().BeFalse();
    }

    [Test]
    public void GivenTwoIntersectingSets_WhenCheckingIntersection_ThenReturnsTrue()
    {
        // Given
        var cells1 = DistinctCells<string>.Create([
            CreateCell("A", 0, 0),
            CreateCell("A", 0, 1)
        ]).AsT0;

        var cells2 = DistinctCells<string>.Create([
            CreateCell("A", 0, 1), // Shared cell
            CreateCell("A", 0, 2)
        ]).AsT0;

        // When
        var result = cells1.Intersects(cells2);

        // Then
        result.Should().BeTrue();
    }

    [Test]
    public void GivenIdenticalSets_WhenCheckingIntersection_ThenReturnsTrue()
    {
        // Given
        var cells1 = DistinctCells<string>.Create([
            CreateCell("A", 0, 0),
            CreateCell("A", 0, 1)
        ]).AsT0;

        var cells2 = DistinctCells<string>.Create([
            CreateCell("A", 0, 0),
            CreateCell("A", 0, 1)
        ]).AsT0;

        // When
        var result = cells1.Intersects(cells2);

        // Then
        result.Should().BeTrue();
    }

    #endregion

    #region Merge Method Tests

    [Test]
    public void GivenTwoNonIntersectingSets_WhenMerging_ThenReturnsDoesNotIntersect()
    {
        // Given
        var cells1 = DistinctCells<string>.Create([
            CreateCell("A", 0, 0),
            CreateCell("A", 0, 1)
        ]).AsT0;

        var cells2 = DistinctCells<string>.Create([
            CreateCell("A", 1, 0),
            CreateCell("A", 1, 1)
        ]).AsT0;

        // When
        var result = cells1.Merge(cells2);

        // Then
        result.ShouldBeOfTypeOneOf<DoesNotIntersect>();
    }

    [Test]
    public void GivenTwoIntersectingSets_WhenMerging_ThenReturnsMergedSet()
    {
        // Given
        var cells1 = DistinctCells<string>.Create([
            CreateCell("A", 0, 0),
            CreateCell("A", 0, 1)
        ]).AsT0;

        var cells2 = DistinctCells<string>.Create([
            CreateCell("A", 0, 1), // Shared
            CreateCell("A", 0, 2)
        ]).AsT0;

        // When
        var result = cells1.Merge(cells2);

        // Then
        result.ShouldBeOfTypeOneOf<DistinctCells<string>>();
        result.AsT0.Count.Should().Be(3, "merged set should contain all unique cells");
        result.AsT0.Should().BeEquivalentTo(new[]
        {
            CreateCell("A", 0, 0),
            CreateCell("A", 0, 1),
            CreateCell("A", 0, 2)
        });
    }

    [Test]
    public void GivenIdenticalSets_WhenMerging_ThenReturnsSameSet()
    {
        // Given
        var cells1 = DistinctCells<string>.Create([
            CreateCell("A", 0, 0),
            CreateCell("A", 0, 1)
        ]).AsT0;

        var cells2 = DistinctCells<string>.Create([
            CreateCell("A", 0, 0),
            CreateCell("A", 0, 1)
        ]).AsT0;

        // When
        var result = cells1.Merge(cells2);

        // Then
        result.ShouldBeOfTypeOneOf<DistinctCells<string>>();
        result.AsT0.Count.Should().Be(2);
        result.AsT0.Should().BeEquivalentTo(cells1);
    }

    [Test]
    public void GivenSubsetAndSuperset_WhenMerging_ThenReturnsSuperset()
    {
        // Given
        var subset = DistinctCells<string>.Create([
            CreateCell("A", 0, 0),
            CreateCell("A", 0, 1)
        ]).AsT0;

        var superset = DistinctCells<string>.Create([
            CreateCell("A", 0, 0),
            CreateCell("A", 0, 1),
            CreateCell("A", 0, 2),
            CreateCell("A", 0, 3)
        ]).AsT0;

        // When
        var result = subset.Merge(superset);

        // Then
        result.ShouldBeOfTypeOneOf<DistinctCells<string>>();
        result.AsT0.Count.Should().Be(4);
        result.AsT0.Should().BeEquivalentTo(superset);
    }

    #endregion

    #region IntersectWith Method Tests

    [Test]
    public void GivenTwoIntersectingSets_WhenIntersectingWith_ThenModifiesFirstSet()
    {
        // Given
        var cells1 = DistinctCells<string>.Create([
            CreateCell("A", 0, 0),
            CreateCell("A", 0, 1),
            CreateCell("A", 0, 2)
        ]).AsT0;

        var cells2 = DistinctCells<string>.Create([
            CreateCell("A", 0, 1),
            CreateCell("A", 0, 2),
            CreateCell("A", 0, 3)
        ]).AsT0;

        // When
        cells1.IntersectWith(cells2);

        // Then
        cells1.Count.Should().Be(2);
        cells1.Should().BeEquivalentTo(new[]
        {
            CreateCell("A", 0, 1),
            CreateCell("A", 0, 2)
        });
    }

    [Test]
    public void GivenTwoNonIntersectingSets_WhenIntersectingWith_ThenFirstSetBecomesEmpty()
    {
        // Given
        var cells1 = DistinctCells<string>.Create([
            CreateCell("A", 0, 0),
            CreateCell("A", 0, 1)
        ]).AsT0;

        var cells2 = DistinctCells<string>.Create([
            CreateCell("A", 1, 0),
            CreateCell("A", 1, 1)
        ]).AsT0;

        // When
        cells1.IntersectWith(cells2);

        // Then
        cells1.Count.Should().Be(0);
        cells1.Should().BeEmpty();
    }

    #endregion
}