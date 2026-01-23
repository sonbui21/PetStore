namespace Ordering.API.Application.Sagas;

public interface IOrderSagaOrchestrator
{
    Task StartSagaAsync(Guid orderId);

    Task HandleGracePeriodConfirmedAsync(Guid orderId);

    Task HandleStockConfirmedAsync(Guid orderId);

    Task HandleStockRejectedAsync(Guid orderId, List<Guid> orderStockItems);

    Task HandlePaymentSucceededAsync(Guid orderId);

    Task HandlePaymentFailedAsync(Guid orderId);
}
