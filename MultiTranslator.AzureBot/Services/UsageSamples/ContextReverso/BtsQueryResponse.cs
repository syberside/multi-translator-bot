using Newtonsoft.Json;

namespace EchoBot.Services.UsageSamples.ContextReverso
{
    public class BtsQueryResponse
    {
        [JsonProperty("list")]
        public BtsQueryListItem[] Items { get; set; }
    }
}