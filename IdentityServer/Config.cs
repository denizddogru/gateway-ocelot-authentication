namespace IdentityServer;
using IdentityServer4.Models;

public static class Config
{
    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new("demoapi", "Demo API")
            {
                Scopes = { "demoapi.read", "demoapi.write" }
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("demoapi.read", "Demo API Read"),
            new ApiScope("demoapi.write", "Demo API Write")
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new() {
                ClientId = "gateway",
                ClientName = "Gateway Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("gateway-secret".Sha256()) },
                AllowedScopes = { "demoapi.read", "demoapi.write" }
            }
        };
}