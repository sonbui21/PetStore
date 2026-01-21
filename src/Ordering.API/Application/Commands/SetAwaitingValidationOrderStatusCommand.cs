namespace Ordering.API.Application.Commands;

public record SetAwaitingValidationOrderStatusCommand(Guid OrderId) : IRequest<bool>;