using System.Net.Http;
using System.Threading.Tasks;
using MultiTranslator.AzureBot.Services.UsageSamples.ContextReverso;
using FluentAssertions;
using Xunit;

namespace MultiTranslator.IntegrationTests.Services
{
    public class ContextReversoClientTest
    {
        [Fact]
        public async Task CanTranslate()
        {
            var httpClientFactory = new HttpFactory();
            var client = new ContextReversoClient(httpClientFactory);

            ValidateResult(await client.GetSamplesAsync("добрый вечер!", CRLanguage.Ru, CRLanguage.Eng));

            ValidateResult(await client.GetSamplesAsync("good evening!", CRLanguage.Eng, CRLanguage.Ru));

        }

        private static void ValidateResult(HtmlUsageSample[] result)
        {
            result.Should().NotBeNullOrEmpty();
            foreach (var item in result)
            {
                item.SourceHtml.Should().NotBeNullOrWhiteSpace().And.NotBeEmpty();//.And.ContainAll("<em>", "</em>");
                item.TargetHtml.Should().NotBeNullOrWhiteSpace().And.NotBeEmpty();//.And.ContainAll("<em>", "</em>");
            }
        }

        private class HttpFactory : IHttpClientFactory
        {
            public HttpClient CreateClient(string name)
            {
                return new HttpClient();
            }
        }
    }
}