using System.Runtime.Serialization;

namespace EchoBot.Services.UsageSamples.ContextReverso
{
    public enum CRLanguage
    {
        [EnumMember(Value = "ru")]
        Ru,
        [EnumMember(Value = "en")]
        Eng,
    }
}