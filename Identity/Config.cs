using Duende.IdentityServer.Models;

namespace Identity;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("ReadingListApi.read"),
            new ApiScope("ReadingListApi.write")
        };

    public static IEnumerable<ApiResource> ApiResources => new[]
    {
        new ApiResource("readingList")
        {
            Scopes = new List<string> {"ReadingListApi.read", "ReadingListApi.write"},
            ApiSecrets = new List<Secret> {new Secret("ScopeSecret".Sha256())},
            UserClaims = new List<string> {"role"}
        }
    };
    
    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // m2m client credentials flow client
            /*new Client
            {
                ClientId = "m2m.client",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,

                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },
                AllowedScopes = {"weatherapi.read", "weatherapi.write"}
            },*/

            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "swagger",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },
                    
                AllowedGrantTypes = GrantTypes.Code,

                RequirePkce = true,
                RedirectUris = { "https://localhost:7007/swagger/oauth2-redirect.html" },
                AllowedCorsOrigins = {"https://localhost:7007"},

                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "ReadingListApi.read", "ReadingListApi.write" },
                RequireConsent = true
            }
        };
}