using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;

namespace PetStore.ServiceDefaults;

public static partial class Extensions
{
    public static IApplicationBuilder UseDefaultOpenApi(this WebApplication app)
    {
        var configuration = app.Configuration;
        var openApiSection = configuration.GetSection("OpenApi");

        if (!openApiSection.Exists())
        {
            return app;
        }

        if (app.Environment.IsDevelopment())
        {
            var clientId = openApiSection.GetSection("Auth").GetSection("ClientId").Value;

            var identitySection = configuration.GetSection("Identity");
            var scopes = identitySection.Exists()
                ? identitySection.GetRequiredSection("Scopes").GetChildren().ToDictionary(p => p.Key, p => p.Value)
                : [];

            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                // Disable default fonts to avoid download unnecessary fonts
                options.DefaultFonts = false;

                options.AddPreferredSecuritySchemes("OAuth2")
                .AddAuthorizationCodeFlow("OAuth2", flow =>
                {
                    flow.ClientId = clientId;
                    flow.SelectedScopes = [.. scopes.Keys];
                    flow.Pkce = Pkce.Sha256;
                });

            });

            app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();
        }

        return app;
    }

    public static IHostApplicationBuilder AddDefaultOpenApi(this IHostApplicationBuilder builder)
    {
        var openApi = builder.Configuration.GetSection("OpenApi");

        if (!openApi.Exists())
        {
            return builder;
        }

        var identitySection = builder.Configuration.GetSection("Identity");

        var scopes = identitySection.Exists()
            ? identitySection.GetRequiredSection("Scopes").GetChildren().ToDictionary(p => p.Key, p => p.Value)
            : [];

        builder.Services.AddOpenApi("v1", options =>
        {
            options.ApplyApiVersionInfo(openApi.GetRequiredValue("Document:Title"), openApi.GetRequiredValue("Document:Description"));
            options.ApplyAuthorizationChecks([.. scopes.Keys]);
            options.ApplySecuritySchemeDefinitions();
            options.ApplyOperationDeprecatedStatus();
        });

        return builder;
    }
}
