using NSubstitute;
using ThreeInARow.Infrastructure.ADT;
using ThreeInARow.Infrastructure.Implementation;

namespace ThreeInARow.Infrastructure.Tests;

public class TestEvent : IEvent;

public class AnotherTestEvent : IEvent;

[TestFixture]
public class DefaultEventBusTests
{
    private DefaultEventBus _eventBus;

    [SetUp]
    public void Setup()
    {
        _eventBus = new DefaultEventBus();
    }

    [Test]
    public void GivenBusWithOneRegisteredHandler_WhenPublishingEventForThisHandler_ThenHandlerIsInvoked()
    {
        // Given
        var handler = Substitute.For<IEventHandler<TestEvent>>();
        _eventBus.Register(handler);

        // When
        var @event = new TestEvent();
        _eventBus.Publish(@event);

        // Then
        handler.Received(1).Handle(@event);
    }

    [Test]
    public void GivenBusWithOneRegisteredHandler_WhenPublishingEventForAnotherHandler_ThenHandlerIsNotInvoked()
    {
        // Given
        var handler = Substitute.For<IEventHandler<TestEvent>>();
        _eventBus.Register(handler);

        // When
        _eventBus.Publish(new AnotherTestEvent());

        // Then
        handler.DidNotReceive().Handle(Arg.Any<TestEvent>());
    }
}