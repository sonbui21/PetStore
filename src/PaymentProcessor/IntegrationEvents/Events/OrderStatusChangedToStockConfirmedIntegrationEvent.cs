namespace PaymentProcessor.IntegrationEvents.Events;

public record OrderStatusChangedToStockConfirmedIntegrationEvent(Guid OrderId) : IntegrationEvent;
