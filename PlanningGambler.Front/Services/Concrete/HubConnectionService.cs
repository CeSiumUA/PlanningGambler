using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using PlanningGambler.Dtos.Results;
using PlanningGambler.Front.Options;
using PlanningGambler.Shared.Dtos;
using PlanningGambler.Shared.Dtos.Requests;
using PlanningGambler.Shared.Dtos.Results;

namespace PlanningGambler.Front.Services.Concrete
{
    public class HubConnectionService : IAsyncDisposable
    {
        private readonly HubConnection _hubConnection;

        private string? _token;

        public event EventHandler<NewStageResult?> OnStageCreated;

        public event EventHandler<Guid> OnStageSelected;

        public event EventHandler<int> OnCountDown;

        public event EventHandler<VotingResult[]?> OnStageVotingResult;

        public event EventHandler<HiddenVotingResult?> OnParticipantVoted;

        public event EventHandler<ParticipantsChangedDto> OnParticipantConnected;

        public event EventHandler<ParticipantsChangedDto> OnParticipantDisconnected;

        private readonly ILogger<HubConnectionService> _logger;

        public HubConnectionService(ILogger<HubConnectionService> logger, IConfiguration configuration)
        {
            this._logger = logger;
            this._hubConnection = new HubConnectionBuilder()
                .WithUrl($"{configuration["ApiOptions:ApiUrl"]}/planninghub", options =>
                {
                    options.AccessTokenProvider = async () => await this.RetrieveToken();
                })
                .Build();
        }

        public async Task CreateStage(string stageTitle, DateTimeOffset? deadline = null)
        {
            var createStageRequest = new CreateStageRequest(stageTitle, deadline);
            await this._hubConnection.SendAsync("CreateStage", createStageRequest);
        }

        public async Task SelectStage(Guid stageId)
        {
            await this._hubConnection.SendAsync("SelectStage", stageId);
        }

        public async Task StartCountDown()
        {
            await this._hubConnection.SendAsync("StartCountDown");
        }

        public async Task<RoomDto?> FetchRoom()
        {
            return await this._hubConnection.InvokeAsync<RoomDto?>("FetchRoom");
        }

        public async Task Vote(int vote)
        {
            await this._hubConnection.InvokeAsync("Vote", vote);
        }

        public async ValueTask DisposeAsync()
        {
            await this._hubConnection.StopAsync();
            await this._hubConnection.DisposeAsync();
        }

        public async Task StartConnection(string token)
        {
            this._token = token;
            this.RegisterHandlers();
            await this._hubConnection.StartAsync();
        }

        private Task<string?> RetrieveToken()
        {
            return Task.FromResult(this._token);
        }

        private void RegisterHandlers()
        {
            this._logger.LogInformation("Registering handlers");
            this._hubConnection.On<NewStageResult?>("StageCreated", x =>
            {
                this.OnStageCreated?.Invoke(this, x);
            });

            this._hubConnection.On<Guid>("StageSelected", x =>
            {
                this.OnStageSelected?.Invoke(this, x);
            });

            this._hubConnection.On<int>("CountDown", x =>
            {
                this.OnCountDown?.Invoke(this, x);
            });

            this._hubConnection.On<VotingResult[]?>("StageVotingResult", x =>
            {
                this.OnStageVotingResult?.Invoke(this, x);
            });

            this._hubConnection.On<HiddenVotingResult?>("ParticipantVoted", x =>
            {
                this.OnParticipantVoted?.Invoke(this, x);
            });

            this._hubConnection.On<ParticipantsChangedDto>("ParticipantConnected", x =>
            {
                this._logger.LogInformation("Participant connected");
                this.OnParticipantConnected?.Invoke(this, x);
            });

            this._hubConnection.On<ParticipantsChangedDto>("ParticipantDisconnected", x =>
            {
                this.OnParticipantDisconnected?.Invoke(this, x);
            });
        }
    }
}
