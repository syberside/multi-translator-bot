using System;
using System.Threading.Tasks;
using Google.Cloud.Translation.V2;

namespace EchoBot.Services
{
    public class GoogleTranslateFacade : ITranslator
    {
        private readonly TranslationClient _translationClient;
        public GoogleTranslateFacade(TranslationClient translationClient)
        {
            _translationClient = translationClient;
        }

        public async Task<string> TranslateAsync(string text, Languages from, Languages to)
        {
            var sourceLanguage = MapLanguage(from);
            var targetLanguage = MapLanguage(to);
            var response = await _translationClient.TranslateTextAsync(text, sourceLanguage: sourceLanguage, targetLanguage: targetLanguage);
            return response.TranslatedText;
        }

        private string MapLanguage(Languages from)
        {
            switch (from)
            {
                case Languages.En: return LanguageCodes.English;
                case Languages.Ru: return LanguageCodes.Russian;
                default: throw new NotSupportedException();
            }
        }
    }
}