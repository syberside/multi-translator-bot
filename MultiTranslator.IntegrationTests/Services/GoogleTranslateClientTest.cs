using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using MultiTranslator.AzureBot.Services;
using FluentAssertions;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Translation.V2;
using Xunit;

namespace MultiTranslator.IntegrationTests.Services
{
    public class GoogleTranslateClientTest
    {
        [Fact]
        public async Task CanTranslate()
        {
            var googleClient = GetGoogleTranslateClient();

            var client = new GoogleTranslateFacade(googleClient);

            var result = await client.TranslateAsync("hello", Languages.En, Languages.Ru);

            result.Should().BeEquivalentTo("здравствуйте");
        }

        private TranslationClient GetGoogleTranslateClient()
        {
            var binDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //var rootDir = Path.GetFullPath(Path.Combine(binDir, ".."));
            var configPath = Path.Combine(binDir, "gcp-creds.json");
            var creds = GoogleCredential.FromFile(configPath);
            return TranslationClient.Create(creds);
        }
    }
}
