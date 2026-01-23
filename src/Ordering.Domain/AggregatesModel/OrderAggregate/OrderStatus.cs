using System.Text.Json.Serialization;

namespace Ordering.Domain.AggregatesModel.OrderAggregate;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatus
{
    Submitted = 1,
    AwaitingValidation = 2,
    StockConfirmed = 3,
    AwaitingPayment = 4,
    PaymentConfirmed = 5,
    Paid = 6,
    Shipped = 7,
    Cancelled = 8
}
