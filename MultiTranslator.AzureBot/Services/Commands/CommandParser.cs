using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using MultiTranslator.AzureBot.Services.Emoji;
using MultiTranslator.AzureBot.Services.UsageSamples;

namespace MultiTranslator.AzureBot.Services.Commands
{
    public class CommandParser : ICommandParser
    {
        private readonly ILanguageDetector _languageDetector;
        private readonly ITranslator _translator;
        private readonly ILanguageToEmojiConvertor _convertor;
        private readonly IUsageSamplesProvider _samplesProvider;

        public CommandParser(
            ILanguageDetector languageDetector,
            ITranslator translator,
            ILanguageToEmojiConvertor convertor,
            IUsageSamplesProvider samplesProvider)
        {
            _languageDetector = languageDetector;
            _translator = translator;
            _convertor = convertor;
            _samplesProvider = samplesProvider;
        }

        private const string _samplesCommandName = "/samples";
        private const string _translateCommandName = "/translate";

        public ICommand ParseCommand(string message)
        {
            var tokens = message.Split();
            var commandText = tokens.First().ToLower();
            var argsTokens = tokens.Skip(1).ToArray();

            if (!commandText.StartsWith('/'))
            {
                return CreateTranslateCommand(tokens);
            }

            switch (commandText)
            {
                case _translateCommandName:
                    return CreateTranslateCommand(argsTokens);
                case _samplesCommandName:
                    return CreateSamplesCommand(argsTokens);
                default: return new UnknownCommand(commandText);
            }
        }

        private ICommand CreateSamplesCommand(string[] tokens)
        {
            if (tokens.Length == 0)
            {
                return new UnknownCommand("");
            }

            var page = 0;
            var havePageToken = false;
            var pageToken = tokens.First();
            if (pageToken.StartsWith(SamplesCommand.PagePrefix))
            {
                var pageIntStr = pageToken.Remove(0, SamplesCommand.PagePrefix.Length);
                page = int.Parse(pageIntStr);
                havePageToken = true;
            }

            var textTokens = tokens.Skip(havePageToken ? 1 : 0);
            var text = string.Join(" ", textTokens);
            return new SamplesCommand(_samplesProvider, _languageDetector, _convertor, text, page);
        }

        private TranslateCommand CreateTranslateCommand(string[] argsTokens)
        {
            var message = string.Join(" ", argsTokens);
            return new TranslateCommand(_languageDetector, _translator, _convertor, message);
        }
    }
}