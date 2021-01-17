using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using MultiTranslator.AzureBot.Services;
using MultiTranslator.AzureBot.Services.Helpers;
using MultiTranslator.AzureBot.Services.UsageSamples.ContextReverso;
using FluentAssertions;
using Moq;
using Xunit;

namespace MultiTranslator.UnitTests.Services
{
    public class ContextReversoUsageSamplesAdapterTest
    {
        private readonly ContextReversoUsageSamplesAdapter _adapter;
        private readonly Mock<IContextReversoClient> _clientMock;
        private readonly Fixture _fixture;
        public ContextReversoUsageSamplesAdapterTest()
        {
            _fixture = new Fixture();
            _clientMock = new Mock<IContextReversoClient>();
            _adapter = new ContextReversoUsageSamplesAdapter(_clientMock.Object);
        }

        [Fact]
        public async Task GetSamplesAsync_ReplacesEmTagsWithAsterisks()
        {
            var request = _fixture.Create<string>();
            var sampleResponse = new HtmlUsageSample
            {
                SourceHtml = "hello <em>everybody</em>",
                TargetHtml = "<em>everybody</em> here",
            };
            _clientMock
                .Setup(x => x.GetSamplesAsync(request, It.IsAny<CRLanguage>(), It.IsAny<CRLanguage>()))
                .ReturnsAsync(new[] { sampleResponse, });

            var result = await _adapter.GetSamplesAsync(request, _fixture.Create<Languages>(), _fixture.Create<Languages>());

            result.Should().NotBeNull().And.HaveCount(1);
            var item = result.First();

            item.SourceMd.Should().NotBeNull();
            item.SourceMd.ToMdString().Should().BeEquivalentTo("hello **everybody**");
            item.TargetMd.Should().NotBeNull();
            item.TargetMd.ToMdString().Should().BeEquivalentTo("**everybody** here");
        }
    }
}