using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace SentinelForwarder.Service
{
    public abstract class AzureSentinelServiceBase
    {
        public static async Task<string> GetTokenAsync(string tenantId, string clientId, string secret) {
            var app = ConfidentialClientApplicationBuilder.Create(clientId)
            .WithClientSecret(secret)
            .WithTenantId(tenantId)
            .Build();

            var scopes = new string[] { "https://management.core.windows.net/.default" };

            AuthenticationResult result = null;

            result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
            return result.AccessToken;
        }

        public JsonSerializerOptions GetJsonSerializerOptions () {
            var stringEnumConverter = new System.Text.Json.Serialization.JsonStringEnumConverter();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            options.Converters.Add(stringEnumConverter);

            return options;
        }

        public async Task<HttpClient> GetAuthenticatedHttpClientAsync(string tenantId, string clientId, string secret) {
            var token = await GetTokenAsync(tenantId, clientId, secret);
            
            var httpClient = new HttpClient();
            
            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            return httpClient;
        }
    }
}