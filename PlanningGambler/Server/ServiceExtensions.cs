using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using PlanningGambler.Server.Commands;
using PlanningGambler.Server.Models;
using PlanningGambler.Server.Services;
using PlanningGambler.Server.Services.Interfaces;

namespace PlanningGambler.Server;

public static class ServiceExtensions
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
    {
        var assembly = typeof(CreateRoomCommand).GetType().Assembly;

        return services
            .AddMediatR(typeof(Room).Assembly)
            .AddSingleton<IRoomStorage, RoomStorageService>()
            .AddSingleton<ITokenKeyProvider, TokenKeyService>()
            .AddMediatR(assembly)
            .AddFluentValidation(new[] { assembly });
    }
}