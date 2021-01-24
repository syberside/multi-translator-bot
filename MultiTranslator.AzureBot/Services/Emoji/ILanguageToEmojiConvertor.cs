namespace MultiTranslator.AzureBot.Services.Emoji
{
    public interface ILanguageToEmojiConvertor
    {
        string Convert(Languages language);
    }
}