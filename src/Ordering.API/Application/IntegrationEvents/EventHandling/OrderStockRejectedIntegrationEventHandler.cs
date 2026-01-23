namespace Ordering.API.Application.IntegrationEvents.EventHandling;

public class OrderStockRejectedIntegrationEventHandler(
    IOrderSagaOrchestrator orchestrator,
    ILogger<OrderStockRejectedIntegrationEventHandler> logger) : IIntegrationEventHandler<OrderStockRejectedIntegrationEvent>
{
    public async Task Handle(OrderStockRejectedIntegrationEvent @event)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

        var orderStockRejectedItems = @event.OrderStockItems
            .FindAll(c => !c.HasStock)
            .Select(c => c.ProductId)
            .ToList();

        await orchestrator.HandleStockRejectedAsync(@event.OrderId, orderStockRejectedItems);
    }
}
