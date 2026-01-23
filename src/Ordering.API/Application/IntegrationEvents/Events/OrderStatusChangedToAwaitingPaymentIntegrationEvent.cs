namespace Ordering.API.Application.IntegrationEvents.Events;

public record OrderStatusChangedToAwaitingPaymentIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; }
    public OrderStatus OrderStatus { get; }
    public string BuyerName { get; }
    public string BuyerIdentityGuid { get; }

    public OrderStatusChangedToAwaitingPaymentIntegrationEvent(
        Guid orderId,
        OrderStatus orderStatus,
        string buyerName,
        string buyerIdentityGuid)
    {
        OrderId = orderId;
        OrderStatus = orderStatus;
        BuyerName = buyerName;
        BuyerIdentityGuid = buyerIdentityGuid;
    }
}
