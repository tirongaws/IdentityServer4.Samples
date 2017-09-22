using Clients;
using IdentityModel.Client;
using System;
using System.Threading.Tasks;

namespace ConsoleResourceOwnerFlowUserInfo
{
    class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        static async Task MainAsync()
        {
            Console.Title = "Console ResourceOwner Flow UserInfo";

            var response = await RequestTokenAsync();
            response.Show();

            await GetClaimsAsync(response.AccessToken);
            Console.ReadLine();
        }

        static async Task<TokenResponse> RequestTokenAsync()
        {
            var disco = await GetAsync(Constants.Authority);
            if (disco.IsError) throw new Exception(disco.Error);

            var client = new TokenClient(
                disco.TokenEndpoint,
                "roclient",
                "roclient.secret");

            return await client.RequestResourceOwnerPasswordAsync("bob", "bob.password", "openid custom.profile");
        }

        static async Task GetClaimsAsync(string token)
        {
            var disco = await GetAsync(Constants.Authority);
            if (disco.IsError) throw new Exception(disco.Error);

            var client = new UserInfoClient(disco.UserInfoEndpoint);

            var response = await client.GetAsync(token);
            if (response.IsError) throw new Exception(response.Error);

            "\n\nUser claims:".ConsoleGreen();
            foreach (var claim in response.Claims)
            {
                Console.WriteLine("{0}\n {1}", claim.Type, claim.Value);
            }
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