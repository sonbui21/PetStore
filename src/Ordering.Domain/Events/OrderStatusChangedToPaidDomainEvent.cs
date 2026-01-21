namespace Ordering.Domain.Events;

/// <summary>
/// Event used when the order is paid
/// </summary>
public class OrderStatusChangedToPaidDomainEvent(Guid orderId, IEnumerable<OrderItem> orderItems) : INotification
{
    public Guid OrderId { get; } = orderId;
    public IEnumerable<OrderItem> OrderItems { get; } = orderItems;
}
