namespace Catalog.API.IntegrationEvents.Events;

public record ConfirmedOrderStockItem(Guid ProductId, bool HasStock);
