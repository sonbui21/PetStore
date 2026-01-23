namespace Ordering.API.Application.Commands;

// Regular CommandHandler
public class SetAwaitingPaymentOrderStatusCommandHandler(IOrderRepository orderRepository) : IRequestHandler<SetAwaitingPaymentOrderStatusCommand, bool>
{

    /// <summary>
    /// Handler which processes the command when
    /// graceperiod has finished
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<bool> Handle(SetAwaitingPaymentOrderStatusCommand command, CancellationToken cancellationToken)
    {
        var orderToUpdate = await orderRepository.GetAsync(command.OrderId);
        if (orderToUpdate == null)
        {
            return false;
        }

        orderToUpdate.SetAwaitingPaymentStatus();
        return await orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}


// Use for Idempotency in Command process
public class SetAwaitingPaymentIdentifiedOrderStatusCommandHandler(
    IMediator mediator,
    IRequestManager requestManager,
    ILogger<IdentifiedCommandHandler<SetAwaitingPaymentOrderStatusCommand, bool>> logger)
    : IdentifiedCommandHandler<SetAwaitingPaymentOrderStatusCommand, bool>(mediator, requestManager, logger)
{
    protected override bool CreateResultForDuplicateRequest()
    {
        return true; // Ignore duplicate requests for processing order.
    }
}
