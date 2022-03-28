using System.Text.RegularExpressions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using PlanningGambler.Hubs;
using PlanningGambler.Models.Exceptions;
using PlanningGambler.Services.Abstract;
using PlanningGambler.Shared.Dtos;
using PlanningGambler.Shared.Models;
using PlanningGambler.Telegram.Dtos;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PlanningGambler.TelegramServices.Implementations;

public class TelegramBotService
{
    private TelegramBotClient? _telegramBotClient = null;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<TelegramBotService> _logger;
    public TelegramBotService(ILogger<TelegramBotService> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public void StartListener(string botToken)
    {
        _telegramBotClient = new TelegramBotClient(botToken);
        _telegramBotClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync);
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Got message");
        if (update.Type != UpdateType.Message || string.IsNullOrEmpty(update.Message?.Text))
        {
            return;
        }

        var messageText = update.Message.Text;
        if (messageText == "/start")
        {
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, GetGreetings(), cancellationToken: cancellationToken);
        }
        else if (messageText.StartsWith("/join"))
        {
            var regex = new Regex(
                @"(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}");
            var roomId = regex.Match(messageText).Value;
            var name = $"{update.Message?.Chat.FirstName} {update.Message?.Chat.LastName}";
            var user = new TelegramUser(update.Message!.Chat.Id,
                name,
                Guid.Parse(roomId));
            try
            {
                await NotifyUserJoined(user);
            }
            catch (RoomNotFoundException e)
            {
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, e.Message, cancellationToken: cancellationToken);
                return;
            }
        }
    }

    private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
    }

    private async Task NotifyUserJoined(TelegramUser user)
    {
        var participantDto = new ParticipantDto(user.ChatId.ToString(), user.UserName, MemberType.Participant);
        List<ParticipantDto> newParticipantsList = new List<ParticipantDto>();
        _logger.LogInformation("Notifying user joined");
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var roomManagerService = scope.ServiceProvider.GetService<IRoomManagerService>()!;

            var participant = new PlanningParticipant(user.ChatId.ToString(), user.UserName, MemberType.Participant,
                user.RoomId, ClientType.Telegram);
            await roomManagerService.AddParticipantToRoom(participant);
            _logger.LogInformation("Participant added to room");
            newParticipantsList = roomManagerService.GetRoom(user.RoomId).Participants
                .Select(x => new ParticipantDto(x.Id, x.DisplayName, x.MemberType)).ToList();
        }
        _logger.LogInformation("New participants count: {0}", newParticipantsList.Count);
        var participantsChanged =
                new ParticipantsChangedDto(participantDto, newParticipantsList);
        await NotifyClientsAsync(user.RoomId.ToString(), "ParticipantConnected", participantsChanged);
    }
    
    private async Task NotifyClientsAsync(string roomId, string methodName, object parameter)
    {
        try
        {
            _logger.LogInformation("Notifying clients");
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var hub = scope.ServiceProvider.GetService<IHubContext<PlanningHub>>();
                await hub!.Clients.Group(roomId).SendAsync(methodName, parameter);
            }
            _logger.LogInformation("Notifications sent!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private string GetGreetings()
    {
        return $"Welcome to Planning Gambler Bot!{Environment.NewLine}" +
               $"Here are some commands:{Environment.NewLine}" +
               $"/join <room id or invite url> - join room{Environment.NewLine}" +
               $"";
    }
}