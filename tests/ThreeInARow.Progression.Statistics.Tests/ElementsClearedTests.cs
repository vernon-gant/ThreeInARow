using FluentAssertions;
using OneOf.Types;
using ThreeInARow.Progression.Events;
using ThreeInARow.Progression.Statistics.ADT;
using ThreeInARow.Progression.Statistics.Implementation.Statistics.General;
using ThreeInARow.TestingUtilities;
using ThreeInARow.ValueObjects;

namespace ThreeInARow.Progression.Statistics.Tests;

[TestFixture]
public class ElementsClearedStatisticTests
{
    private ElementsClearedStatistic<string> _elementsClearedStatistic;

    [SetUp]
    public void SetUp()
    {
        _elementsClearedStatistic = new ElementsClearedStatistic<string>();
    }

    #region Basic Property Tests

    [Test]
    public void GivenElementsClearedStatistic_WhenGettingName_ThenReturnsCorrectName()
    {
        // Given / When
        var name = _elementsClearedStatistic.Name;

        // Then
        name.Value.Should().Be("Elements Cleared");
    }

    [Test]
    public void GivenElementsClearedStatistic_WhenGettingDescription_ThenReturnsCorrectDescription()
    {
        // Given / When
        var desc = _elementsClearedStatistic.Description;

        // Then
        desc.ShouldBeOfTypeOneOf<NonEmptyString>();
        desc.AsT0.Value.Should().Be("Total number of elements cleared during the game");
    }

    [Test]
    public void GivenElementsClearedStatistic_WhenGettingUnit_ThenReturnsCorrectUnit()
    {
        // Given / When
        var unit = _elementsClearedStatistic.Unit;

        // Then
        unit.ShouldBeOfTypeOneOf<None>();
    }

    #endregion

    #region Value Property Tests

    [Test]
    public void GivenNewElementsClearedStatistic_WhenValueIsQueried_ThenReturnsNotEnoughData()
    {
        // Given / When
        var value = _elementsClearedStatistic.Value;

        // Then
        value.ShouldBeOfTypeOneOf<NotEnoughData>();
    }

    [Test]
    public void GivenElementsClearedEventHandled_WhenValueIsQueried_ThenReturnsCorrectCount()
    {
        // Given
        var elementsCleared = new ElementsCleared<string>
        {
            Elements = new List<string> { "A", "B", "C" }.ToNonEmptyList()
        };
        _elementsClearedStatistic.Handle(elementsCleared);

        // When
        var value = _elementsClearedStatistic.Value;

        // Then
        value.ShouldBeOfTypeOneOf<NonEmptyString>();
        value.AsT0.Value.Should().Be("3");
    }

    [Test]
    public void GivenMultipleElementsClearedEventsHandled_WhenValueIsQueried_ThenReturnsAccumulatedCount()
    {
        // Given
        var firstEvent = new ElementsCleared<string>
        {
            Elements = new List<string> { "A", "B" }.ToNonEmptyList()
        };
        var secondEvent = new ElementsCleared<string>
        {
            Elements = new List<string> { "C", "D", "E" }.ToNonEmptyList()
        };

        _elementsClearedStatistic.Handle(firstEvent);
        _elementsClearedStatistic.Handle(secondEvent);

        // When
        var value = _elementsClearedStatistic.Value;

        // Then
        value.ShouldBeOfTypeOneOf<NonEmptyString>();
        value.AsT0.Value.Should().Be("5");
    }

    #endregion
}