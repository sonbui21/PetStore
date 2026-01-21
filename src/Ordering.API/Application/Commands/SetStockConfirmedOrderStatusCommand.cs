namespace Ordering.API.Application.Commands;

public record SetStockConfirmedOrderStatusCommand(Guid OrderId) : IRequest<bool>;
