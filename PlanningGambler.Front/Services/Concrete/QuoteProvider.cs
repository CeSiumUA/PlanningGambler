using System.Net.Http.Json;
using PlanningGambler.Front.Models;
using PlanningGambler.Front.Services.Abstract;

namespace PlanningGambler.Front.Services.Concrete
{
    public class QuoteProvider : IQuoteProvider
    {
        private readonly HttpClient _httpClient;
        public QuoteProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> GetQuoteOfTheDay()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<QuoteResponseModel>("qod?language=en");
                return response!.Contents.Quotes.First().Quote;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
