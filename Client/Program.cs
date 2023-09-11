using Client;
using Client.Shared.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri (builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddSingleton<InMemoryDataBaseCache>();

await builder.Build().RunAsync();
