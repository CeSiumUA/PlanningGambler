using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PlanningGambler.Front.Options;
using PlanningGambler.Front.Services.Abstract;
using PlanningGambler.Shared.Dtos;
using PlanningGambler.Shared.Dtos.Requests;

namespace PlanningGambler.Front.Services.Concrete
{
    public class RoomConnectionService : IRoomConnectionProvider
    {
        private readonly HttpClient _httpClient;

        public RoomConnectionService(HttpClient httpClient, IConfiguration configuration)
        {
            this._httpClient = httpClient;
            this._httpClient.BaseAddress = new Uri(configuration["ApiOptions:ApiUrl"]);
        }

        public async Task<RoomToken?> CreateRoom(string displayName, string? password)
        {
            var createRoomRequest = new BaseRoomRequest(displayName, password);
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/rooms/create");
            await using var memoryStream = new MemoryStream();
            await JsonSerializer.SerializeAsync(memoryStream, createRoomRequest);
            memoryStream.Position = 0;
            using var contentStream = new StreamContent(memoryStream);
            httpRequestMessage.Content = contentStream;
            httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            using var response = await this._httpClient.SendAsync(httpRequestMessage);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<RoomToken>(responseStream);
        }

        public async Task<RoomToken?> JoinRoom(Guid roomId, string displayName, string? password = null)
        {
            var joinRoomRequest = new JoinRoomRequest(displayName, password, roomId);
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/rooms/join");
            await using var memoryStream = new MemoryStream();
            await JsonSerializer.SerializeAsync(memoryStream, joinRoomRequest);
            memoryStream.Position = 0;
            using var contentStream = new StreamContent(memoryStream);
            httpRequestMessage.Content = contentStream;
            httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            using var response = await this._httpClient.SendAsync(httpRequestMessage);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<RoomToken>(responseStream);
        }
    }
}
