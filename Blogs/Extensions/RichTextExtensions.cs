using Contentful.Core.Models;
using System.Text;

namespace Blogs.Extensions
{
    public static class RichTextExtensions
    {
        public static string ToPlainText(this Document document)
        {
            var sb = new StringBuilder();
            ExtractText(document.Content, sb);
            return sb.ToString();
        }

        private static void ExtractText(List<IContent> contentNodes, StringBuilder sb)
        {
            foreach (var node in contentNodes)
            {
                if (node is Paragraph paragraph)
                {
                    foreach (var inline in paragraph.Content)
                    {
                        if (inline is Text text)
                            sb.Append(text.Value);
                    }
                    sb.AppendLine();
                }
            }
        }
    }

}
