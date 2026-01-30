namespace Payment.API.Application.Behaviors;

public class TransactionBehavior<TRequest, TResponse>(
    PaymentContext dbContext,
    IPaymentIntegrationEventService paymentIntegrationEventService,
    ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger = logger ?? throw new ArgumentException(nameof(ILogger));
    private readonly PaymentContext _dbContext = dbContext ?? throw new ArgumentException(nameof(PaymentContext));
    private readonly IPaymentIntegrationEventService _paymentIntegrationEventService
        = paymentIntegrationEventService ?? throw new ArgumentException(nameof(PaymentIntegrationEventService));

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = default(TResponse);
        var typeName = request.GetGenericTypeName();

        try
        {
            if (_dbContext.HasActiveTransaction)
            {
                return await next(cancellationToken);
            }

            var strategy = _dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                Guid transactionId;

                await using var transaction = await _dbContext.BeginTransactionAsync();
                using (_logger.BeginScope(new List<KeyValuePair<string, object>> { new("TransactionContext", transaction.TransactionId) }))
                {
                    _logger.LogInformation("Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, request);

                    response = await next();

                    _logger.LogInformation("Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);

                    await _dbContext.CommitTransactionAsync(transaction);

                    transactionId = transaction.TransactionId;
                }

                await _paymentIntegrationEventService.PublishEventsThroughEventBusAsync(transactionId);
            });

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Handling transaction for {CommandName} ({@Command})", typeName, request);

            throw;
        }
    }
}

