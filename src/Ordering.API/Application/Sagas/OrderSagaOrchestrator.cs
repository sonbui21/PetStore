namespace Ordering.API.Application.Sagas;

public class OrderSagaOrchestrator(
    IOrderSagaRepository sagaRepository,
    IMediator mediator,
    ILogger<OrderSagaOrchestrator> logger) : IOrderSagaOrchestrator
{
    private readonly IOrderSagaRepository _sagaRepository = sagaRepository ?? throw new ArgumentNullException(nameof(sagaRepository));
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    private readonly ILogger<OrderSagaOrchestrator> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task StartSagaAsync(Guid orderId)
    {
        var saga = await _sagaRepository.GetAsync(orderId);
        if (saga != null)
        {
            _logger.LogInformation("Order saga already exists for order {OrderId} at step {Step}", orderId, saga.CurrentStep);
            return;
        }

        saga = OrderSagaState.Start(orderId);
        _sagaRepository.Add(saga);

        _logger.LogInformation("Order saga started for order {OrderId}", orderId);
        await _sagaRepository.UnitOfWork.SaveEntitiesAsync();
    }

    public async Task HandleGracePeriodConfirmedAsync(Guid orderId)
    {
        var saga = await GetOrCreateSagaAsync(orderId);
        saga.MarkAwaitingValidation();
        _sagaRepository.Update(saga);
        await _sagaRepository.UnitOfWork.SaveEntitiesAsync();

        var command = new SetAwaitingValidationOrderStatusCommand(orderId);
        await SendCommandAsync(command);
    }

    public async Task HandleStockConfirmedAsync(Guid orderId)
    {
        var saga = await GetOrCreateSagaAsync(orderId);
        saga.MarkStockConfirmed();
        _sagaRepository.Update(saga);
        await _sagaRepository.UnitOfWork.SaveEntitiesAsync();

        var command = new SetStockConfirmedOrderStatusCommand(orderId);
        await SendCommandAsync(command);
    }

    public async Task HandleStockRejectedAsync(Guid orderId, List<Guid> orderStockItems)
    {
        var saga = await GetOrCreateSagaAsync(orderId);
        saga.MarkStockRejected();
        saga.MarkCancelled();
        _sagaRepository.Update(saga);
        await _sagaRepository.UnitOfWork.SaveEntitiesAsync();

        var command = new SetStockRejectedOrderStatusCommand(orderId, orderStockItems);
        await SendCommandAsync(command);
    }

    public async Task HandlePaymentSucceededAsync(Guid orderId)
    {
        var saga = await GetOrCreateSagaAsync(orderId);
        saga.MarkPaymentSucceeded();
        saga.MarkCompleted();
        _sagaRepository.Update(saga);
        await _sagaRepository.UnitOfWork.SaveEntitiesAsync();

        var command = new SetPaidOrderStatusCommand(orderId);
        await SendCommandAsync(command);
    }

    public async Task HandlePaymentFailedAsync(Guid orderId)
    {
        var saga = await GetOrCreateSagaAsync(orderId);
        saga.MarkPaymentFailed();
        saga.MarkCancelled();
        _sagaRepository.Update(saga);
        await _sagaRepository.UnitOfWork.SaveEntitiesAsync();

        var command = new CancelOrderCommand(orderId);
        await SendCommandAsync(command);
    }

    private async Task<OrderSagaState> GetOrCreateSagaAsync(Guid orderId)
    {
        var saga = await _sagaRepository.GetAsync(orderId);
        if (saga != null)
        {
            return saga;
        }

        saga = OrderSagaState.Start(orderId);
        _sagaRepository.Add(saga);
        await _sagaRepository.UnitOfWork.SaveEntitiesAsync();

        _logger.LogInformation("Order saga created for order {OrderId}", orderId);
        return saga;
    }

    private async Task SendCommandAsync<TCommand>(TCommand command) where TCommand : IRequest<bool>
    {
        _logger.LogInformation("Saga orchestrator sending command {CommandName} ({@Command})", command.GetType().Name, command);
        await _mediator.Send(command);
    }
}
