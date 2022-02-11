using Blazored.LocalStorage;
using MatBlazor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PlanningGambler.Front;
using PlanningGambler.Front.Services.Abstract;
using PlanningGambler.Front.Services.Concrete;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7049") });
builder.Services.AddScoped<IRoomConnectionProvider, RoomConnectionService>();
builder.Services.AddMatBlazor();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
