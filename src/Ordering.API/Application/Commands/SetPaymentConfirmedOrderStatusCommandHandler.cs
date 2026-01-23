namespace Ordering.API.Application.Commands;

// Regular CommandHandler
public class SetPaymentConfirmedOrderStatusCommandHandler(IOrderRepository orderRepository) : IRequestHandler<SetPaymentConfirmedOrderStatusCommand, bool>
{
    /// <summary>
    /// Handler which processes the command when
    /// payment authorization succeeds.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<bool> Handle(SetPaymentConfirmedOrderStatusCommand command, CancellationToken cancellationToken)
    {
        var orderToUpdate = await orderRepository.GetAsync(command.OrderId);
        if (orderToUpdate == null)
        {
            return false;
        }

        orderToUpdate.SetPaymentConfirmedStatus();
        return await orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}

// Use for Idempotency in Command process
public class SetPaymentConfirmedIdentifiedOrderStatusCommandHandler(
    IMediator mediator,
    IRequestManager requestManager,
    ILogger<IdentifiedCommandHandler<SetPaymentConfirmedOrderStatusCommand, bool>> logger)
    : IdentifiedCommandHandler<SetPaymentConfirmedOrderStatusCommand, bool>(mediator, requestManager, logger)
{
    protected override bool CreateResultForDuplicateRequest()
    {
        return true; // Ignore duplicate requests for processing order.
    }
}
