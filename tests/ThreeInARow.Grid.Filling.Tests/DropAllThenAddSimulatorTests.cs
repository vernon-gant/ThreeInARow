using NSubstitute;
using ThreeInARow.Grid.ADT;
using ThreeInARow.Grid.Filling.ADT;
using ThreeInARow.Grid.Filling.Implementations;
using ThreeInARow.Grid.Implementations;
using ThreeInARow.TestingUtilities;

namespace ThreeInARow.Grid.Filling.Tests;

public class DropAllThenAddSimulatorTests : MGridTestUtility
{
    [Test]
    public void GivenEmptySpacesAfterHorizontalMatchRemoval_WhenPlayerWaitsForGridRefill_ThenElementsDropAndNewOnesAppearAtTop()
    {
        // Given a grid where a horizontal match has been removed from the middle, leaving empty spaces
        var initialGrid = new[,]
        {
            { ".", ".", ".",  ".",  ".",  ".", ".", "." },
            { ".", ".", ".",  ".",  ".",  ".", ".", "." },
            { ".", ".", ".",  ".",  ".",  ".", ".", "." },
            { ".", ".", null, null, null, ".", ".", "." }, // Empty spaces from removed horizontal match
            { ".", ".", ".",  ".",  ".",  ".", ".", "." },
            { ".", ".", ".",  ".",  ".",  ".", ".", "." }
        };

        var grid = new HorizontalVerticalSwapGrid<string>(initialGrid);
        var generator = Substitute.For<IGenerator<string>>();
        SetupGenerator(generator, new Dictionary<int, string[]>
        {
            { 2, ["@"] },
            { 3, ["@"] },
            { 4, ["@"] }
        });

        var simulator = new DropAllThenAddSimulator<string, HorizontalVerticalSwapGrid<string>>(generator);
        simulator.Start(grid);

        // When the player waits for the grid to refill automatically
        var refillResult = simulator.ExecuteNextStep();

        // Then existing elements drop down to fill gaps and new elements appear at the top
        Assert.That(refillResult.IsT0, Is.True, "Grid refill should complete successfully");

        // Verify the grid is properly refilled
        this.AssertGridMatches(grid, new[,]
        {
            { ".", ".", "@",  "@",  "@",  ".", ".", "." }, // New elements at top
            { ".", ".", ".",  ".",  ".",  ".", ".", "." },
            { ".", ".", ".",  ".",  ".",  ".", ".", "." },
            { ".", ".", ".",  ".",  ".",  ".", ".", "." },
            { ".", ".", ".",  ".",  ".",  ".", ".", "." },
            { ".", ".", ".",  ".",  ".",  ".", ".", "." }
        });

        Assert.That(simulator.HasMoreSteps.AsT0, Is.False, "Grid refill should be complete after horizontal match removal");
    }

    [Test]
    public void GivenEmptySpacesAfterVerticalMatchRemoval_WhenPlayerWaitsForGridRefill_ThenElementsDropGraduallyAndNewOnesAppearAtTop()
    {
        // Given a grid where a vertical match has been removed from the middle, leaving empty spaces in a column
        var initialGrid = new[,]
        {
            { ".", ".", ".",  ".", ".", ".", ".", "." },
            { ".", ".", null, ".", ".", ".", ".", "." }, // Empty spaces from removed vertical match
            { ".", ".", null, ".", ".", ".", ".", "." },
            { ".", ".", null, ".", ".", ".", ".", "." },
            { ".", ".", ".",  ".", ".", ".", ".", "." },
            { ".", ".", ".",  ".", ".", ".", ".", "." }
        };

        var grid = new HorizontalVerticalSwapGrid<string>(initialGrid);
        var generator = Substitute.For<IGenerator<string>>();
        SetupGenerator(generator, new Dictionary<int, string[]>
        {
            { 2, ["@", "@", "@"] }, // Three new elements for the affected column
        });

        var simulator = new DropAllThenAddSimulator<string, HorizontalVerticalSwapGrid<string>>(generator);
        simulator.Start(grid);

        // When the player waits for the first refill step
        var firstStep = simulator.ExecuteNextStep();

        // Then the first new element appears at the top
        Assert.That(firstStep.IsT0, Is.True, "First refill step should complete successfully");

        this.AssertGridMatches(grid, new[,]
        {
            { ".", ".", "@",  ".", ".", ".", ".", "." }, // First new element appears
            { ".", ".", null, ".", ".", ".", ".", "." }, // Still empty spaces below
            { ".", ".", null, ".", ".", ".", ".", "." },
            { ".", ".", ".", ".", ".", ".", ".", "." },
            { ".", ".", ".",  ".", ".", ".", ".", "." },
            { ".", ".", ".",  ".", ".", ".", ".", "." }
        });

        Assert.That(simulator.HasMoreSteps.AsT0, Is.True, "More refill steps should be needed after vertical match removal");

        // When the player waits for the second refill step
        var secondStep = simulator.ExecuteNextStep();

        // Then the second new element appears and elements continue dropping
        Assert.That(secondStep.IsT0, Is.True, "Second refill step should complete successfully");

        this.AssertGridMatches(grid, new[,]
        {
            { ".", ".", "@",  ".", ".", ".", ".", "." }, // Previous element remains
            { ".", ".", null,  ".", ".", ".", ".", "." }, // Still one empty space
            { ".", ".", "@",  ".", ".", ".", ".", "." }, // Second new element drops into position
            { ".", ".", ".",  ".", ".", ".", ".", "." },
            { ".", ".", ".",  ".", ".", ".", ".", "." },
            { ".", ".", ".",  ".", ".", ".", ".", "." }
        });

        Assert.That(simulator.HasMoreSteps.AsT0, Is.True, "One more refill step should be needed");

        // When the player waits for the final refill step
        var finalStep = simulator.ExecuteNextStep();

        // Then the column is completely filled and refill process is complete
        Assert.That(finalStep.IsT0, Is.True, "Final refill step should complete successfully");

        this.AssertGridMatches(grid, new[,]
        {
            { ".", ".", "@",  ".", ".", ".", ".", "." }, // All three new elements in place
            { ".", ".", "@",  ".", ".", ".", ".", "." },
            { ".", ".", "@",  ".", ".", ".", ".", "." },
            { ".", ".", ".",  ".", ".", ".", ".", "." },
            { ".", ".", ".",  ".", ".", ".", ".", "." },
            { ".", ".", ".",  ".", ".", ".", ".", "." }
        });

        Assert.That(simulator.HasMoreSteps.AsT0, Is.False, "Grid refill should be complete after all elements have dropped");
    }

    #region Helper Methods

    private static void SetupGenerator(IGenerator<string> generator, Dictionary<int, string[]> columnElements)
    {
        generator.Generate(Arg.Any<IReadableGrid<string>>());

        foreach (var kvp in columnElements)
        {
            var queue = new Queue<string>(kvp.Value);
            generator.ForColumn(kvp.Key).Returns(queue);
        }
    }

    #endregion
}