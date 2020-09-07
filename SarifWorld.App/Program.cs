using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace SarifWorld.App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddLocalization();

            WebAssemblyHost host = builder.Build();

            var jsInterop = host.Services.GetRequiredService<IJSRuntime>();
            var language = await jsInterop.InvokeAsync<string>("getLanguage");

            var culture = new CultureInfo(language);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            await host.RunAsync();
        }
    }
}
