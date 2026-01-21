namespace Ordering.API.Application.IntegrationEvents.Events;

public record OrderStockRejectedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; }

    public List<ConfirmedOrderStockItem> OrderStockItems { get; }

    public OrderStockRejectedIntegrationEvent(Guid orderId,
        List<ConfirmedOrderStockItem> orderStockItems)
    {
        OrderId = orderId;
        OrderStockItems = orderStockItems;
    }
}

public record ConfirmedOrderStockItem
{
    public Guid ProductId { get; }
    public bool HasStock { get; }

    public ConfirmedOrderStockItem(Guid productId, bool hasStock)
    {
        ProductId = productId;
        HasStock = hasStock;
    }
}