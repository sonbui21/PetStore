namespace Payment.API.Domain.Events;

public record OrderPaymentSucceededIntegrationEvent(Guid OrderId) : IntegrationEvent;
