namespace Ordering.Domain.Events;

/// <summary>
/// Event used when the order stock items are confirmed
/// </summary>
public class OrderStatusChangedToStockConfirmedDomainEvent(Guid orderId) : INotification
{
    public Guid OrderId { get; } = orderId;
}
