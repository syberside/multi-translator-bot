using System.Threading.Tasks;

namespace EchoBot.Services
{
    public interface ILanguageDetector
    {
        public Task<Languages> DetectAsync(string text);
    }
}