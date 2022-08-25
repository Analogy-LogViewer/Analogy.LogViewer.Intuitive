using Newtonsoft.Json.Linq;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Intuitive.Serilog
{
    static class PropertyFactory
    {
        const string TypeTagPropertyName = "$type";

        public static LogEventProperty CreateProperty(string name, JToken value, List<Rendering> renderings)
        {
            return new LogEventProperty(name, CreatePropertyValue(value, renderings));
        }

        static LogEventPropertyValue CreatePropertyValue(JToken value, List<Rendering> renderings)
        {
            if (value.Type == JTokenType.Null)
            {
                return new ScalarValue(null);
            }

            if (value is JObject obj)
            {
                JToken tt;
                obj.TryGetValue(TypeTagPropertyName, out tt);
                return new StructureValue(
                    obj.Properties().Where(kvp => kvp.Name != TypeTagPropertyName).Select(kvp => CreateProperty(kvp.Name, kvp.Value, null)),
                    tt?.Value<string>());
            }

            if (value is JArray arr)
            {
                return new SequenceValue(arr.Select(v => CreatePropertyValue(v, null)));
            }

            var raw = value.Value<JValue>().Value;

            return renderings != null && renderings.Any() ?
                new RenderableScalarValue(raw, renderings) :
                new ScalarValue(raw);
        }
    }
}
