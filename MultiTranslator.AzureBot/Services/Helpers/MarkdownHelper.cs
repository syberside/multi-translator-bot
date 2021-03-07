using System.IO;
using Markdig.Renderers.Normalize;
using Markdig.Syntax;

namespace MultiTranslator.AzureBot.Services.Helpers
{
    public static class MarkdownExtension
    {
        public static string ToMdString(this MarkdownDocument doc)
        {
            using (var writer = new StringWriter())
            {
                var renderer = new NormalizeRenderer(writer);
                renderer.Render(doc);
                writer.Flush();
                return writer.ToString();
            }
        }
    }
}