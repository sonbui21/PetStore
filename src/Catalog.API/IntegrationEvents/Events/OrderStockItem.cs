namespace Catalog.API.IntegrationEvents.Events;

public record OrderStockItem(Guid ProductId, Guid VariantId, int Quantity);
