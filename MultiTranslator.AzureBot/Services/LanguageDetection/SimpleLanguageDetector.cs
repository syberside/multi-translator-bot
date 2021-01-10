using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EchoBot.Services
{

    public class SimpleLanguageDetector : ILanguageDetector
    {
        private readonly Regex _languageRegexp = new Regex("^[a-zA-Z0-9]*$");
        public Task<Languages> DetectAsync(string text)
        {
            var isEng = _languageRegexp.IsMatch(text);
            var result = isEng ? Languages.En : Languages.Ru;
            return Task.FromResult(result);
        }
    }
}