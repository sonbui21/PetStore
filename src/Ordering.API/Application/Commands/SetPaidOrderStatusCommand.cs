namespace Ordering.API.Application.Commands;

public record SetPaidOrderStatusCommand(Guid OrderId) : IRequest<bool>;
