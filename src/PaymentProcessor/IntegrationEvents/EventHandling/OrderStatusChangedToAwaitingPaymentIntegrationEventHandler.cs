namespace PaymentProcessor.IntegrationEvents.EventHandling;

public class OrderStatusChangedToAwaitingPaymentIntegrationEventHandler(
    IEventBus eventBus,
    IOptionsMonitor<PaymentOptions> options,
    ILogger<OrderStatusChangedToAwaitingPaymentIntegrationEventHandler> logger) :
    IIntegrationEventHandler<OrderStatusChangedToAwaitingPaymentIntegrationEvent>
{
    public async Task Handle(OrderStatusChangedToAwaitingPaymentIntegrationEvent @event)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

        IntegrationEvent orderPaymentIntegrationEvent;

        // Business feature comment:
        // When OrderStatusChangedToAwaitingPayment Integration Event is handled.
        // Here we're simulating authorizing the payment against a gateway (e.g., Stripe).
        // The payment can be successful or it can fail.
        if (options.CurrentValue.PaymentSucceeded)
        {
            orderPaymentIntegrationEvent = new OrderPaymentSucceededIntegrationEvent(@event.OrderId);
        }
        else
        {
            orderPaymentIntegrationEvent = new OrderPaymentFailedIntegrationEvent(@event.OrderId);
        }

        logger.LogInformation("Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", orderPaymentIntegrationEvent.Id, orderPaymentIntegrationEvent);

        await eventBus.PublishAsync(orderPaymentIntegrationEvent);
    }
}
