using Microsoft.AspNetCore.SignalR.Client;
using PlanningGambler.Shared.Dtos;
using PlanningGambler.Shared.Dtos.Requests;
using PlanningGambler.Shared.Dtos.Results;

namespace PlanningGambler.Front.Services.Concrete;

public class HubConnectionService : IAsyncDisposable
{
    private readonly HubConnection _hubConnection;

    private readonly ILogger<HubConnectionService>? _logger;

    private string? _token;

    public HubConnectionService(ILogger<HubConnectionService> logger, HttpClient httpClient)
    {
        _logger = logger;
        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{httpClient.BaseAddress}planninghub",
                options => { options.AccessTokenProvider = async () => await RetrieveToken(); })
            .WithAutomaticReconnect()
            .Build();
    }

    public async ValueTask DisposeAsync()
    {
        await _hubConnection.StopAsync();
        await _hubConnection.DisposeAsync();
    }

    public event EventHandler<NewStageResult?>? OnStageCreated;

    public event EventHandler<Guid>? OnStageSelected;

    public event EventHandler<int>? OnCountDown;

    public event EventHandler<VotingResult[]?>? OnStageVotingResult;

    public event EventHandler<HiddenVotingResult?>? OnParticipantVoted;

    public event EventHandler<ParticipantsChangedDto>? OnParticipantConnected;

    public event EventHandler<ParticipantsChangedDto>? OnParticipantDisconnected;

    public async Task CreateStage(string stageTitle, DateTimeOffset? deadline = null)
    {
        var createStageRequest = new CreateStageRequest(stageTitle, deadline);
        await _hubConnection.SendAsync("CreateStage", createStageRequest);
    }

    public async Task SelectStage(Guid stageId)
    {
        await _hubConnection.SendAsync("SelectStage", stageId);
    }

    public async Task StartCountDown()
    {
        await _hubConnection.SendAsync("StartCountDown");
    }

    public async Task<RoomDto?> FetchRoom()
    {
        return await _hubConnection.InvokeAsync<RoomDto?>("FetchRoom");
    }

    public async Task Vote(string vote)
    {
        await _hubConnection.InvokeAsync("Vote", vote);
    }

    public async Task StartConnection(string token)
    {
        _token = token;
        RegisterHandlers();
        await _hubConnection.StartAsync();
    }

    private Task<string?> RetrieveToken()
    {
        return Task.FromResult(_token);
    }

    private void RegisterHandlers()
    {
        _logger.LogInformation("Registering handlers");
        _hubConnection.On<NewStageResult?>("StageCreated", x => { OnStageCreated?.Invoke(this, x); });

        _hubConnection.On<Guid>("StageSelected", x => { OnStageSelected?.Invoke(this, x); });

        _hubConnection.On<int>("CountDown", x => { OnCountDown?.Invoke(this, x); });

        _hubConnection.On<VotingResult[]?>("StageVotingResult", x => { OnStageVotingResult?.Invoke(this, x); });

        _hubConnection.On<HiddenVotingResult?>("ParticipantVoted", x => { OnParticipantVoted?.Invoke(this, x); });

        _hubConnection.On<ParticipantsChangedDto>("ParticipantConnected", x =>
        {
            _logger.LogInformation("Participant connected");
            OnParticipantConnected?.Invoke(this, x);
        });

        _hubConnection.On<ParticipantsChangedDto>("ParticipantDisconnected",
            x => { OnParticipantDisconnected?.Invoke(this, x); });
    }
}