using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;

namespace Analogy.LogViewer.Intuitive.Serilog
{
    internal sealed class RenderableScalarValue : ScalarValue
    {
        private readonly Dictionary<string, string> renderings = new(StringComparer.OrdinalIgnoreCase);

        public RenderableScalarValue(object value, List<Rendering> renderings)
            : base(value)
        {
            if (renderings is null)
            {
                throw new ArgumentNullException(nameof(renderings));
            }

            foreach (var rendering in renderings)
            {
                this.renderings[rendering.Format] = rendering.Rendered;
            }
        }

        public override void Render(TextWriter output, string? format = null, IFormatProvider? formatProvider = null)
        {
            string? rendering;
            if (format is not null && renderings.TryGetValue(format, out rendering))
            {
                output.Write(rendering);
            }
            else
            {
                base.Render(output, format, formatProvider);
            }
        }
    }
}