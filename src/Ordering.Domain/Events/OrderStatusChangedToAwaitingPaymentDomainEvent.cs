namespace Ordering.Domain.Events;

/// <summary>
/// Event used when the order moves to awaiting payment authorization.
/// </summary>
public class OrderStatusChangedToAwaitingPaymentDomainEvent(Guid orderId) : INotification
{
    public Guid OrderId { get; } = orderId;
}
