using Serilog.Events;

namespace Analogy.LogViewer.Intuitive.Serilog
{
    public class ParsingResult
    {
        public LogEvent? LogEvent { get; set; }
        public string Line { get; set; }

        public ParsingResult(LogEvent? logEvent, string line)
        {
            this.LogEvent = logEvent;
            this.Line = line;
        }
    }
}