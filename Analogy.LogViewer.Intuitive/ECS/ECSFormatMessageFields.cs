using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Intuitive.ECS
{
    public class ECSFormatMessageFields: IMessageFields
    {
        public string Timestamp { get; }
        public string MessageTemplate { get; }
        public string Level { get; }
        public string Exception { get; }
        public string Renderings { get; }
        public string EventId { get; }
        public string Message { get; }
        public string[] All { get; }
        public string[] Required { get; }
        string Prefix = "@";
        string EscapedInitialAt = "@@";
        public ECSFormatMessageFields()
        {
            Timestamp = "@timestamp";
            MessageTemplate = "MessageTemplate";
            Level = "log.level";
            Exception = "Exception";
            Renderings = "RenderedMessage";
            EventId = "@i";
            Message = "message";
            All = new[] { Timestamp, MessageTemplate, Level, Exception, Renderings, EventId, Message
            };
            Required = new[] { Timestamp, MessageTemplate, Level, Exception, Renderings
            };

        }
        public string Unescape(string name)
        {
            if (name.StartsWith(EscapedInitialAt))
            {
                return name.Substring(1);
            }

            return name;
        }

        public bool IsUnrecognized(string name)
        {
            return !name.StartsWith(EscapedInitialAt) &&
                   name.StartsWith(Prefix) &&
                   !All.Contains(name);
        }
    }
}
