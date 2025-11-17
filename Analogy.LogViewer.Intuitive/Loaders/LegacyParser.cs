using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Analogy.LogViewer.Intuitive.Loaders
{
    internal sealed class LegacyParser
    {
        private readonly ISplitterLogParserSettings logFileSettings;
#pragma warning disable SA1401
        public readonly string[] Splitters;
#pragma warning restore SA1401
        private static string[] SplitterValues { get; } = { " - " };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public LegacyParser()
        {
            Splitters = SplitterValues;
            logFileSettings = new SplitterLogParserSettings();
            logFileSettings.AddMap(0, AnalogyLogMessagePropertyName.Date);
            logFileSettings.AddMap(1, AnalogyLogMessagePropertyName.Module);
            logFileSettings.AddMap(2, AnalogyLogMessagePropertyName.Text);
        }
        public AnalogyLogMessage Parse(string line)
        {
            var items = line.Split(Splitters, StringSplitOptions.None).ToList();
            List<(AnalogyLogMessagePropertyName, string)> map = new List<(AnalogyLogMessagePropertyName, string)>();
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (logFileSettings.Maps.TryGetValue(i, out var result))
                {
                    map.Add((result, items[i]));
                }
            }
            return AnalogyLogMessage.Parse(map);
        }
    }
}