using System.Net.Http.Json;
using PlanningGambler.Front.Models;
using PlanningGambler.Front.Services.Abstract;

namespace PlanningGambler.Front.Services.Concrete;

public class QuoteProvider : IQuoteProvider
{
    private static List<string> _quotes = new List<string>()
    {
        "Be yourself; everyone else is already taken",
        "I'm selfish, impatient and a little insecure. I make mistakes, I am out of control and at times hard to handle. But if you can't handle me at my worst, then you sure as hell don't deserve me at my best",
        "Two things are infinite: the universe and human stupidity; and I'm not sure about the universe",
        "So many books, so little time",
        "A room without books is like a body without a soul",
        "Be who you are and say what you feel, because those who mind don't matter, and those who matter don't mind",
        "You've gotta dance like there's nobody watching,\r\nLove like you'll never be hurt,\r\nSing like there's nobody listening,\r\nAnd live like it's heaven on earth",
        "You know you're in love when you can't fall asleep because reality is finally better than your dreams",
        "You only live once, but if you do it right, once is enough",
        "Be the change that you wish to see in the world",
        "In three words I can sum up everything I've learned about life: it goes on",
        "If you want to know what a man's like, take a good look at how he treats his inferiors, not his equals",
        "Don’t walk in front of me… I may not follow\r\nDon’t walk behind me… I may not lead\r\nWalk beside me… just be my friend",
        "If you tell the truth, you don't have to remember anything",
        "Friendship ... is born at the moment when one man says to another \"What! You too? I thought that no one but myself . . .",
        "I've learned that people will forget what you said, people will forget what you did, but people will never forget how you made them feel",
        "A friend is someone who knows all about you and still loves you",
        "To live is the rarest thing in the world. Most people exist, that is all",
        "Always forgive your enemies; nothing annoys them so much",
        "Live as if you were to die tomorrow. Learn as if you were to live forever",
        "Darkness cannot drive out darkness: only light can do that. Hate cannot drive out hate: only love can do that",
        "We accept the love we think we deserve",
        "Without music, life would be a mistake",
        "I am so clever that sometimes I don't understand a single word of what I am saying",
        "To be yourself in a world that is constantly trying to make you something else is the greatest accomplishment",
        "I believe that everything happens for a reason. People change so that you can learn to let go, things go wrong so that you appreciate them when they're right, you believe lies so you eventually learn to trust no one but yourself, and sometimes good things fall apart so better things can fall together",
        "It is better to be hated for what you are than to be loved for what you are not",
        "Twenty years from now you will be more disappointed by the things that you didn't do than by the ones you did do. So throw off the bowlines. Sail away from the safe harbor. Catch the trade winds in your sails. Explore. Dream. Discover"
    };

    private readonly Random _random;

    public QuoteProvider()
    {
        _random = new Random();
    }

    public async Task<string> GetQuoteOfTheDay()
    {
        if (_quotes.Any())
        {
            return _quotes[_random.Next(0, _quotes.Count)];
        }
        return string.Empty;
    }
}