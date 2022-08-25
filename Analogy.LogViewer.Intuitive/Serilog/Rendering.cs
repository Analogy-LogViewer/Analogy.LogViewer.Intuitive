using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Intuitive.Serilog
{
    class Rendering
    {
        public string Name { get; }
        public string Format { get; }
        public string Rendered { get; }

        public Rendering(string name, string format, string rendered)
        {
            Name = name;
            Format = format;
            Rendered = rendered;
        }
    }
}
