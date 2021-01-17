using System.Net.Http;
using System.Threading.Tasks;
using EchoBot.Services.UsageSamples.ContextReverso;
using FluentAssertions;
using Xunit;

namespace MultiTranslator.IntegrationTests.Services
{
    public class ContextReversoClientTest
    {
        [Fact]
        public async Task CanTranslate()
        {
            var httpClient = new HttpClient();
            var client = new ContextReversoClient(httpClient);

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
    }
}