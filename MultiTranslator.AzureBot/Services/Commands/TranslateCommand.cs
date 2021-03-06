using System.Collections.Generic;
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
        public string Message { get; }

        public TranslateCommand(ILanguageDetector languageDetector, ITranslator translator, ILanguageToEmojiConvertor convertor, string message)
        {
            _translator = translator;
            _convertor = convertor;
            _languageDetector = languageDetector;
            Message = message;
        }

        public async Task<IMessageActivity[]> ExecuteAsync()
        {
            var from = await _languageDetector.DetectAsync(Message);
            var to = InvertLanguage(from);

            var translation = await _translator.TranslateAsync(Message, from, to);

            var fromEmoji = _convertor.Convert(from);
            var toEmoji = _convertor.Convert(to);

            var directTranslationBuilder = new StringBuilder();
            directTranslationBuilder
                .AppendLine($"{fromEmoji} {Message}").AppendLine()
                .AppendLine($"{toEmoji} {translation}").AppendLine();
            var directTranslation = directTranslationBuilder.ToString();

            var card = new HeroCard
            {
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.ImBack, title: "Usage samples", value: $"/samples {Message}"),
                },
            };

            var reply = MessageFactory.Attachment(card.ToAttachment(), text: directTranslation);
            return new[] { reply };
        }

        private Languages InvertLanguage(Languages from) => from == Languages.En ? Languages.Ru : Languages.En;
    }
}