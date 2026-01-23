namespace Ordering.Domain.Sagas;

public enum OrderSagaStep
{
    Started = 0,
    AwaitingValidation = 1,
    StockConfirmed = 2,
    StockRejected = 3,
    PaymentSucceeded = 4,
    PaymentFailed = 5,
    Cancelled = 6,
    Completed = 7
}
