using PlanningGambler.Client.Models;
using PlanningGambler.Client.Services.Interfaces;
using PlanningGambler.Shared.Dtos.Request;
using PlanningGambler.Shared.Dtos.Response;
using System.Net.Http.Headers;
using System.Text.Json;

namespace PlanningGambler.Client.Services;

public class RoomEntryService : IRoomEntryProvider
{
    private readonly HttpClient _httpClient;

    public RoomEntryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> CheckTokenValidity(string token)
    {
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "api/room/verify");
        httpRequestMessage.Headers.Authorization = AuthenticationHeaderValue.Parse($"Bearer {token}");
        using var response = await _httpClient.SendAsync(httpRequestMessage);

        if (response.IsSuccessStatusCode)
        {
            await using var tokenVerifyResponseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<TokenValidationResponse>(tokenVerifyResponseStream);
            return result!.IsValid;
        }

        return false;
    }

    public async Task<TokenResponse?> CreateRoom(CreateRoomModel createRoomModel)
    {
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "api/room/create");

        await using var memoryStream = new MemoryStream();
        await JsonSerializer.SerializeAsync(memoryStream, new CreateRoomDto(createRoomModel.DisplayName, createRoomModel.RoomPassword));
        memoryStream.Position = 0;
        using var contentStream = new StreamContent(memoryStream);
        httpRequestMessage.Content = contentStream;
        httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        using var response = await _httpClient.SendAsync(httpRequestMessage);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        await using var responseStream = await response.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<TokenResponse>(responseStream);
    }

    public async Task<TokenResponse?> JoinRoom(JoinRoomModel joinRoomModel)
    {
        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "api/room/join");

        await using var memoryStream = new MemoryStream();
        await JsonSerializer.SerializeAsync(memoryStream, new JoinRoomDto(joinRoomModel.RoomId!.Value, joinRoomModel.DisplayName, joinRoomModel.RoomPassword));
        memoryStream.Position = 0;
        using var contentStream = new StreamContent(memoryStream);
        httpRequestMessage.Content = contentStream;
        httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        using var response = await _httpClient.SendAsync(httpRequestMessage);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        await using var responseStream = await response.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<TokenResponse>(responseStream);
    }
}