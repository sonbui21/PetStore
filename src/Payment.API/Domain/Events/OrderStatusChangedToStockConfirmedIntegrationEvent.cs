namespace Payment.API.Domain.Events;

public record OrderStatusChangedToStockConfirmedIntegrationEvent(Guid OrderId) : IntegrationEvent;
