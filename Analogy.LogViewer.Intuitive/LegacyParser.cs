using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analogy.LogViewer.Intuitive
{
    internal class LegacyParser
    {
        private readonly ISplitterLogParserSettings _logFileSettings;

        public readonly string[] splitters;
        public static string[] SplitterValues { get; } = { " - " };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public LegacyParser()
        {
            splitters = SplitterValues;
            _logFileSettings = new SplitterLogParserSettings();
            _logFileSettings.AddMap(0,AnalogyLogMessagePropertyName.Date);
            _logFileSettings.AddMap(1,AnalogyLogMessagePropertyName.Module);
            _logFileSettings.AddMap(2,AnalogyLogMessagePropertyName.Text);
        }
        public AnalogyLogMessage Parse(string line)
        {
            var items = line.Split(splitters, StringSplitOptions.None).ToList();
            List<(AnalogyLogMessagePropertyName, string)> map = new List<(AnalogyLogMessagePropertyName, string)>();
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (_logFileSettings.Maps.ContainsKey(i))
                {
                    map.Add((_logFileSettings.Maps[i], items[i]));
                }
            }
            return AnalogyLogMessage.Parse(map);
        }
    }
}
