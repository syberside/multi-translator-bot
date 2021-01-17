using System.Threading.Tasks;

namespace EchoBot.Services.UsageSamples.ContextReverso
{
    public interface IContextReversoClient
    {
        Task<HtmlUsageSample[]> GetSamplesAsync(string text, CRLanguage from, CRLanguage to);
    }
}