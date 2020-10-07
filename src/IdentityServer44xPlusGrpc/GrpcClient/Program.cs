using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcService;
using IdentityModel.Client;

namespace GrpcClient
{
    class Program
    {
        private const string GrpcServerAddress = "https://localhost:5005";
        private const string IdentityServerAddress = "https://localhost:5001";
        private const string ApiName = "protected_grpc";
        private const string ApiSecret = "grpc_secret";
        private const string ApiScope = "grpc_scope";

        static async Task Main(string[] args)
        {
            Console.ReadLine();

            using var authClient = new HttpClient();
            var discoveryDocument = await authClient.GetDiscoveryDocumentAsync(IdentityServerAddress);

            var tokenResponse = await authClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = ApiName,
                ClientSecret = ApiSecret,
                Scope = ApiScope,
            });

            using var callClient = new HttpClient()
            {
                DefaultRequestVersion = Version.Parse("2.0")
            };
            callClient.SetBearerToken(tokenResponse.AccessToken);
            var response = await callClient.GetAsync($"{GrpcServerAddress}/secret");
            var str = await response.Content.ReadAsStringAsync();
            Console.WriteLine(str);
            Console.ReadLine();
            //return;


            var metadata = new Metadata
            {
                { "Authorization", $"Bearer {tokenResponse.AccessToken}"}
            };

            var callOptions = new CallOptions(metadata);

            using var channel = GrpcChannel.ForAddress(GrpcServerAddress);
            var client = new Greeter.GreeterClient(channel);

            var reply = await client.SayHelloAsync(new HelloRequest
            {
                Name = "GreeterClient"
            }, callOptions);
            Console.WriteLine($"Greeting: {reply.Message}");
            Console.ReadKey();
        }
    }
}
