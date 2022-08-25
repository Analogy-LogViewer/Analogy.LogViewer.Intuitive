using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Intuitive.Serilog
{
    public class ParsingResult
    {
        public LogEvent? evt { get; set; }
        public string Line { get; set; }

        public ParsingResult(LogEvent? evt, string line)
        {
            this.evt = evt;
            this.Line = line;
        }
    }
}
