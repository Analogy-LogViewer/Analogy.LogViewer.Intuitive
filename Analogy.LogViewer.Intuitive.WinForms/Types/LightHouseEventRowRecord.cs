using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Intuitive.Types
{
    public class LightHouseEventRowRecord
    {
        public string Time { get; set; } = "";
        public string Servosync { get; set; } = "";
        public string Src { get; set; } = "";
        public string Message { get; set; } = "";
    }
}