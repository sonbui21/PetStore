namespace Ordering.API.Application.DomainEventHandlers;

public class OrderStatusChangedToAwaitingPaymentDomainEventHandler(
    IOrderRepository orderRepository,
    ILogger<OrderStatusChangedToAwaitingPaymentDomainEventHandler> logger,
    IBuyerRepository buyerRepository,
    IOrderingIntegrationEventService orderingIntegrationEventService)
        : INotificationHandler<OrderStatusChangedToAwaitingPaymentDomainEvent>
{
    private readonly IOrderRepository _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IBuyerRepository _buyerRepository = buyerRepository ?? throw new ArgumentNullException(nameof(buyerRepository));
    private readonly IOrderingIntegrationEventService _orderingIntegrationEventService = orderingIntegrationEventService ?? throw new ArgumentNullException(nameof(orderingIntegrationEventService));

    public async Task Handle(OrderStatusChangedToAwaitingPaymentDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        OrderingApiTrace.LogOrderStatusUpdated(_logger, domainEvent.OrderId, OrderStatus.AwaitingPayment);

        var order = await _orderRepository.GetAsync(domainEvent.OrderId);
        var buyer = await _buyerRepository.FindByIdAsync(order.BuyerId.Value);

        var integrationEvent = new OrderStatusChangedToAwaitingPaymentIntegrationEvent(
            order.Id,
            order.OrderStatus,
            buyer.Name,
            buyer.IdentityGuid);

        await _orderingIntegrationEventService.AddAndSaveEventAsync(integrationEvent);
    }
}
