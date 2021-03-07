using System.Threading.Tasks;

namespace MultiTranslator.AzureBot.Services
{
    public interface ITranslator
    {
        Task<string> TranslateAsync(string text, Languages from, Languages to);
    }
}