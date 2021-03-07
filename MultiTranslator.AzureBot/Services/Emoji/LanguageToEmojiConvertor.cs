using System;

namespace MultiTranslator.AzureBot.Services.Emoji
{
    public class LanguageToEmojiConvertor : ILanguageToEmojiConvertor
    {
        public string Convert(Languages language)
        {
            switch (language)
            {
                case Languages.En: return "\U0001F1FA\U0001F1F8";
                case Languages.Ru: return "\U0001F1F7\U0001F1FA";
                default: throw new NotSupportedException();
            }
        }
    }
}