namespace Ordering.Domain.Sagas;

public class OrderSagaState : IAggregateRoot
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public OrderSagaStep CurrentStep { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private OrderSagaState() { }

    private OrderSagaState(Guid orderId, OrderSagaStep step)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        CurrentStep = step;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static OrderSagaState Start(Guid orderId)
    {
        return new OrderSagaState(orderId, OrderSagaStep.Started);
    }

    public void MarkAwaitingPayment()
    {
        TransitionTo(OrderSagaStep.AwaitingPayment);
    }

    public void MarkStockConfirmed()
    {
        TransitionTo(OrderSagaStep.StockConfirmed);
    }

    public void MarkStockRejected()
    {
        TransitionTo(OrderSagaStep.StockRejected);
    }

    public void MarkPaymentConfirmed()
    {
        TransitionTo(OrderSagaStep.PaymentConfirmed);
    }

    public void MarkPaymentFailed()
    {
        TransitionTo(OrderSagaStep.PaymentFailed);
    }

    public void MarkCancelled()
    {
        TransitionTo(OrderSagaStep.Cancelled);
    }

    public void MarkCompleted()
    {
        TransitionTo(OrderSagaStep.Completed);
    }

    private void TransitionTo(OrderSagaStep nextStep)
    {
        CurrentStep = nextStep;
        UpdatedAt = DateTime.UtcNow;
    }
}
