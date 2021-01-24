using System.Linq;
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
            if (!message.StartsWith('/'))
            {
                return CreateTranslateCommand(message);
            }
            var tokens = message.Split();
            var commandText = tokens.First().ToLower();
            var commandArgs = message.Remove(0, commandText.Length).Trim();
            switch (commandText)
            {
                case _translateCommandName:
                    return CreateTranslateCommand(commandArgs);
                case _samplesCommandName:
                    return CreateSamplesCommand(commandArgs);
                default: return new UnknownCommand(commandText);
            }
        }

        private ICommand CreateSamplesCommand(string commandArgs)
            => new SamplesCommand(_samplesProvider, _languageDetector, _convertor, commandArgs);

        private TranslateCommand CreateTranslateCommand(string message)
            => new TranslateCommand(_languageDetector, _translator, _convertor, message);
    }
}