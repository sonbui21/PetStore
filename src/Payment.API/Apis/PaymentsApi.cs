using Payment.API.Application.Commands;

namespace Payment.API.Apis;

public static class PaymentsApi
{
    public static RouteGroupBuilder MapPaymentsApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/payments");

        api.MapPost("/intents", CreatePaymentIntentsAsync);

        return api;
    }

    public static async Task<Results<Ok<CreatePaymentIntentRequestResponse>, BadRequest<string>>> CreatePaymentIntentsAsync(
        [FromHeader(Name = "x-requestid")] Guid requestId,
         CreatePaymentIntentRequest request,
        [AsParameters] PaymentServices services)
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
            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = await paymentIntentService.CreateAsync(new PaymentIntentCreateOptions
            {
                Amount = request.Amount,
                Currency = "usd",
                // In the latest version of the API, specifying the `automatic_payment_methods` parameter is optional because Stripe enables its functionality by default.
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
                CaptureMethod = "manual",
            });

            var payment = new Domain.Model.Payment()
            {
                PaymentIntentId = paymentIntent.Id,
                OrderId = request.OrderId,
                Amount = request.Amount,
                ClientSecret = paymentIntent.ClientSecret,
                Status = paymentIntent.Status,
            };

            services.PaymentRepository.Add(payment);
            await services.PaymentRepository.UnitOfWork.SaveEntitiesAsync();

            services.Logger.LogInformation("CreatePaymentCommand succeeded - RequestId: {RequestId}", requestId);
            return TypedResults.Ok(new CreatePaymentIntentRequestResponse(paymentIntent.ClientSecret));

            //var createPaymentCommand = new CreatePaymentCommand(
            //    paymentIntent.Id, request.OrderId, request.Amount, paymentIntent.Status, paymentIntent.ClientSecret);

            //var requestCreateOrder = new IdentifiedCommand<CreatePaymentCommand, bool>(createPaymentCommand, requestId);

            //services.Logger.LogInformation(
            //    "Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
            //    requestCreateOrder.GetGenericTypeName(),
            //    nameof(requestCreateOrder.Id),
            //    requestCreateOrder.Id,
            //    requestCreateOrder);

            //var result = await services.Mediator.Send(requestCreateOrder);

            //if (result)
            //{
            //    services.Logger.LogInformation("CreatePaymentCommand succeeded - RequestId: {RequestId}", requestId);
            //    return TypedResults.Ok(new CreatePaymentIntentRequestResponse(paymentIntent.ClientSecret));
            //}
            //else
            //{
            //    services.Logger.LogWarning("CreatePaymentCommand failed - RequestId: {RequestId}", requestId);
            //    return TypedResults.BadRequest("Failed to create payment.");
            //}
        }
    }
}

public record CreatePaymentIntentRequest(string UserId, Guid OrderId, long Amount);

public record CreatePaymentIntentRequestResponse(string ClientSecret);
