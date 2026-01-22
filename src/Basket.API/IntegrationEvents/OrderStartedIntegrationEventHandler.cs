namespace Basket.API.IntegrationEvents;

public class OrderStartedIntegrationEventHandler(
    IBasketRepository repository,
    ILogger<OrderStartedIntegrationEventHandler> logger) : IIntegrationEventHandler<OrderStartedIntegrationEvent>
{
    public async Task Handle(OrderStartedIntegrationEvent @event)
    {
        LogExtensions.LogHandlingIntegrationEvent(logger, @event.Id, @event);

        await repository.DeleteBasketAsync(@event.UserId);
    }
}
