#pragma warning disable CS8604 // Possible null reference argument.
using Newtonsoft.Json.Linq;
using Serilog.Events;
using System.Collections.Generic;
using System.Linq;

namespace Analogy.LogViewer.Intuitive.Serilog
{
    internal static class PropertyFactory
    {
        private const string TypeTagPropertyName = "$type";

        public static LogEventProperty CreateProperty(string name, JToken value, List<Rendering>? renderings)
        {
            return new LogEventProperty(name, CreatePropertyValue(value, renderings));
        }

        private static LogEventPropertyValue CreatePropertyValue(JToken value, List<Rendering>? renderings)
        {
            if (value.Type is JTokenType.Null)
            {
                return new ScalarValue(value: null);
            }

            if (value is JObject obj)
            {
                JToken? tt;
                obj.TryGetValue(TypeTagPropertyName, out tt);
                return new StructureValue(
#pragma warning disable MA0006
                    obj.Properties().Where(kvp => kvp.Name != TypeTagPropertyName).Select(kvp => CreateProperty(kvp.Name, kvp.Value, renderings: null)),
#pragma warning restore MA0006
                    tt?.Value<string>());
            }

            if (value is JArray arr)
            {
                return new SequenceValue(arr.Select(v => CreatePropertyValue(v, renderings: null)));
            }

            var raw = value.Value<JValue>()?.Value;

            return renderings is not null && renderings.Count is not 0 ?
                new RenderableScalarValue(raw, renderings) :
                new ScalarValue(raw);
        }
    }
}