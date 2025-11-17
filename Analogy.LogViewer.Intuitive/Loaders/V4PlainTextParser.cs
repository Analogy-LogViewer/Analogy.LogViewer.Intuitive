#pragma warning disable MA0011
#pragma warning disable RS0030
using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.Intuitive.Loaders
{
    public class V4PlainTextParser
    {
        private string DateExample => "2023-06-18 11:12:18.214 +03:00";
        private string logLevelExample = "[INF]";
        private readonly ISplitterLogParserSettings logFileSettings;

        public V4PlainTextParser()
        {
            logFileSettings = new SplitterLogParserSettings();
            logFileSettings.AddMap(0, AnalogyLogMessagePropertyName.Date);
            logFileSettings.AddMap(1, AnalogyLogMessagePropertyName.Level);
            logFileSettings.AddMap(2, AnalogyLogMessagePropertyName.Text);
        }
        public AnalogyLogMessage Parse(string fullLine)
        {
            List<(AnalogyLogMessagePropertyName, string)> map = new List<(AnalogyLogMessagePropertyName, string)>();

            var dateSection = fullLine.Substring(0, DateExample.Length);
            var levelSection = fullLine.Substring(DateExample.Length + 1, logLevelExample.Length);
            var textSection = fullLine.Substring(DateExample.Length + 1 + logLevelExample.Length + 1);
            levelSection = levelSection.Trim().Replace("[", "").Replace("]", "");
            map.Add((AnalogyLogMessagePropertyName.Date, dateSection));
            map.Add((AnalogyLogMessagePropertyName.Level, levelSection));
            map.Add((AnalogyLogMessagePropertyName.Text, textSection));

            return AnalogyLogMessage.Parse(map);
        }

        public bool IsNewLine(string? line)
        {
            return TryGetDateTime(line, out _);
        }

        private bool TryGetDateTime(string? line, out DateTime time)
        {
            time = DateTime.MinValue;
            if (string.IsNullOrEmpty(line))
            {
                return false;
            }

            //line:2023-06-18 11:12:18.214 +03:00 [INF] *************************************************************** [06/18/2023 08:12:18]
            if (line.Length <= DateExample.Length)
            {
                return false;
            }

            if (line.Length.Equals(DateExample.Length))
            {
                return DateTime.TryParse(line, out time);
            }
            string start = line.Substring(0, DateExample.Length);
            return DateTime.TryParse(start, out time);
        }
    }
}