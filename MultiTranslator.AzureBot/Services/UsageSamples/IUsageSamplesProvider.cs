using System.Threading.Tasks;
using Markdig.Syntax;

namespace EchoBot.Services.UsageSamples
{
    public interface IUsageSamplesProvider
    {
        Task<MardownUsageSample[]> GetSamplesAsync(string text, Languages from, Languages to);
    }

    public class MardownUsageSample
    {
        public MarkdownDocument SourceMd { get; set; }
        public MarkdownDocument TargetMd { get; set; }
    }
}