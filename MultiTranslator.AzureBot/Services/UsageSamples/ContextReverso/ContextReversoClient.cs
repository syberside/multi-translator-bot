using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EchoBot.Services.UsageSamples.ContextReverso
{
    public class ContextReversoClient : IContextReversoClient
    {
        private readonly HttpClient _httpClient;
        private const string _btsQueryEndpoint = "https://contex.reverso.net/bts-query-service";

        public ContextReversoClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HtmlUsageSample[]> GetSamplesAsync(string text, CRLanguage from, CRLanguage to)
        {
            var request = new ContextReversoBtsQueryRequest
            {
                Mode = 0,
                Page = 1,
                SourceLang = from,
                TargetLang = to,
                SourceText = text,
            };
            var requestJson = JsonConvert.SerializeObject(request);
            var content = new StringContent(requestJson);

            var httpAnswer = await _httpClient.PostAsync(_btsQueryEndpoint, content);

            var answerContent = await httpAnswer.Content.ReadAsStringAsync();
            if (httpAnswer.StatusCode != HttpStatusCode.OK)
            {
                var message = $"ContextReverso bad http response: {httpAnswer.StatusCode}, {answerContent}";
                throw new Exception(message);
            }

            var deserialized = JsonConvert.DeserializeObject<BtsQueryResponse>(answerContent);
            return deserialized.Items.Select(x => new HtmlUsageSample
            {
                SourceHtml = x.SourceText,
                TargetHtml = x.TargetText,
            }).ToArray();
        }
    }
}