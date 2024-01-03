using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
//using MultipleBlazorApps.SecondClient.Helpers;
//using MultipleBlazorApps.SecondClient.Repository;
using MultiBlazorApps.Components.Authentication;
using MultiBlazorApps.Components.Repository;
using MultiBlazorApps.Components.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;


namespace MultipleBlazorApps.SecondClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            Console.WriteLine($"{new Uri(builder.HostEnvironment.BaseAddress)}");
            ConfigureServices(builder.Services);
            await builder.Build().RunAsync();


        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions(); // Authourisation system
            services.AddScoped<IHttpService, HttpServices>();
            services.AddScoped<IPeopleRepository, PeopleRepository>();
            services.AddScoped<IReceiptRepository, ReceiptRepository>();
            services.AddScoped<ITenancyRepository, TenancyRepository>();
            services.AddScoped<IAccountsRepository, AccountsRepository>();
            services.AddAuthorizationCore();
            services.AddScoped<JWTAuthenticationStateProvider>();
            services.AddScoped<AuthenticationStateProvider, JWTAuthenticationStateProvider>(
                provider => provider.GetRequiredService<JWTAuthenticationStateProvider>()
            );
            services.AddScoped<ILoginService, JWTAuthenticationStateProvider>(
                provider => provider.GetRequiredService<JWTAuthenticationStateProvider>()
            );

            services.AddScoped<TokenRenewer>();

        }
    }
}
