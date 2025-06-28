namespace ThreeInARow.Infrastructure.ADT;

public interface IEventHandler<in TNotification> where TNotification : IEvent
{
    void Handle(TNotification notification);
}