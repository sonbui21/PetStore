namespace Ordering.Domain.Events;

/// <summary>
/// Event used when the payment authorization is confirmed.
/// </summary>
public class OrderStatusChangedToPaymentConfirmedDomainEvent(Guid orderId, IEnumerable<OrderItem> orderItems) : INotification
{
    public Guid OrderId { get; } = orderId;
    public IEnumerable<OrderItem> OrderItems { get; } = orderItems;
}
