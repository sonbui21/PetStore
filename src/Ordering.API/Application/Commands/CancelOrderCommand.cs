namespace Ordering.API.Application.Commands;

public record CancelOrderCommand(Guid OrderId) : IRequest<bool>;
