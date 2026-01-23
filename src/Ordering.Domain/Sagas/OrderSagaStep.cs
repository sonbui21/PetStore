namespace Ordering.Domain.Sagas;

public enum OrderSagaStep
{
    Started = 0,
    AwaitingPayment = 1,
    PaymentConfirmed = 2,
    StockConfirmed = 3,
    StockRejected = 4,
    PaymentFailed = 5,
    Cancelled = 6,
    Completed = 7
}
