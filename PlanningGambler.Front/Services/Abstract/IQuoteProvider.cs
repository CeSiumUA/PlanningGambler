namespace PlanningGambler.Front.Services.Abstract
{
    public interface IQuoteProvider
    {
        public Task<string> GetQuoteOfTheDay();
    }
}
