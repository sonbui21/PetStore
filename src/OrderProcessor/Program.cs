var builder = WebApplication.CreateBuilder(args);

builder.AddBasicServiceDefaults();

builder.AddRabbitMqEventBus("eventbus")
    .ConfigureJsonOptions(options => options.TypeInfoResolverChain.Add(IntegrationEventContext.Default));

builder.AddNpgsqlDataSource("orderingdb");

builder.Services.AddOptions<BackgroundTaskOptions>()
    .BindConfiguration(nameof(BackgroundTaskOptions));

builder.Services.AddHostedService<GracePeriodManagerService>();

var app = builder.Build();

app.MapDefaultEndpoints();

await app.RunAsync();

