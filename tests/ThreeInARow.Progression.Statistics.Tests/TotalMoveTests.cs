using FluentAssertions;
using OneOf.Types;
using ThreeInARow.Infrastructure.ValueObjects;
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
        name.Value.Should().Be("Total Moves");
    }

    [Test]
    public void GivenTotalMoves_WhenGettingDescription_ThenReturnsCorrectDescription()
    {
        // Given
        // When
        var description = _totalMoves.Description;

        // Then
        description.ShouldBeOfTypeOneOf<NonEmptyString>();
        description.AsT0.Value.Should().Be("Only counts moves that could be placed on the board");
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
        value.ShouldBeOfTypeOneOf<NonEmptyString>();
        value.AsT0.Value.Should().Be("0");
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
        value.AsT0.Value.Should().Be("1");
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
        value.AsT0.Value.Should().Be("1");
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
        value.AsT0.Value.Should().Be("3");
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
        value.AsT0.Value.Should().Be("3");
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
        value.AsT0.Value.Should().Be("5", "should count all moves regardless of match production");
    }

    #endregion
}