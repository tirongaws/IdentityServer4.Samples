using Clients;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleClientCredentialsFlow
{
    public class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        public static async Task MainAsync()
        {
            Console.Title = "Console Client Credentials Flow";

            var response = await RequestTokenAsync();
            response.Show();

            Console.ReadLine();
            await CallServiceAsync(response.AccessToken);
            Console.ReadLine();
        }

        static async Task<TokenResponse> RequestTokenAsync()
        {
            var disco = await GetAsync(Constants.Authority);
            if (disco.IsError) throw new Exception(disco.Error);

            var client = new TokenClient(
                disco.TokenEndpoint,
                "client",
                "client.secret");

            return await client.RequestClientCredentialsAsync("api1");
        }

        public static async Task<DiscoveryResponse> GetAsync(string authority)
        {
            using (var client = new DiscoveryClient(authority))
            {
                client.Policy.RequireHttps = false;
                return await client.GetAsync().ConfigureAwait(false);
            }
        }

        static async Task CallServiceAsync(string token)
        {
            var baseAddress = Constants.SampleApi;

            var client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
            var t1 = @"eyJhbGciOiJSUzI1NiIsImtpZCI6IjZhNDRkOTg3NzcwMTM4Njg2MDE1NWRlODI5ODc0OTcwIiwidHlwIjoiSldUIn0.eyJuYmYiOjE1MDE2NjMxMzYsImV4cCI6MTUwMTY2NjczNiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6NTAwMC9yZXNvdXJjZXMiLCJhcGkxIl0sImNsaWVudF9pZCI6ImNsaWVudCIsInNjb3BlIjpbImFwaTEiXX0.qy4GrgtY21BbLFxjcoIQcE6DWJsgpbkjeNTtPLrX0O0Oi2BKzjjwdvrsUBB681NoNwT301Ivfw7SoqrLwsCt1Hmq2iO6hPOGlpJrehgW9papOBGfnCP4UZ6ItNlFvDsLDDzw0khghpnXwV-4RzyHP5lRJXathB1rKcItVCkqUgLx54tcW3C3QBxFjblcCgm4h4WEIjk5TJuPlm5vFYT5uDLv-GD4MJTZegoXuktXtycrv3cGP3UNWNdGI6D7vUIzf4lcEwSztjvj-4V2KVtSqyWQnptTcr6wq7DtzDepuZ4lJUx6ZM2nC1UZ63tvdnisgERxrtawIPveH32lKCqf7w";
            var t2 = @"eyJhbGciOiJSUzI1NiIsImtpZCI6IjZhNDRkOTg3NzcwMTM4Njg2MDE1NWRlODI5ODc0OTcwIiwidHlwIjoiSldUIn0.eyJuYmYiOjE1MDE2NjM3MTQsImV4cCI6MTUwMTY2NzMxNCwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6NTAwMC9yZXNvdXJjZXMiLCJhcGkxIl0sImNsaWVudF9pZCI6ImNsaWVudCIsInNjb3BlIjpbImFwaTEiXX0.lrBxhKOPjG9IwPra2qlLGrwKUCszOWKGjjAwJGC-ojhpTnxGM_ywJ-1CTqI8EuMtK9KjI3THe0Mi4FL9viwivQ6_b5oUEL054A5wv4XkFVl164c2KgiZCMYmC7H4g_Ebdae-Czl6NOJM_rAfCtXgqPXDlIECwG6zzAothGOVmuV2zkTL2o7uL23dOtk596OJuzKOCCKQ9henrqeX0ocNHwdqMB3a_Hxz_uoc38cJuQGyklb3sg_-DPIYNxxu6PheD3YoPC-1DauHIM1J7fo9TTuOP6AGIihIO1e7pBMYrEihsltTJI3FZWySe__zuHdG_kWf4NZOy0c_LrRwXMlhjg";
            var t3 = @"eyJhbGciOiJSUzI1NiIsImtpZCI6IjZhNDRkOTg3NzcwMTM4Njg2MDE1NWRlODI5ODc0OTcwIiwidHlwIjoiSldUIn0.eyJuYmYiOjE1MDE2NjM3MTQsImV4cCI6MTUwMTY2NzMxNCwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6NTAwMC9yZXNvdXJjZXMiLCJhcGkxIl0sImNsaWVudF9pZCI6ImNsaWVudCIsInNjb3BlIjpbImFwaTEiXX0.qy4GrgtY21BbLFxjcoIQcE6DWJsgpbkjeNTtPLrX0O0Oi2BKzjjwdvrsUBB681NoNwT301Ivfw7SoqrLwsCt1Hmq2iO6hPOGlpJrehgW9papOBGfnCP4UZ6ItNlFvDsLDDzw0khghpnXwV-4RzyHP5lRJXathB1rKcItVCkqUgLx54tcW3C3QBxFjblcCgm4h4WEIjk5TJuPlm5vFYT5uDLv-GD4MJTZegoXuktXtycrv3cGP3UNWNdGI6D7vUIzf4lcEwSztjvj-4V2KVtSqyWQnptTcr6wq7DtzDepuZ4lJUx6ZM2nC1UZ63tvdnisgERxrtawIPveH32lKCqf7w";
            //token = t3;
            client.SetBearerToken(token);
            var response = await client.GetStringAsync("identity");

            "\n\nService claims:".ConsoleGreen();
            Console.WriteLine(JArray.Parse(response));
        }
    }
}