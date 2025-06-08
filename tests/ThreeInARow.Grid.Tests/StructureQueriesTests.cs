using ThreeInARow.Grid.Implementations.Queries;
using ThreeInARow.TestingUtilities;

namespace ThreeInARow.Grid.Tests;

[TestFixture]
public class StructureQueriesTests : MGridTestUtility
{
    [Test]
    public void GivenAColumnOf8Cells_WhenQueryingConsecutiveSequencesWithLength3_ThenResultContains6Sequences()
    {
        // Given a column of 8 cells with elements A to H
        var gridData = new[,]
        {
            { "A" },
            { "B" },
            { "C" },
            { "D" },
            { "E" },
            { "F" },
            { "G" },
            { "H" }
        };
        var cellsEnumerable = this.CreateCellsFromGrid(gridData);

        // When querying for sequences of length 3
        const int sequenceLength = 3;
        var sequences = cellsEnumerable.ToNLengthSequences(sequenceLength).ToList();

        // Then the result should contain 6 sequences
        Assert.That(sequences.Count, Is.EqualTo(6), "Expected 6 sequences of length 3 from a column of 8 cells.");
        Assert.Multiple(() =>
        {
            Assert.That(sequences[0].Select(c => c.Element.AsT0), Is.EqualTo(new[] { "A", "B", "C" }), "First sequence should be A, B, C");
            Assert.That(sequences[1].Select(c => c.Element.AsT0), Is.EqualTo(new[] { "B", "C", "D" }), "Second sequence should be B, C, D");
            Assert.That(sequences[2].Select(c => c.Element.AsT0), Is.EqualTo(new[] { "C", "D", "E" }), "Third sequence should be C, D, E");
            Assert.That(sequences[3].Select(c => c.Element.AsT0), Is.EqualTo(new[] { "D", "E", "F" }), "Fourth sequence should be D, E, F");
            Assert.That(sequences[4].Select(c => c.Element.AsT0), Is.EqualTo(new[] { "E", "F", "G" }), "Fifth sequence should be E, F, G");
            Assert.That(sequences[5].Select(c => c.Element.AsT0), Is.EqualTo(new[] { "F", "G", "H" }), "Sixth sequence should be F, G, H");
        });
    }

    [Test]
    public void GivenAColumnOf8Cells_WhenQueryingConsecutiveSequencesWithLength4_ThenResultContains5Sequences()
    {
        // Given a column of 8 cells with elements A to H
        var gridData = new[,]
        {
            { "A" },
            { "B" },
            { "C" },
            { "D" },
            { "E" },
            { "F" },
            { "G" },
            { "H" }
        };
        var cellsEnumerable = this.CreateCellsFromGrid(gridData);

        // When querying for sequences of length 4
        const int sequenceLength = 4;
        var sequences = cellsEnumerable.ToNLengthSequences(sequenceLength).ToList();

        // Then the result should contain 5 sequences
        Assert.That(sequences.Count, Is.EqualTo(5), "Expected 5 sequences of length 4 from a column of 8 cells.");
        Assert.Multiple(() =>
        {
            Assert.That(sequences[0].Select(c => c.Element.AsT0), Is.EqualTo(new[] { "A", "B", "C", "D" }), "First sequence should be A, B, C, D");
            Assert.That(sequences[1].Select(c => c.Element.AsT0), Is.EqualTo(new[] { "B", "C", "D", "E" }), "Second sequence should be B, C, D, E");
            Assert.That(sequences[2].Select(c => c.Element.AsT0), Is.EqualTo(new[] { "C", "D", "E", "F" }), "Third sequence should be C, D, E, F");
            Assert.That(sequences[3].Select(c => c.Element.AsT0), Is.EqualTo(new[] { "D", "E", "F", "G" }), "Fourth sequence should be D, E, F, G");
            Assert.That(sequences[4].Select(c => c.Element.AsT0), Is.EqualTo(new[] { "E", "F", "G", "H" }), "Fifth sequence should be E, F, G, H");
        });
    }

    [Test]
    public void GivenARowOf8Cells_WhenQueryingConsecutiveSequencesWithLength3_ThenResultContains6Sequences()
    {
        // Given a row of 8 cells with elements A to H
        var gridData = new[,]
        {
            { "A", "B", "C", "D", "E", "F", "G", "H" }
        };
        var cellsEnumerable = this.CreateCellsFromGrid(gridData);

        // When querying for sequences of length 3
        const int sequenceLength = 3;
        var sequences = cellsEnumerable.ToNLengthSequences(sequenceLength).ToList();

        // Then the result should contain 6 sequences
        Assert.That(sequences.Count, Is.EqualTo(6), "Expected 6 sequences of length 3 from a row of 8 cells.");
        Assert.Multiple(() =>
        {
            Assert.That(sequences[0].Select(c => c.Element.AsT0), Is.EqualTo(new[] { "A", "B", "C" }), "First sequence should be A, B, C");
            Assert.That(sequences[1].Select(c => c.Element.AsT0), Is.EqualTo(new[] { "B", "C", "D" }), "Second sequence should be B, C, D");
            Assert.That(sequences[2].Select(c => c.Element.AsT0), Is.EqualTo(new[] { "C", "D", "E" }), "Third sequence should be C, D, E");
            Assert.That(sequences[3].Select(c => c.Element.AsT0), Is.EqualTo(new[] { "D", "E", "F" }), "Fourth sequence should be D, E, F");
            Assert.That(sequences[4].Select(c => c.Element.AsT0), Is.EqualTo(new[] { "E", "F", "G" }), "Fifth sequence should be E, F, G");
            Assert.That(sequences[5].Select(c => c.Element.AsT0), Is.EqualTo(new[] { "F", "G", "H" }), "Sixth sequence should be F, G, H");
        });
    }

    [Test]
    public void GivenARowOf8Cells_WhenQueryingConsecutiveSequencesWithLength4_ThenResultContains5Sequences()
    {
        // Given a row of 8 cells with elements A to H
        var gridData = new[,]
        {
            { "A", "B", "C", "D", "E", "F", "G", "H" }
        };
        var cellsEnumerable = this.CreateCellsFromGrid(gridData);

        // When querying for sequences of length 4
        const int sequenceLength = 4;
        var sequences = cellsEnumerable.ToNLengthSequences(sequenceLength).ToList();

        // Then the result should contain 5 sequences
        Assert.That(sequences.Count, Is.EqualTo(5), "Expected 5 sequences of length 4 from a row of 8 cells.");
        Assert.Multiple(() =>
        {
            Assert.That(sequences[0].Select(c => c.Element.AsT0), Is.EqualTo(new[] { "A", "B", "C", "D" }), "First sequence should be A, B, C, D");
            Assert.That(sequences[1].Select(c => c.Element.AsT0), Is.EqualTo(new[] { "B", "C", "D", "E" }), "Second sequence should be B, C, D, E");
            Assert.That(sequences[2].Select(c => c.Element.AsT0), Is.EqualTo(new[] { "C", "D", "E", "F" }), "Third sequence should be C, D, E, F");
            Assert.That(sequences[3].Select(c => c.Element.AsT0), Is.EqualTo(new[] { "D", "E", "F", "G" }), "Fourth sequence should be D, E, F, G");
            Assert.That(sequences[4].Select(c => c.Element.AsT0), Is.EqualTo(new[] { "E", "F", "G", "H" }), "Fifth sequence should be E, F, G, H");
        });
    }
}