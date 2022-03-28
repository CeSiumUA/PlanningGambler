using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PlanningGambler.Hubs;
using PlanningGambler.Models;
using PlanningGambler.Services.Abstract;
using PlanningGambler.Services.Concrete;
using PlanningGambler.TelegramServices.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration(config => config.AddEnvironmentVariables());

builder.Services.AddAuthentication(auth =>
    {
        auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = true;
        options.Events = new JwtBearerEvents()
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/planninghub"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidIssuer = TokenOptions.ValidIssuer,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(TokenOptions.SigningKey)
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.AllowAnyHeader();
        policyBuilder.AllowAnyOrigin();
        policyBuilder.AllowAnyMethod();
    });
});

builder.Services.AddScoped<IRoomsService, RoomsService>();
builder.Services.AddScoped<IRoomManagerService, RoomsService>();
builder.Services.AddSingleton<IRoomStorage, RoomStorageService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddSingleton<TelegramBotService>();

var telegramBotKey = builder.Configuration.GetConnectionString("TelegramBotKey");

builder.Services.AddSwaggerDocument();

var app = builder.Build();

var botService = app.Services.GetRequiredService<TelegramBotService>();
botService.StartListener(telegramBotKey);

app.UseCors(policyBuilder =>
{
    policyBuilder.AllowAnyHeader()
        .AllowAnyMethod()
        .SetIsOriginAllowed((host) => true)
        .AllowCredentials();
});

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
    app.UseWebAssemblyDebugging();
}

app.UseStaticFiles();
app.UseBlazorFrameworkFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<PlanningHub>("/planninghub");

app.UseEndpoints(endpoints =>
{
    endpoints.MapFallbackToFile("index.html");
});

app.Run();