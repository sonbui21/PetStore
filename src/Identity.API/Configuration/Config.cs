namespace Identity.API.Configuration;

public static class Config
{
    // ApiResources define the apis in your system
    public static IEnumerable<ApiResource> GetApis()
    {
        return
            [
                new ApiResource("orders", "Orders Service"),
                new ApiResource("basket", "Basket Service"),
                new ApiResource("SampleAPI", "SampleAPI Service"),
            ];
    }

    // ApiScope is used to protect the API 
    //The effect is the same as that of API resources in IdentityServer 3.x
    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return
            [
                new ApiScope("orders", "Orders Service"),
                new ApiScope("basket", "Basket Service"),
                new ApiScope("SampleAPI", "Sample API"),
            ];
    }

    // Identity resources are data like user ID, name, or email address of a user
    // see: http://docs.identityserver.io/en/release/configuration/resources.html
    public static IEnumerable<IdentityResource> GetResources()
    {
        return
            [
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            ];
    }

    // client want to access resources (aka scopes)
    public static IEnumerable<Client> GetClients(IConfiguration configuration)
    {
        return
            [
                new Client
                {
                    ClientId = "pet-ecommerce",
                    ClientName = "Pet Ecommerce",
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    RequireConsent = false,
                    RequirePkce = true,

                    RedirectUris = { $"{configuration["ClientUrls:PetEcommerce"]}/api/auth/callback/identity-service" },
                    PostLogoutRedirectUris = { $"{configuration["ClientUrls:PetEcommerce"]}" },
                    AllowedCorsOrigins= { $"{configuration["ClientUrls:PetEcommerce"]}" },

                    AllowedScopes =
                    [
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "orders",
                        "basket",
                        "SampleAPI"
                    ],
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AccessTokenLifetime = 60*60*2, // 2 hours
                    IdentityTokenLifetime= 60*60*2 // 2 hours
                },
                new Client
                {
                    ClientId = "pet-dashboard",
                    ClientName = "Pet Dashboard",
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    RequireConsent = false,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = { $"{configuration["ClientUrls:PetDashboard"]}" },
                    PostLogoutRedirectUris = { $"{configuration["ClientUrls:PetDashboard"]}" },
                    AllowedCorsOrigins= { $"{configuration["ClientUrls:PetDashboard"]}" },

                    AllowedScopes =
                    [
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "orders",
                        "basket",
                        "SampleAPI"
                    ],
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AccessTokenLifetime = 60*60*2, // 2 hours
                    IdentityTokenLifetime= 60*60*2 // 2 hours
                }

            ];
    }
}
