using System.Threading.Tasks;

namespace MultiTranslator.AzureBot.Services
{
    public interface ILanguageDetector
    {
        public Task<Languages> DetectAsync(string text);
    }
}