namespace OrderProcessor.Events;

public record GracePeriodConfirmedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; }

    public GracePeriodConfirmedIntegrationEvent(Guid orderId) =>
        OrderId = orderId;
}
