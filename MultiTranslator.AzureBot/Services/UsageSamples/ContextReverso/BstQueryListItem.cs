using Newtonsoft.Json;

namespace EchoBot.Services.UsageSamples.ContextReverso
{
    public class BstQueryListItem
    {
        [JsonProperty("s_text")]
        public string SourceText { get; set; }

        [JsonProperty("t_text")]
        public string TargetText { get; set; }
    }
}