var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddApplicationServices();
builder.Services.AddProblemDetails();

builder.AddDefaultOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();


app.MapOrdersApi()
    .RequireAuthorization();

app.UseDefaultOpenApi();
await app.RunAsync();
