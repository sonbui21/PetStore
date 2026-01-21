namespace Ordering.API.Application.Commands;

public record SetStockRejectedOrderStatusCommand(Guid OrderId, List<Guid> OrderStockItems) : IRequest<bool>;
