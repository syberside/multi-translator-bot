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

        public SamplesCommand(IUsageSamplesProvider samplesProvider,
                              ILanguageDetector languageDetector,
                              ILanguageToEmojiConvertor convertor,
                              string message)
        {
            _samplesProvider = samplesProvider;
            _languageDetector = languageDetector;
            Message = message;
            _convertor = convertor;
        }

        public async Task<Activity[]> ExecuteAsync()
        {
            var from = await _languageDetector.DetectAsync(Message);
            var to = InvertLanguage(from);

            var samples = await _samplesProvider.GetSamplesAsync(Message, from, to);

            var fromEmoji = _convertor.Convert(from);
            var toEmoji = _convertor.Convert(to);

            var activities = new List<Activity>();
            foreach (var sample in samples.Take(2))
            {
                var fromMdString = sample.SourceMd.ToMdString();
                var toMdString = sample.TargetMd.ToMdString();
                var sampleBuilder = new StringBuilder();
                sampleBuilder
                    .AppendLine($"{fromEmoji} {fromMdString}").AppendLine()
                    .AppendLine($"{toEmoji} {toMdString}").AppendLine();
                var sampleText = sampleBuilder.ToString();
                var sampleAction = MessageFactory.Text(sampleText, sampleText);
                activities.Add(sampleAction);
            }
            return activities.ToArray();
        }

        private Languages InvertLanguage(Languages from) => from == Languages.En ? Languages.Ru : Languages.En;
    }
}