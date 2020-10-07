using System.Collections;
using System.Collections.Generic;
using IdentityModel;
using IdentityServer4.Models;

namespace IdentityServer
{
    public static class IdentityServerConfiguration
    {
        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "protected_grpc",
                    ClientSecrets = { new Secret { Value = "grpc_secret".ToSha256() } },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "grpc_scope" },
                },
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new[]
            {
                new ApiScope("grpc_scope"),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource("grpc_scope"),
            };
        }
    }
}