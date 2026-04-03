using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using System;
using System.IO;

namespace Analogy.LogViewer.Intuitive
{
    internal sealed class AnalogySink : ILogEventSink
    {
        private readonly ITextFormatter textFormatter;
#pragma warning disable SA1401
        public static string Output = string.Empty;
#pragma warning restore SA1401
        public AnalogySink(ITextFormatter textFormatter)
        {
            this.textFormatter = textFormatter ?? throw new ArgumentNullException(nameof(textFormatter));
        }

        public void Emit(LogEvent logEvent)
        {
            if (logEvent is null)
            {
                throw new ArgumentNullException(nameof(logEvent));
            }

            var sr = new StringWriter();
            textFormatter.Format(logEvent, sr);
            Output = sr.ToString().Trim();
        }
    }
}