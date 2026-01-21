namespace Ordering.API.Application.IntegrationEvents.Events;

public record OrderStockConfirmedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; }

    public OrderStockConfirmedIntegrationEvent(Guid orderId) => OrderId = orderId;
}