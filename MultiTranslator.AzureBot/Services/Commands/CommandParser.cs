using MultiTranslator.AzureBot.Services.Emoji;

namespace MultiTranslator.AzureBot.Services.Commands
{
    public class CommandParser : ICommandParser
    {
        private readonly ILanguageDetector _languageDetector;
        private readonly ITranslator _translator;
        private readonly ILanguageToEmojiConvertor _convertor;

        public CommandParser(ILanguageDetector languageDetector, ITranslator translator, ILanguageToEmojiConvertor convertor)
        {
            _languageDetector = languageDetector;
            _translator = translator;
            _convertor = convertor;
        }

        public ICommand ParseCommand(string message)
        {
            return CreateDefaultCommand(message);
        }

        private ICommand CreateDefaultCommand(string message)
        {
            return new TranslateCommand(_languageDetector, _translator, _convertor, message);
        }
    }
}