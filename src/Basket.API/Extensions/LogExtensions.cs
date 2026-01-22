namespace Basket.API.Extensions;

public static partial class LogExtensions
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})")]
    public static partial void LogHandlingIntegrationEvent(ILogger logger, Guid integrationEventId, IntegrationEvent @integrationEvent);

    [LoggerMessage(Level = LogLevel.Debug, Message = "Begin UpdateBasket call from method {Method} for basket id {Id}")]
    public static partial void LogBeginUpdateBasket(ILogger logger, string method, string id);
}
