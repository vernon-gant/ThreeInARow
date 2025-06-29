using FluentAssertions;
using OneOf.Types;
using ThreeInARow.Progression.Events.Events;
using ThreeInARow.Progression.Statistics.Implementation.Statistics.General;
using ThreeInARow.TestingUtilities;

namespace ThreeInARow.Progression.Statistics.Tests;

[TestFixture]
public class TotalMovesTests
{
    private TotalMoves _totalMoves;

    [SetUp]
    public void SetUp()
    {
        _totalMoves = new TotalMoves();
    }

    #region Basic Property Tests

    [Test]
    public void GivenTotalMoves_WhenGettingName_ThenReturnsCorrectName()
    {
        // Given
        // When
        var name = _totalMoves.Name;

        // Then
        name.Should().Be("Total Moves");
    }

    [Test]
    public void GivenTotalMoves_WhenGettingDescription_ThenReturnsCorrectDescription()
    {
        // Given
        // When
        var description = _totalMoves.Description;

        // Then
        description.ShouldBeOfTypeOneOf<string>();
        description.AsT0.Should().Be("Only counts moves that could be placed on the board");
    }

    [Test]
    public void GivenTotalMoves_WhenGettingUnit_ThenReturnsNone()
    {
        // Given
        // When
        var unit = _totalMoves.Unit;

        // Then
        unit.ShouldBeOfTypeOneOf<None>();
    }

    #endregion

    #region Value Property Tests

    [Test]
    public void GivenNoMovesHandled_WhenGettingValue_ThenReturnsZero()
    {
        // Given
        // When
        var value = _totalMoves.Value;

        // Then
        value.ShouldBeOfTypeOneOf<string>();
        value.AsT0.Should().Be("0");
    }

    [Test]
    public void GivenOneMoveMadeWithoutMatch_WhenGettingValue_ThenReturnsOne()
    {
        // Given
        var moveEvent = new MoveMade
        {
            ProducedMatch = false
        };
        _totalMoves.Handle(moveEvent);

        // When
        var value = _totalMoves.Value;

        // Then
        value.IsT0.Should().BeTrue();
        value.AsT0.Should().Be("1");
    }

    [Test]
    public void GivenOneMoveMadeWithMatch_WhenGettingValue_ThenReturnsOne()
    {
        // Given
        var moveEvent = new MoveMade
        {
            ProducedMatch = true
        };
        _totalMoves.Handle(moveEvent);

        // When
        var value = _totalMoves.Value;

        // Then
        value.IsT0.Should().BeTrue();
        value.AsT0.Should().Be("1");
    }

    [Test]
    public void GivenMultipleMovesMadeWithoutMatches_WhenGettingValue_ThenReturnsCorrectCount()
    {
        // Given
        _totalMoves.Handle(new MoveMade { ProducedMatch = false });
        _totalMoves.Handle(new MoveMade { ProducedMatch = false });
        _totalMoves.Handle(new MoveMade { ProducedMatch = false });

        // When
        var value = _totalMoves.Value;

        // Then
        value.IsT0.Should().BeTrue();
        value.AsT0.Should().Be("3");
    }

    [Test]
    public void GivenMultipleMovesMadeWithMatches_WhenGettingValue_ThenReturnsCorrectCount()
    {
        // Given
        _totalMoves.Handle(new MoveMade { ProducedMatch = true });
        _totalMoves.Handle(new MoveMade { ProducedMatch = true });
        _totalMoves.Handle(new MoveMade { ProducedMatch = true });

        // When
        var value = _totalMoves.Value;

        // Then
        value.IsT0.Should().BeTrue();
        value.AsT0.Should().Be("3");
    }

    [Test]
    public void GivenMixedMovesMadeWithAndWithoutMatches_WhenGettingValue_ThenReturnsCorrectTotalCount()
    {
        // Given
        _totalMoves.Handle(new MoveMade { ProducedMatch = true });
        _totalMoves.Handle(new MoveMade { ProducedMatch = false });
        _totalMoves.Handle(new MoveMade { ProducedMatch = true });
        _totalMoves.Handle(new MoveMade { ProducedMatch = false });
        _totalMoves.Handle(new MoveMade { ProducedMatch = false });

        // When
        var value = _totalMoves.Value;

        // Then
        value.IsT0.Should().BeTrue();
        value.AsT0.Should().Be("5", "should count all moves regardless of match production");
    }

    #endregion

    #region Event Handling Tests

    [Test]
    public void GivenMoveMadeEventWithMatch_WhenHandling_ThenIncrementsCounter()
    {
        // Given
        var initialValue = int.Parse(_totalMoves.Value.AsT0);
        var moveEvent = new MoveMade
        {
            ProducedMatch = true
        };

        // When
        _totalMoves.Handle(moveEvent);

        // Then
        var newValue = int.Parse(_totalMoves.Value.AsT0);
        newValue.Should().Be(initialValue + 1);
    }

    [Test]
    public void GivenMoveMadeEventWithoutMatch_WhenHandling_ThenIncrementsCounter()
    {
        // Given
        var initialValue = int.Parse(_totalMoves.Value.AsT0);
        var moveEvent = new MoveMade
        {
            ProducedMatch = false
        };

        // When
        _totalMoves.Handle(moveEvent);

        // Then
        var newValue = int.Parse(_totalMoves.Value.AsT0);
        newValue.Should().Be(initialValue + 1);
    }

    [Test]
    public void GivenSequenceOfMoves_WhenHandling_ThenCountsAllMovesRegardlessOfMatchResult()
    {
        // Given
        var moves = new[]
        {
            new MoveMade { ProducedMatch = true },
            new MoveMade { ProducedMatch = true },
            new MoveMade { ProducedMatch = false },
            new MoveMade { ProducedMatch = true },
            new MoveMade { ProducedMatch = false },
            new MoveMade { ProducedMatch = false },
            new MoveMade { ProducedMatch = true }
        };

        // When
        foreach (var move in moves)
        {
            _totalMoves.Handle(move);
        }

        // Then
        _totalMoves.Value.AsT0.Should().Be("7", "should count all moves in the sequence");
    }

    [Test]
    public void GivenLargeNumberOfMoves_WhenHandling_ThenMaintainsAccurateCount()
    {
        // Given
        const int totalMoves = 1000;
        const int matchingMoves = 300;

        // When
        for (int i = 0; i < matchingMoves; i++)
        {
            _totalMoves.Handle(new MoveMade { ProducedMatch = true });
        }

        for (int i = 0; i < (totalMoves - matchingMoves); i++)
        {
            _totalMoves.Handle(new MoveMade { ProducedMatch = false });
        }

        // Then
        _totalMoves.Value.AsT0.Should().Be(totalMoves.ToString());
    }

    #endregion
}