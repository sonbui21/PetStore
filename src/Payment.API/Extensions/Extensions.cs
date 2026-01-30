using Payment.API.Application.IntegrationEvents;

namespace Payment.API.Extensions;

internal static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        // Add the authentication services to DI
        builder.AddDefaultAuthentication();

        services.AddDbContext<PaymentContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("paymentdb"));
        });
        builder.EnrichNpgsqlDbContext<PaymentContext>();

        services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService<PaymentContext>>();

        services.AddTransient<IPaymentIntegrationEventService, PaymentIntegrationEventService>();

        builder.AddRabbitMqEventBus("eventbus")
               .AddEventBusSubscriptions();

        services.AddHttpContextAccessor();

        // Configure mediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<Program>();

            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
            cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
        });

        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IRequestManager, RequestManager>();

        StripeConfiguration.ApiKey = builder.Configuration
            .GetRequiredSection("StripeConfiguration")
            .GetSection("ApiKey").Value;
    }

    private static void AddEventBusSubscriptions(this IEventBusBuilder eventBus)
    {
        eventBus.AddSubscription<
            OrderStatusChangedToStockConfirmedIntegrationEvent,
            OrderStatusChangedToStockConfirmedIntegrationEventHandler>();
    }
}