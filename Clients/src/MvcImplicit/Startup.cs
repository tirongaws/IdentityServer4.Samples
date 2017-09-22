using Clients;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace MvcImplicit
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            HostingEnvironment = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                        .AddCookie(o => o.LoginPath = new PathString("/login"));
                        //.AddFacebook(o =>
                        //{
                        //    o.AppId = Configuration["facebook:appid"];
                        //    o.AppSecret = Configuration["facebook:appsecret"];
                        //});
        }

        public void Configure(IApplicationBuilder app)
        {
            if (HostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            //var c = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions
            //{
            //    //AuthenticationScheme = "Cookies",

            //    //AutomaticAuthenticate = true,

            //    ExpireTimeSpan = TimeSpan.FromMinutes(60),
            //    CookieDomain = "mvcimplicit"
            //};
           
            //app.UseCookieAuthentication(c);

            app.UseAuthentication();


            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseOpenIdConnectAuthentication(new Microsoft.AspNetCore.Authentication.OpenIdConnectOpenIdConnectOptions
            {
                AuthenticationScheme = "oidc",
                SignInScheme = "Cookies",

                Authority = Constants.Authority,
                RequireHttpsMetadata = false,

                ClientId = "mvc.implicit",

                ResponseType = "id_token",
                Scope = { "openid", "profile", "email" },

                SaveTokens = true,

                TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role,
                },

                //Events = new OpenIdConnectEvents
                //{
                //    OnRedirectToIdentityProvider = n =>
                //    {
                //        // acr_values is where you can pass custom hints/params
                //        //n.ProtocolMessage.AcrValues = "tenant:foo";
                //        //n.ProtocolMessage.AcrValues = "idp:Google";
                //        return Task.FromResult(0);
                //    }
                //}
            });

            app.UseMvcWithDefaultRoute();
        }
    }
}