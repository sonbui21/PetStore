namespace Ordering.API.Application.IntegrationEvents.Events;

public record GracePeriodConfirmedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; }

    public GracePeriodConfirmedIntegrationEvent(Guid orderId) =>
        OrderId = orderId;
}
