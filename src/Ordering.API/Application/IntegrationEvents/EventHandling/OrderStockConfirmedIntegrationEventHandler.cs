namespace Ordering.API.Application.IntegrationEvents.EventHandling;

public class OrderStockConfirmedIntegrationEventHandler(
    IOrderSagaOrchestrator orchestrator,
    ILogger<OrderStockConfirmedIntegrationEventHandler> logger) :
    IIntegrationEventHandler<OrderStockConfirmedIntegrationEvent>
{
    public async Task Handle(OrderStockConfirmedIntegrationEvent @event)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

        await orchestrator.HandleStockConfirmedAsync(@event.OrderId);
    }
}
