namespace Ordering.Domain.Events;

public class BuyerAndPaymentMethodVerifiedDomainEvent(Buyer buyer, PaymentMethod payment, Guid orderId) : INotification
{
    public Buyer Buyer { get; private set; } = buyer;
    public PaymentMethod Payment { get; private set; } = payment;
    public Guid OrderId { get; private set; } = orderId;
}
