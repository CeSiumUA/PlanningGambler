using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using PlanningGambler.Client;
using PlanningGambler.Client.Services;
using PlanningGambler.Client.Services.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
    .AddScoped<IRoomEntryProvider, RoomEntryService>()
    .AddScoped<HubConnectionService>()
    .AddMudServices()
    .AddBlazoredLocalStorage();

await builder.Build().RunAsync();
