using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Intuitive
{
    class AnalogySink : ILogEventSink
    {
        readonly ITextFormatter _textFormatter;
        public static string output = string.Empty;
        public AnalogySink(ITextFormatter textFormatter)
        {
            _textFormatter = textFormatter ?? throw new ArgumentNullException(nameof(textFormatter));

        }

        public void Emit(LogEvent logEvent)
        {
            if (logEvent == null)
            {
                throw new ArgumentNullException(nameof(logEvent));
            }

            var sr = new StringWriter();
            _textFormatter.Format(logEvent, sr);
            output = sr.ToString().Trim();
        }
    }

}
