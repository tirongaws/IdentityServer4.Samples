using Clients;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleCustomGrant
{
    class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        static async Task MainAsync()
        {
            Console.Title = "Console Custom Grant";

            // custom grant type with subject support
            var response = await RequestTokenAsync("custom");
            response.Show();

            Console.ReadLine();
            await CallServiceAsync(response.AccessToken);

            Console.ReadLine();

            // custom grant type without subject support
            response = await RequestTokenAsync("custom.nosubject");
            response.Show();

            Console.ReadLine();
            await CallServiceAsync(response.AccessToken);
            Console.ReadLine();
        }

        static async Task<TokenResponse> RequestTokenAsync(string grantType)
        {
            var disco = await GetAsync(Constants.Authority);
            if (disco.IsError) throw new Exception(disco.Error);

            var client = new TokenClient(
                disco.TokenEndpoint,
                "client.custom",
                "secret");

            var customParameters = new Dictionary<string, string>
                {
                    { "custom_credential", "custom credential11111111111"}
                };

            return await client.RequestCustomGrantAsync(grantType, "api1", customParameters);
        }

        static async Task CallServiceAsync(string token)
        {
            var baseAddress = Constants.SampleApi;

            var client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };

            client.SetBearerToken(token);
            var response = await client.GetStringAsync("identity");

            "\n\nService claims:".ConsoleGreen();
            Console.WriteLine(JArray.Parse(response));
        }

        public static async Task<DiscoveryResponse> GetAsync(string authority)
        {
            using (var client = new DiscoveryClient(authority))
            {
                client.Policy.RequireHttps = false;
                return await client.GetAsync().ConfigureAwait(false);
            }
        }
    }
}