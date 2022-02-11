using Microsoft.AspNetCore.SignalR.Client;

namespace PlanningGambler.Front.Services.Concrete
{
    public class HubConnectionService : IAsyncDisposable
    {
        private readonly HubConnection _hubConnection;

        private string? _token;

        public HubConnectionService()
        {
            this._hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7049/planninghub", options =>
                {
                    options.AccessTokenProvider = async () => await this.RetreiveToken();
                })
                .Build();
        }

        public async ValueTask DisposeAsync()
        {
            await this._hubConnection?.StopAsync();
            await this._hubConnection.DisposeAsync();
        }

        public async Task StartConnection(string token)
        {
            this._token = token;
            await this._hubConnection.StartAsync();
        }

        private async Task<string> RetreiveToken()
        {
            return this._token;
        }
    }
}
