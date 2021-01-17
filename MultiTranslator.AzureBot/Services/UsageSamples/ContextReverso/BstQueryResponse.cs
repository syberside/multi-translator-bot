using Newtonsoft.Json;

namespace EchoBot.Services.UsageSamples.ContextReverso
{
    public class BstQueryResponse
    {
        [JsonProperty("list")]
        public BstQueryListItem[] Items { get; set; }
    }
}