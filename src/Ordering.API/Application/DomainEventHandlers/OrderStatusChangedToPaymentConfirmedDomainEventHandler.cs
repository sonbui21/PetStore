namespace Ordering.API.Application.DomainEventHandlers;

public class OrderStatusChangedToPaymentConfirmedDomainEventHandler(
    IOrderRepository orderRepository,
    ILogger<OrderStatusChangedToPaymentConfirmedDomainEventHandler> logger,
    IBuyerRepository buyerRepository,
    IOrderingIntegrationEventService orderingIntegrationEventService)
        : INotificationHandler<OrderStatusChangedToPaymentConfirmedDomainEvent>
{
    private readonly IOrderRepository _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IBuyerRepository _buyerRepository = buyerRepository ?? throw new ArgumentNullException(nameof(buyerRepository));
    private readonly IOrderingIntegrationEventService _orderingIntegrationEventService = orderingIntegrationEventService ?? throw new ArgumentNullException(nameof(orderingIntegrationEventService));

    public async Task Handle(OrderStatusChangedToPaymentConfirmedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        OrderingApiTrace.LogOrderStatusUpdated(_logger, domainEvent.OrderId, OrderStatus.PaymentConfirmed);

        var order = await _orderRepository.GetAsync(domainEvent.OrderId);
        var buyer = await _buyerRepository.FindByIdAsync(order.BuyerId.Value);

        var orderStockList = domainEvent.OrderItems
            .Select(orderItem => new OrderStockItem(orderItem.ProductId, orderItem.VariantId, orderItem.Quantity));

        var integrationEvent = new OrderStatusChangedToPaymentConfirmedIntegrationEvent(
            order.Id,
            order.OrderStatus,
            buyer.Name,
            buyer.IdentityGuid,
            orderStockList);

        await _orderingIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
    }
}
