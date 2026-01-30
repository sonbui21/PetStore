namespace Payment.API.Domain.Events;

public record OrderPaymentFailedIntegrationEvent(Guid OrderId) : IntegrationEvent;