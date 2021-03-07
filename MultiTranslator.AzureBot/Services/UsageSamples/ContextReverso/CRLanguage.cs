using System.Runtime.Serialization;

namespace MultiTranslator.AzureBot.Services.UsageSamples.ContextReverso
{
    public enum CRLanguage
    {
        [EnumMember(Value = "ru")]
        Ru,
        [EnumMember(Value = "en")]
        Eng,
    }
}