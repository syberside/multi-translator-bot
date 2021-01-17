using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Markdig;
using Markdig.Syntax;

namespace MultiTranslator.AzureBot.Services.UsageSamples.ContextReverso
{
    public class ContextReversoUsageSamplesAdapter : IUsageSamplesProvider
    {
        private readonly IContextReversoClient _client;

        public ContextReversoUsageSamplesAdapter(IContextReversoClient client)
        {
            _client = client;
        }

        public async Task<MardownUsageSample[]> GetSamplesAsync(string text, Languages from, Languages to)
        {
            var langFrom = MapLanguageToCode(from);
            var langTo = MapLanguageToCode(to);

            var htmlExamples = await _client.GetSamplesAsync(text, langFrom, langTo);

            var md = TransformHtmlToMd(htmlExamples);
            return md;
        }

        private MardownUsageSample[] TransformHtmlToMd(HtmlUsageSample[] htmlExamples)
        {
            return htmlExamples.Select(x => new MardownUsageSample
            {
                //TODO: use more robust solution for html parsing
                SourceMd = ReplaceEmTagWithAsterisks(x.SourceHtml),
                TargetMd = ReplaceEmTagWithAsterisks(x.TargetHtml),
            }).ToArray();
        }

        private MarkdownDocument ReplaceEmTagWithAsterisks(string html)
        {
            var mdText = Regex.Replace(html, @"<em>", "**");
            mdText = Regex.Replace(mdText, @"</em>", "**");
            return Markdown.Parse(mdText);
        }

        private CRLanguage MapLanguageToCode(Languages language)
        {
            switch (language)
            {
                case Languages.En: return CRLanguage.Eng;
                case Languages.Ru: return CRLanguage.Ru;
                default: throw new NotSupportedException();
            }
        }
    }
}