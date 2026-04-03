using System;

namespace Analogy.LogViewer.Intuitive.Serilog
{
    internal static class MessageTemplateSyntax
    {
        public static string Escape(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return text.Replace("{", "{{").Replace("}", "}}");
        }
    }
}