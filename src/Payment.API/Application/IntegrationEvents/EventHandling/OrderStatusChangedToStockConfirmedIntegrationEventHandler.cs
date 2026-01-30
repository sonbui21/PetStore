namespace Payment.API.Application.IntegrationEvents.EventHandling;

public class OrderStatusChangedToStockConfirmedIntegrationEventHandler(
    IPaymentRepository paymentRepository,
    IEventBus eventBus,
    ILogger<OrderStatusChangedToStockConfirmedIntegrationEventHandler> logger) :
    IIntegrationEventHandler<OrderStatusChangedToStockConfirmedIntegrationEvent>
{
    public async Task Handle(OrderStatusChangedToStockConfirmedIntegrationEvent @event)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

        IntegrationEvent orderPaymentIntegrationEvent;

        // Business feature comment:
        // When OrderStatusChangedToStockConfirmed Integration Event is handled.
        var paymentIntentService = new PaymentIntentService();
        var payment = await paymentRepository.GetByOrderIdAsync(@event.OrderId);
        if (payment is not null)
        {
            var paymentIntent = await paymentIntentService.GetAsync(payment.PaymentIntentId);
            if (paymentIntent.Status == "requires_capture")
            {
                var captured = await paymentIntentService.CaptureAsync(payment.PaymentIntentId);
                payment.Status = captured.Status;

                await paymentRepository.UnitOfWork.SaveEntitiesAsync();

                orderPaymentIntegrationEvent = new OrderPaymentSucceededIntegrationEvent(@event.OrderId);
            }
            else
            {
                orderPaymentIntegrationEvent = new OrderPaymentFailedIntegrationEvent(@event.OrderId);
            }
        }
        else
        {
            orderPaymentIntegrationEvent = new OrderPaymentFailedIntegrationEvent(@event.OrderId);
        }
        


        logger.LogInformation("Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", orderPaymentIntegrationEvent.Id, orderPaymentIntegrationEvent);

        await eventBus.PublishAsync(orderPaymentIntegrationEvent);
    }
}