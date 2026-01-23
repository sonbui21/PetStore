namespace Catalog.API.IntegrationEvents.Events;

public record OrderStatusChangedToPaymentConfirmedIntegrationEvent(Guid OrderId, IEnumerable<OrderStockItem> OrderStockItems) : IntegrationEvent;
