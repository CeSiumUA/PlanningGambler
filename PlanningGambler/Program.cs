using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PlanningGambler.Hubs;
using PlanningGambler.Models;
using PlanningGambler.Services.Abstract;
using PlanningGambler.Services.Concrete;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
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

var app = builder.Build();

app.UseCors(policyBuilder =>
{
    policyBuilder.AllowAnyHeader()
        .AllowAnyMethod()
        .SetIsOriginAllowed((host) => true)
        .AllowCredentials();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<PlanningHub>("/planninghub");

app.Run();