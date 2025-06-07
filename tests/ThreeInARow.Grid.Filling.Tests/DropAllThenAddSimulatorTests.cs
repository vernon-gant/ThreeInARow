using NSubstitute;
using ThreeInARow.Grid.ADT;
using ThreeInARow.Grid.Filling.ADT;
using ThreeInARow.Grid.Filling.Implementations;
using ThreeInARow.Grid.Implementations;
using ThreeInARow.Grid.Matching.Tests;
using ThreeInARow.Grid.ValueObjects;

namespace ThreeInARow.Grid.Filling.Tests;

public class DropAllThenAddSimulatorTests : MGridTestUtility
{
    [Test]
    public void When_MatchRemovedFromMiddle_Then_ElementsFallAndNewOnesAddedOnTop()
    {
        // Given - Initial grid with a match removed from the middle
        var initialGrid = new[,]
        {
            { "B", "C", "D",  "A",  "A",  "C", "D", "A" },
            { "E", "F", "G",  "H",  "I",  "J", "K", "A" },
            { "L", "M", "N",  "O",  "P",  "Q", "R", "C" },
            { "A", "B", null, null, null, "F", "G", "H" },
            { "I", "J", "K",  "L",  "M",  "N", "O", "P" },
            { "Q", "R", "S",  "T",  "U",  "V", "W", "X" }
        };

        var grid = new HorizontalVerticalSwapGrid<string>(initialGrid);
        var generator = Substitute.For<IGenerator<string>>();
        SetupGenerator(generator, new Dictionary<int, string[]>
        {
            { 2, ["#"] },
            { 3, ["#"] },
            { 4, ["#"] }
        });

        var simulator = new DropAllThenAddSimulator<string, HorizontalVerticalSwapGrid<string>>(generator);
        simulator.Start(grid);

        // When - Execute the first step
        var step1 = simulator.ExecuteNextStep();

        // Then - The elements fall down and new ones are added on top
        Assert.That(step1.IsT0, Is.True);
        this.AssertGridMatches(grid, new[,]
        {
            { "B", "C", "#",  "#",  "#",  "C", "D", "A" },
            { "E", "F", "D",  "A",  "A",  "J", "K", "A" },
            { "L", "M", "G",  "H",  "I",  "Q", "R", "C" },
            { "A", "B", "N",  "O",  "P", "F", "G", "H" },
            { "I", "J", "K",  "L",  "M",  "N", "O", "P" },
            { "Q", "R", "S",  "T",  "U",  "V", "W", "X" }
        });
        Assert.That(simulator.HasMoreSteps.AsT0, Is.False,"There should be more steps after the first execution.");
    }

    private void SetupGenerator(IGenerator<string> generator, Dictionary<int, string[]> columnElements)
    {
        generator.Generate(Arg.Any<IReadableGrid<string>>());

        foreach (var kvp in columnElements)
        {
            var queue = new Queue<string>(kvp.Value);
            generator.ForColumn(new GridColumn(kvp.Key)).Returns(queue);
        }
    }
}