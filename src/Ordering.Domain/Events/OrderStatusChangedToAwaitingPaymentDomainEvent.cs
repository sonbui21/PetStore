namespace Ordering.Domain.Events;

/// <summary>
/// Event used when the order enters payment authorization.
/// </summary>
public class OrderStatusChangedToAwaitingPaymentDomainEvent(Guid orderId) : INotification
{
    public Guid OrderId { get; } = orderId;
}
