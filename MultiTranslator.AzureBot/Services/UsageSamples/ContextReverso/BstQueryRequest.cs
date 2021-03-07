using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace MultiTranslator.AzureBot.Services.UsageSamples.ContextReverso
{
    public class BstQueryRequest
    {
        [JsonProperty("source_lang")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CRLanguage SourceLang { get; set; }

        [JsonProperty("target_lang")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CRLanguage TargetLang { get; set; }

        [JsonProperty("mode")]
        public int Mode { get; set; }

        [JsonProperty("npage")]
        public int Page { get; set; }

        [JsonProperty("source_text")]
        public string SourceText { get; set; }

        [JsonProperty("target_text")]
        public string TargetText { get; set; }
    }

}