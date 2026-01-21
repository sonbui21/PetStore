namespace Ordering.API.Application.Commands;

public record ShipOrderCommand(Guid OrderId) : IRequest<bool>;
