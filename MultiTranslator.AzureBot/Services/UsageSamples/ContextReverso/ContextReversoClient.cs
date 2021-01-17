using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EchoBot.Services.UsageSamples.ContextReverso
{
    public class ContextReversoClient : IContextReversoClient
    {
        private readonly HttpClient _httpClient;
        private const string _bstQueryEndpoint = "https://context.reverso.net/bst-query-service";

        public ContextReversoClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HtmlUsageSample[]> GetSamplesAsync(string text, CRLanguage from, CRLanguage to)
        {
            var request = new ContextReversoBstQueryRequest
            {
                Mode = 0,
                Page = 1,
                SourceLang = from,
                TargetLang = to,
                SourceText = text,
                TargetText = "",
            };

            var requestJson = JsonConvert.SerializeObject(request);
            var stringContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
            var content = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_bstQueryEndpoint),
                Headers =
                {
                    {"Accept",          "application/json" },
                    {"User-Agent",      "Mozilla/5.0" },
                },
                Content = stringContent
            };
            var httpAnswer = await _httpClient.SendAsync(content);

            var answerContent = await httpAnswer.Content.ReadAsStringAsync();
            if (httpAnswer.StatusCode != HttpStatusCode.OK)
            {
                var message = $"ContextReverso bad http response: {httpAnswer.StatusCode}, {answerContent}";
                throw new Exception(message);
            }

            BstQueryResponse deserialized;
            try
            {
                deserialized = JsonConvert.DeserializeObject<BstQueryResponse>(answerContent);
            }
            catch (JsonReaderException ex)
            {
                throw new Exception($"Json parsing exception while parsing: {answerContent}", innerException: ex);
            }
            return deserialized.Items.Select(x => new HtmlUsageSample
            {
                SourceHtml = x.SourceText,
                TargetHtml = x.TargetText,
            }).ToArray();
        }
    }
}