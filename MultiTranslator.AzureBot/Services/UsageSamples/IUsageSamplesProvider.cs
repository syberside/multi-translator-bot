using System.Threading.Tasks;

namespace MultiTranslator.AzureBot.Services.UsageSamples
{
    public interface IUsageSamplesProvider
    {
        Task<MardownUsageSample[]> GetSamplesAsync(string text, Languages from, Languages to);
    }
}