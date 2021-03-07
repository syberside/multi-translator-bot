using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using MultiTranslator.AzureBot.Services.Emoji;
using MultiTranslator.AzureBot.Services.Helpers;
using MultiTranslator.AzureBot.Services.UsageSamples;

namespace MultiTranslator.AzureBot.Services.Commands
{
    public class SamplesCommand : ICommand
    {
        private readonly IUsageSamplesProvider _samplesProvider;
        private readonly ILanguageDetector _languageDetector;
        private readonly ILanguageToEmojiConvertor _convertor;

        public string Message { get; }
        public int Page { get; }

        public const string PagePrefix = "page:";

        public const int PageSize = 2;

        public SamplesCommand(IUsageSamplesProvider samplesProvider,
                              ILanguageDetector languageDetector,
                              ILanguageToEmojiConvertor convertor,
                              string message, int page)
        {
            _samplesProvider = samplesProvider;
            _languageDetector = languageDetector;
            Message = message;
            _convertor = convertor;
            Page = page;
        }

        public async Task<IMessageActivity[]> ExecuteAsync()
        {
            var from = await _languageDetector.DetectAsync(Message);
            var to = InvertLanguage(from);

            var samples = await _samplesProvider.GetSamplesAsync(Message, from, to);

            var fromEmoji = _convertor.Convert(from);
            var toEmoji = _convertor.Convert(to);

            var activities = new List<IMessageActivity>();
            var newSamples = samples.Skip(Page * PageSize).Take(PageSize);
            foreach (var sample in newSamples)
            {
                var fromMdString = sample.SourceMd.ToMdString();
                var toMdString = sample.TargetMd.ToMdString();
                var sampleBuilder = new StringBuilder();
                sampleBuilder
                    .AppendLine($"{fromEmoji} {fromMdString}").AppendLine()
                    .AppendLine($"{toEmoji} {toMdString}").AppendLine();
                var sampleText = sampleBuilder.ToString();
                activities.Add(MessageFactory.Text(sampleText, sampleText));
            }

            var lastActivity = activities.LastOrDefault();
            if (lastActivity != null)
            {
                var card = new HeroCard
                {
                    Buttons = new List<CardAction>
                    {
                        new CardAction(ActionTypes.ImBack, title: "More samples", value: $"/samples {PagePrefix}{Page+1} {Message}"),
                    },
                };
                lastActivity.Attachments.Add(card.ToAttachment());
            }

            return activities.ToArray();
        }

        private Languages InvertLanguage(Languages from) => from == Languages.En ? Languages.Ru : Languages.En;
    }
}