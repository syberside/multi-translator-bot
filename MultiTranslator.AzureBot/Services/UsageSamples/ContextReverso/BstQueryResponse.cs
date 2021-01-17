using Newtonsoft.Json;

namespace MultiTranslator.AzureBot.Services.UsageSamples.ContextReverso
{
    public class BstQueryResponse
    {
        [JsonProperty("list")]
        public BstQueryListItem[] Items { get; set; }
    }
}