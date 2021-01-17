using System.Threading.Tasks;

namespace MultiTranslator.AzureBot.Services.UsageSamples.ContextReverso
{
    public interface IContextReversoClient
    {
        Task<HtmlUsageSample[]> GetSamplesAsync(string text, CRLanguage from, CRLanguage to);
    }
}