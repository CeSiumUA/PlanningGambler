using Microsoft.AspNetCore.SignalR.Client;
using PlanningGambler.Shared.Data;
using PlanningGambler.Shared.Dtos.Response;

namespace PlanningGambler.Client.Services;

public class HubConnectionService : IAsyncDisposable
{
    public event EventHandler<SelectStageResponseDto>? StageSelected;
    public event EventHandler<StageDto>? StageCreated;
    public event EventHandler<MemberConnectedResponseDto>? MemberConnected;
    public event EventHandler<MemberDisconnectedResponseDto>? MemberDisconnected;
    public event EventHandler<int>? CountDownOccured;
    public event EventHandler<StageVotesResultDto>? VotesStated;
    public event EventHandler<VoteDto>? VoteOccured;

    private readonly HubConnection _hubConnection;
    private readonly HttpClient _httpClient;
    private readonly ILogger<HubConnectionService> _logger;
    private string? _token;

    public HubConnectionService(HttpClient httpClient, ILogger<HubConnectionService> logger)
    {
        _httpClient= httpClient;
        _logger= logger;

        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{_httpClient.BaseAddress}planninghub",
                options =>
                {
                    options.AccessTokenProvider = async () => await RetrieveToken();
                })
            .WithAutomaticReconnect()
            .Build();
    }

    public async Task StartConnectionAsync(string token)
    {
        _token = token;
        if (_hubConnection.State == HubConnectionState.Disconnected)
        {
            RegisterHandlers();
            await _hubConnection.StartAsync();
            _logger.LogInformation("Hub connection started");
        }
    }

    public Task<RoomDto> GetRoom()
    {
        return _hubConnection.InvokeAsync<RoomDto>("GetRoom");
    }

    public Task<VoteDto> Vote(VoteType vote)
    {
        return _hubConnection.InvokeAsync<VoteDto>("Vote", vote);
    }

    public Task<StageDto> CreateStage(string stageName)
    {
        return _hubConnection.InvokeAsync<StageDto>("CreateStage", stageName);
    }

    public Task<SelectStageResponseDto> SelectStage(Guid stageId)
    {
        return _hubConnection.InvokeAsync<SelectStageResponseDto>("SelectStage", stageId);
    }

    public Task StartCountdown()
    {
        return _hubConnection.SendAsync("StartCountDown");
    }

    private Task<string?> RetrieveToken()
    {
        return Task.FromResult(_token);
    }

    private void RegisterHandlers()
    {
        _hubConnection.On<SelectStageResponseDto>(HubConstants.StageChangedMethod, x => StageSelected?.Invoke(this, x));

        _hubConnection.On<StageDto>(HubConstants.StageCreatedMethod, x => StageCreated?.Invoke(this, x));

        _hubConnection.On<MemberConnectedResponseDto>(HubConstants.MemberConnectedMethod, x => MemberConnected?.Invoke(this, x));

        _hubConnection.On<MemberDisconnectedResponseDto>(HubConstants.MemberDisconnectedMethod, x => MemberDisconnected?.Invoke(this, x));

        _hubConnection.On<int>(HubConstants.CountDownMethod, x => CountDownOccured?.Invoke(this, x));

        _hubConnection.On<StageVotesResultDto>(HubConstants.VoteResultsMethod, x => VotesStated?.Invoke(this, x));

        _hubConnection.On<VoteDto>(HubConstants.MemberVoted, x => VoteOccured?.Invoke(this, x));
    }

    public async ValueTask DisposeAsync()
    {
        await _hubConnection.StopAsync();
        await _hubConnection.DisposeAsync();
        _logger.LogInformation("Hub connection closed");
    }
}