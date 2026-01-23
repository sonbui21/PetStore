namespace Ordering.API.Application.IntegrationEvents.Events;

public record OrderStatusChangedToPaymentConfirmedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; }
    public OrderStatus OrderStatus { get; }
    public string BuyerName { get; }
    public string BuyerIdentityGuid { get; }
    public IEnumerable<OrderStockItem> OrderStockItems { get; }

    public OrderStatusChangedToPaymentConfirmedIntegrationEvent(
        Guid orderId,
        OrderStatus orderStatus,
        string buyerName,
        string buyerIdentityGuid,
        IEnumerable<OrderStockItem> orderStockItems)
    {
        OrderId = orderId;
        OrderStatus = orderStatus;
        BuyerName = buyerName;
        BuyerIdentityGuid = buyerIdentityGuid;
        OrderStockItems = orderStockItems;
    }
}
