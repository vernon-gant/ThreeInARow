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
    public void GivenNotStartedStopwatch_WhenStateIsQueried_ThenIsRunningShouldBeFalseAndElapsedShouldBeIsNotRunning()
    {
        // Given
        // When
        // Then
        _systemStopwatch.IsRunning.Should().BeFalse();
        _systemStopwatch.Elapsed.ShouldBeOfTypeOneOf<IsNotRunning>();
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
}