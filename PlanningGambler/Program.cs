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

builder.Services.AddScoped<IRoomsService, RoomsService>();
builder.Services.AddSingleton<IRoomStorage, RoomStorageService>();
builder.Services.AddScoped<TokenService>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<PlanningHub>("/planninghub");

app.Run();