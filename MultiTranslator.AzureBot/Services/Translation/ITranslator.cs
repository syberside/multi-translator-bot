using System.Threading.Tasks;

namespace EchoBot.Services
{
    public interface ITranslator
    {
        Task<string> TranslateAsync(string text, Languages from, Languages to);
    }
}