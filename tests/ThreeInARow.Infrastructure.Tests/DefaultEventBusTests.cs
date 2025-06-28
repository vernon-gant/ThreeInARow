using NSubstitute;
using ThreeInARow.Infrastructure.ADT;
using ThreeInARow.Infrastructure.Implementation;

namespace ThreeInARow.Infrastructure.Tests;

public class TestEventA : IEvent;

public class TestEventB : IEvent;

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
    public void GivenBusWithHandlerForEventA_WhenPublishingEventA_ThenHandlerIsInvoked()
    {
        // Given
        var handler = Substitute.For<IEventHandler<TestEventA>>();
        _eventBus.Register(handler);

        // When
        var @event = new TestEventA();
        _eventBus.Publish(@event);

        // Then
        handler.Received(1).Handle(@event);
    }

    [Test]
    public void GivenBusWithHandlerForEventA_WhenPublishingEventB_ThenHandlerIsNotInvoked()
    {
        // Given
        var handler = Substitute.For<IEventHandler<TestEventA>>();
        _eventBus.Register(handler);

        // When
        _eventBus.Publish(new TestEventB());

        // Then
        handler.DidNotReceive().Handle(Arg.Any<TestEventA>());
    }

    [Test]
    public void GivenBusWithMultipleHandlersForEventA_WhenPublishingEventA_ThenAllHandlersAreInvoked()
    {
        // Given
        var handler1 = Substitute.For<IEventHandler<TestEventA>>();
        var handler2 = Substitute.For<IEventHandler<TestEventA>>();
        _eventBus.Register(handler1);
        _eventBus.Register(handler2);

        // When
        var @event = new TestEventA();
        _eventBus.Publish(@event);

        // Then
        handler1.Received(1).Handle(@event);
        handler2.Received(1).Handle(@event);
    }

    [Test]
    public void GivenBusWithNoHandlers_WhenPublishingEventA_ThenNoExceptionIsThrown()
    {
        // Given - No handlers registered

        // When & Then
        Assert.DoesNotThrow(() => _eventBus.Publish(new TestEventA()));
    }

    [Test]
    public void GivenBusWithHandlersForDifferentEvents_WhenPublishingEventB_ThenOnlyEventBHandlerIsInvoked()
    {
        // Given
        var handlerA = Substitute.For<IEventHandler<TestEventA>>();
        var handlerB = Substitute.For<IEventHandler<TestEventB>>();
        _eventBus.Register(handlerA);
        _eventBus.Register(handlerB);

        // When
        var @event = new TestEventB();
        _eventBus.Publish(@event);

        // Then
        handlerA.DidNotReceive().Handle(Arg.Any<TestEventA>());
        handlerB.Received(1).Handle(@event);
    }

    [Test]
    public void GivenBusWithSameHandlerRegisteredTwice_WhenPublishingEventA_ThenHandlerIsInvokedOnce()
    {
        // Given
        var handler = Substitute.For<IEventHandler<TestEventA>>();
        _eventBus.Register(handler);
        _eventBus.Register(handler);

        // When
        var @event = new TestEventA();
        _eventBus.Publish(@event);

        // Then
        handler.Received(1).Handle(@event);
    }

    [Test]
    public void GivenBusWithHandlerThatThrows_WhenPublishingEvent_ThenExceptionPropagates()
    {
        // Given
        var handler = Substitute.For<IEventHandler<TestEventA>>();
        handler.When(h => h.Handle(Arg.Any<TestEventA>())).Do(_ => throw new InvalidOperationException("Handler error"));
        _eventBus.Register(handler);

        // When & Then
        Assert.Throws<InvalidOperationException>(() => _eventBus.Publish(new TestEventA()));
    }
}