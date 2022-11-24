using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PlanningGambler.Server.Commands;
using PlanningGambler.Server.Models;
using PlanningGambler.Server.Services;
using PlanningGambler.Server.Services.Interfaces;
using System.Diagnostics;

namespace PlanningGambler.Server;

public static class ServiceExtensions
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
    {
        var assembly = typeof(CreateRoomCommand).GetType().Assembly;

        services.AddSignalR();

        var tokenService = new TokenKeyService();

        return services
            .AddMediatR(typeof(Room).Assembly)
            .AddSingleton<IRoomStorage, RoomStorageService>()
            .AddSingleton<ITokenKeyProvider>(tokenService)
            .AddSingleton<ISessionStorage, SessionStorageService>()
            .AddConfiguredAuthentication(tokenService)
            .AddMediatR(assembly)
            .AddFluentValidation(new[] { assembly })
            .AddAuthorization();
    }

    public static IServiceCollection AddConfiguredAuthentication(this IServiceCollection services, ITokenKeyProvider tokenKeyProvider)
    {
        services
            .AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/planninghub"))
                            context.Token = accessToken;
                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = tokenKeyProvider.GetIssuer(),
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(tokenKeyProvider.GetKey())
                };
            });

        return services;
    }
}