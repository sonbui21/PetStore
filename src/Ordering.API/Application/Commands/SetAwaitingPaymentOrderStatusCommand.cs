namespace Ordering.API.Application.Commands;

public record SetAwaitingPaymentOrderStatusCommand(Guid OrderId) : IRequest<bool>;
