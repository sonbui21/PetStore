namespace Catalog.API.IntegrationEvents.EventHandling;

public class OrderStatusChangedToPaidIntegrationEventHandler(
    CatalogContext catalogContext,
    ILogger<OrderStatusChangedToPaidIntegrationEventHandler> logger) :
    IIntegrationEventHandler<OrderStatusChangedToPaidIntegrationEvent>
{
    public async Task Handle(OrderStatusChangedToPaidIntegrationEvent @event)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

        foreach (var orderStockItem in @event.OrderStockItems)
        {
            var variant = await catalogContext.ItemVariants
                .SingleOrDefaultAsync(v => v.Id == orderStockItem.VariantId && v.CatalogItemId == orderStockItem.ProductId);

            variant?.RemoveStock(orderStockItem.Quantity);
        }

        await catalogContext.SaveChangesAsync();
    }
}
