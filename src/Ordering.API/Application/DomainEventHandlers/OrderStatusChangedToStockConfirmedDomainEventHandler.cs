namespace Ordering.API.Application.DomainEventHandlers;

public class OrderStatusChangedToStockConfirmedDomainEventHandler(
    ILogger<OrderStatusChangedToStockConfirmedDomainEventHandler> logger)
        : INotificationHandler<OrderStatusChangedToStockConfirmedDomainEvent>
{
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task Handle(OrderStatusChangedToStockConfirmedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        OrderingApiTrace.LogOrderStatusUpdated(_logger, domainEvent.OrderId, OrderStatus.StockConfirmed);
        await Task.CompletedTask;
    }
}
