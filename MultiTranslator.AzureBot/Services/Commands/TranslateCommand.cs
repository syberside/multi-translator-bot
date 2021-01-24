using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using MultiTranslator.AzureBot.Services.Emoji;

namespace MultiTranslator.AzureBot.Services.Commands
{
    public class TranslateCommand : ICommand
    {
        private readonly ILanguageDetector _languageDetector;
        private readonly ITranslator _translator;
        private readonly ILanguageToEmojiConvertor _convertor;
        private readonly string _message;

        public TranslateCommand(ILanguageDetector languageDetector, ITranslator translator, ILanguageToEmojiConvertor convertor, string message)
        {
            _translator = translator;
            _convertor = convertor;
            _message = message;
            _languageDetector = languageDetector;
        }

        public async Task<Activity[]> ExecuteAsync()
        {
            var from = await _languageDetector.DetectAsync(_message);
            var to = InvertLanguage(from);
            var translation = await _translator.TranslateAsync(_message, from, to);


            var fromEmoji = _convertor.Convert(from);
            var toEmoji = _convertor.Convert(to);

            var directTranslationBuilder = new StringBuilder();
            directTranslationBuilder
                .AppendLine($"{fromEmoji} {_message}").AppendLine()
                .AppendLine($"{toEmoji} {translation}").AppendLine();
            var directTranslation = directTranslationBuilder.ToString();
            var directTranslationActivity = MessageFactory.Text(directTranslation, directTranslation);

            // directTranslationActivity.SuggestedActions = new SuggestedActions
            // {
            //     Actions = new List<CardAction>()
            //     {
            //         new CardAction {
            //             Title = "Samples",
            //             Value = $"/samples {message}"
            //         }
            //     }
            // };
            // activities.Add(directTranslationActivity);

            // foreach (var sample in samples.Take(2))
            // {
            //     var sampleBuilder = new StringBuilder();
            //     sampleBuilder
            //         .AppendLine($"{fromEmoji} {sample.SourceMd.ToMdString()}").AppendLine()
            //         .AppendLine($"{toEmoji} {sample.TargetMd.ToMdString()}").AppendLine();
            //     activities.Add(CreateMessageActivity(sampleBuilder.ToString()));
            // }
            return new[] { directTranslationActivity, };
        }

        private Languages InvertLanguage(Languages from) => from == Languages.En ? Languages.Ru : Languages.En;
    }
}