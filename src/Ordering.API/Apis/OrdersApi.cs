using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using CardType = Ordering.API.Application.Queries.CardType;
using Order = Ordering.API.Application.Queries.Order;

namespace Ordering.API.Apis;

public static class OrdersApi
{
    public static RouteGroupBuilder MapOrdersApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/orders");

        api.MapGet("/", GetOrdersByUserAsync);

        api.MapPost("/", CreateOrderAsync);

        api.MapGet("{orderId:guid}", GetOrderAsync);

        return api;
    }

    public static async Task<Results<Ok<string>, BadRequest<string>>> CreateOrderAsync(
        [FromHeader(Name = "x-requestid")] Guid requestId,
        CreateOrderRequest request,
        [AsParameters] OrderServices services)
    {
        services.Logger.LogInformation(
            "Sending command: {CommandName} - {IdProperty}: {CommandId}",
            request.GetGenericTypeName(),
            nameof(request.UserId),
            request.UserId);

        if (requestId == Guid.Empty)
        {
            services.Logger.LogWarning("Invalid IntegrationEvent - RequestId is missing - {@IntegrationEvent}", request);
            return TypedResults.BadRequest("RequestId is missing.");
        }

        using (services.Logger.BeginScope(new List<KeyValuePair<string, object>> { new("IdentifiedCommandId", requestId) }))
        {
            var maskedCCNumber = request.CardNumber[^4..].PadLeft(request.CardNumber.Length, 'X');

            var orderId = Guid.CreateVersion7();

            var createOrderCommand = new CreateOrderCommand(request.Items, orderId, request.UserId, request.UserName,
                request.Name, request.Phone, request.City, request.Street, request.State, request.Country, request.ZipCode,
                maskedCCNumber, request.CardHolderName, request.CardExpiration, request.CardSecurityNumber, request.CardTypeId);

            var requestCreateOrder = new IdentifiedCommand<CreateOrderCommand, bool>(createOrderCommand, requestId);

            services.Logger.LogInformation(
                "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                requestCreateOrder.GetGenericTypeName(),
                nameof(requestCreateOrder.Id),
                requestCreateOrder.Id,
                requestCreateOrder);

            var result = await services.Mediator.Send(requestCreateOrder);

            if (result)
            {
                services.Logger.LogInformation("CreateOrderCommand succeeded - RequestId: {RequestId}", requestId);
                return TypedResults.Ok(orderId.ToString());
            }
            else
            {
                services.Logger.LogWarning("CreateOrderCommand failed - RequestId: {RequestId}", requestId);
                return TypedResults.BadRequest("Failed to place order.");
            }
        }
    }

    public static async Task<Ok<ResultList<OrderSummary>>> GetOrdersByUserAsync([AsParameters] OrderServices services)
    {
        var userId = services.IdentityService.GetUserIdentity();
        var orders = await services.Queries.GetOrdersFromUserAsync(userId);
        return TypedResults.Ok(orders);
    }

    public static async Task<Results<Ok<Order>, NotFound>> GetOrderAsync(Guid orderId, [AsParameters] OrderServices services)
    {
        try
        {
            var order = await services.Queries.GetOrderAsync(orderId);
            return TypedResults.Ok(order);
        }
        catch
        {
            return TypedResults.NotFound();
        }
    }
}

public record CreateOrderRequest(
    string UserId,
    string UserName,
    string Street,
    string Name,
    string Phone,
    string City,
    string State,
    string Country,
    string ZipCode,
    string CardNumber,
    string CardHolderName,
    DateTime CardExpiration,
    string CardSecurityNumber,
    int CardTypeId,
    string Buyer,
    List<BasketItem> Items);