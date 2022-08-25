using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Intuitive.Serilog
{
    static class MessageTemplateSyntax
    {
        public static string Escape(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return text.Replace("{", "{{").Replace("}", "}}");
        }
    }
}
