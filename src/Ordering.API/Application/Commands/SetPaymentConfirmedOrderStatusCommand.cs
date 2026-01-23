namespace Ordering.API.Application.Commands;

public record SetPaymentConfirmedOrderStatusCommand(Guid OrderId) : IRequest<bool>;
