using FluentAssertions;
using ThreeInARow.Infrastructure.ADT;
using ThreeInARow.Infrastructure.Implementation;
using ThreeInARow.TestingUtilities;

namespace ThreeInARow.Infrastructure.Tests;

[TestFixture]
public class SystemStopwatchTests
{
    private SystemStopwatch _systemStopwatch;

    [SetUp]
    public void Setup()
    {
        _systemStopwatch = new SystemStopwatch();
    }

    [Test]
    public void GivenNotStartedStopwatch_WhenStateIsQueried_ThenIsRunningShouldBeFalseAndElapsedShouldBeHasNotStartedYet()
    {
        // Given
        // When
        // Then
        _systemStopwatch.IsRunning.Should().BeFalse();
        _systemStopwatch.Elapsed.ShouldBeOfTypeOneOf<HasNotStartedYet>();
    }

    [Test]
    public void GivenStartedStopwatch_WhenStateIsQueried_ThenIsRunningShouldBeTrueAndElapsedShouldBeTimeSpan()
    {
        // Given
        _systemStopwatch.Start();

        // When
        // Then
        _systemStopwatch.IsRunning.Should().BeTrue();
        _systemStopwatch.Elapsed.ShouldBeOfTypeOneOf<TimeSpan>();
    }

    [Test]
    public void GivenStartedStopwatch_WhenStartIsCalledAgain_ThenShouldReturnIsRunningAndNotStartAgain()
    {
        // Given
        _systemStopwatch.Start();
        Thread.Sleep(250);
        var initialElapsed = _systemStopwatch.Elapsed.AsT0;

        // When
        var result = _systemStopwatch.Start();

        // Then
        result.ShouldBeOfTypeOneOf<AlreadyRunning>();
        _systemStopwatch.IsRunning.Should().BeTrue();
        _systemStopwatch.Elapsed.AsT0.Should().BeGreaterThan(initialElapsed);
    }
}