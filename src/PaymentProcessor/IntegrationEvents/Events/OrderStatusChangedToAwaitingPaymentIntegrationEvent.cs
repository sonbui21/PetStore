namespace PaymentProcessor.IntegrationEvents.Events;

public record OrderStatusChangedToAwaitingPaymentIntegrationEvent(Guid OrderId) : IntegrationEvent;
