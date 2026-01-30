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
                new ApiResource("payments", "Payment Service"),
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
                new ApiScope("payments", "Payments Service"),
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
                    PostLogoutRedirectUris =
                        {
                            $"{configuration["ClientUrls:PetEcommerce"]}/api/auth/signout"
                        },
                    AllowedCorsOrigins= { $"{configuration["ClientUrls:PetEcommerce"]}" },

                    AllowedScopes =
                    [
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "orders",
                        "basket",
                    ],
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AccessTokenLifetime = 60*60*2, // 2 hours
                    IdentityTokenLifetime= 60*60*2 // 2 hours
                },
                new Client
                {
                    ClientId = "postman-pkce",
                    ClientName = "Postman PKCE",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = { "https://oauth.pstmn.io/v1/callback" },
                    PostLogoutRedirectUris = { "https://oauth.pstmn.io/v1/callback" },
                    AllowedCorsOrigins = { "https://oauth.pstmn.io" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "orders",
                        "basket",
                    },
                    AllowOfflineAccess = true,

                    AccessTokenLifetime = 60 * 60 * 2,
                    IdentityTokenLifetime = 60 * 60 * 2
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
                    ],
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AccessTokenLifetime = 60*60*2,
                    IdentityTokenLifetime= 60*60*2
                },
                new Client
                {
                    ClientId = "orderingscalar",
                    ClientName = "Ordering Scalar UI",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = { $"{configuration["OrderingApiClient"]}/scalar/" },
                    PostLogoutRedirectUris = { $"{configuration["OrderingApiClient"]}/scalar/" },
                    AllowedCorsOrigins = { $"{configuration["OrderingApiClient"]}" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "orders",
                    },

                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AccessTokenLifetime = 60*60*2,
                    IdentityTokenLifetime= 60*60*2
                },
                new Client
                {
                    ClientId = "paymentscalar",
                    ClientName = "Payment Scalar UI",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = { $"{configuration["PaymentApiClient"]}/scalar/" },
                    PostLogoutRedirectUris = { $"{configuration["PaymentApiClient"]}/scalar/" },
                    AllowedCorsOrigins = { $"{configuration["PaymentApiClient"]}" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "payments",
                    },

                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AccessTokenLifetime = 60*60*2,
                    IdentityTokenLifetime= 60*60*2
                },

            ];
    }
}
