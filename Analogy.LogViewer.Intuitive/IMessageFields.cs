using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Intuitive
{

    public interface IMessageFields
    {
        string Timestamp { get; }
        string MessageTemplate { get; }
        string Level { get; }
        string Exception { get; }
        string Renderings { get; }
        string EventId { get; }
        string Message { get; }
        string[] All { get; }
        string[] Required { get; }
        string Unescape(string name);
        bool IsUnrecognized(string name);
    }
}
