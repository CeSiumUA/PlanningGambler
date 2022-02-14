using Blazored.LocalStorage;
using MatBlazor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PlanningGambler.Front;
using PlanningGambler.Front.Options;
using PlanningGambler.Front.Services.Abstract;
using PlanningGambler.Front.Services.Concrete;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IRoomConnectionProvider, RoomConnectionService>();
builder.Services.AddScoped<HubConnectionService>();
builder.Services.AddMatBlazor();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
