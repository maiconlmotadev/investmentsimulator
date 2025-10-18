using InvestmentSimulator.Client.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");

// Configure HttpClient to use the API's base address
builder.Services.AddHttpClient("InvestmentSimulator.API", client =>
{
    client.BaseAddress = new Uri("http://localhost:5284");
});

// Make the configured HttpClient available for injection
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("InvestmentSimulator.API"));

await builder.Build().RunAsync();
