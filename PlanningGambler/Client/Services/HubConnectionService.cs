using Microsoft.AspNetCore.SignalR.Client;

namespace PlanningGambler.Client.Services;

public class HubConnectionService : IAsyncDisposable
{
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
        RegisterHandlers();
        await _hubConnection.StartAsync();
        _logger.LogInformation("Hub connection started");
    }

    private Task<string?> RetrieveToken()
    {
        return Task.FromResult(_token);
    }

    private void RegisterHandlers()
    {

    }

    public async ValueTask DisposeAsync()
    {
        await _hubConnection.StopAsync();
        await _hubConnection.DisposeAsync();
        _logger.LogInformation("Hub connection closed");
    }
}