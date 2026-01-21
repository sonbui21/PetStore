namespace Ordering.Domain.Events;

/// <summary>
/// Event used when the grace period order is confirmed
/// </summary>
public class OrderStatusChangedToAwaitingValidationDomainEvent(Guid orderId, IEnumerable<OrderItem> orderItems) : INotification
{
    public Guid OrderId { get; } = orderId;
    public IEnumerable<OrderItem> OrderItems { get; } = orderItems;
}
