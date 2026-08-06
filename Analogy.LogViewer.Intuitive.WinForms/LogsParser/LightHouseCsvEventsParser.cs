using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.LogViewer.Intuitive.Types;
using Analogy.LogViewer.Intuitive.WinForms.Properties;
using Analogy.LogViewer.Template.WinForms;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Intuitive.WinForms.LogsParser
{
    public class LightHouseCsvEventsParser : OfflineDataProviderWinForms
    {
        private static DateTimeOffset LastDateTimeOffset { get; set; } = DateTimeOffset.UtcNow;
        public override string? OptionalTitle { get; set; } = "LightHouse Events CSV Log";
        public override string? InitialFolderFullPath { get; set; } = Environment.CurrentDirectory;
        public override Image? LargeImage { get; set; } = Resources.Intuitive32x32;
        public override Image? SmallImage { get; set; } = Resources.Intuitive16x16;
        public override string FileOpenDialogFilters { get; set; } = "LightHouse event log files (*.csv;*.log)|*.csv;*.log";
        public override Guid Id { get; set; } = new Guid("D851928C-65F2-4625-B9E9-C58E487A481B");

        public override IEnumerable<string> SupportFormats { get; set; } = new[] { "*.csv", "*.log" };
        private string PowerCycle { get; set; } = "";
        public override async Task InitializeDataProvider(ILogger logger)
        {
            await base.InitializeDataProvider(logger);
        }

        public override async Task<IEnumerable<IAnalogyLogMessage>> Process(string fileName, CancellationToken token,
            ILogMessageCreatedHandler messagesHandler)
        {
            var msgs = new List<IAnalogyLogMessage>(0);
            if (CanOpenFile(fileName))
            {
                if (await IsEvtTextOnlyFormat(fileName, token))
                {
                    await ProcessEvtTextOnlyFile(fileName, token, messagesHandler, msgs);
                    return msgs;
                }

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    MissingFieldFound = null,
                    BadDataFound = (BadDataFoundArgs args) =>
                    {
                        var msg = new AnalogyLogMessage()
                        {
                            Text = args.RawRecord,
                            Level = AnalogyLogLevel.Information,
                            RawText = args.RawRecord,
                            RawTextType = AnalogyRowTextType.PlainText,
                            Date = LastDateTimeOffset,
                        };
                        messagesHandler.AppendMessage(msg, fileName);
                        msgs.Add(msg);
                    },
                    Delimiter = ",",
                    WhiteSpaceChars = [],
                };
                LastDateTimeOffset = DateTimeOffset.UtcNow;
                using (var reader = new StreamReader(fileName))
                using (var csv = new CsvReader(reader, config))
                {
                    csv.Context.RegisterClassMap<LightHouseRowRecordMapper>();
                    await foreach (var record in csv.GetRecordsAsync<LightHouseEventRowRecord>(token))
                    {
                        try
                        {
                            var entry = ParseMessage(record, csv.Parser.RawRecord);
                            entry.RawText = csv.Parser.RawRecord;
                            entry.RawTextType = AnalogyRowTextType.PlainText;
                            messagesHandler.AppendMessage(entry, fileName);
                            msgs.Add(entry);
                        }
                        catch (Exception e)
                        {
                            AnalogyErrorMessage err = new AnalogyErrorMessage("Error Decrypting: " + e);
                            messagesHandler.AppendMessage(err, "Analogy");
                            msgs.Add(err);
                        }
                    }
                }
            }

            return msgs;
        }

        private async Task ProcessEvtTextOnlyFile(string fileName, CancellationToken token,
            ILogMessageCreatedHandler messagesHandler, List<IAnalogyLogMessage> msgs)
        {
            LastDateTimeOffset = DateTimeOffset.UtcNow;
#if NET
            var lines = await File.ReadAllLinesAsync(fileName, token);
#else
            token.ThrowIfCancellationRequested();
            var lines = File.ReadAllLines(fileName);
#endif
            for (var i = 1; i < lines.Length; i++)
            {
                token.ThrowIfCancellationRequested();
                var rawLine = lines[i];
                if (string.IsNullOrWhiteSpace(rawLine))
                {
                    continue;
                }

                try
                {
                    var evtText = UnwrapCsvQuotedValue(rawLine);
                    var entry = ParseEvtTextOnlyMessage(evtText, rawLine);
                    entry.RawText = rawLine;
                    entry.RawTextType = AnalogyRowTextType.PlainText;
                    messagesHandler.AppendMessage(entry, fileName);
                    msgs.Add(entry);
                }
                catch (Exception e)
                {
                    var err = new AnalogyErrorMessage("Error Decrypting: " + e);
                    messagesHandler.AppendMessage(err, "Analogy");
                    msgs.Add(err);
                }
            }
        }

        private static async Task<bool> IsEvtTextOnlyFormat(string fileName, CancellationToken token)
        {
            using var reader = new StreamReader(fileName);
            string? firstLine;
#if NET
            firstLine = await reader.ReadLineAsync(token);
#else
            token.ThrowIfCancellationRequested();
            firstLine = await reader.ReadLineAsync();
#endif
            if (string.IsNullOrWhiteSpace(firstLine))
            {
                return false;
            }

            return firstLine.Trim().Equals("evt_text", StringComparison.OrdinalIgnoreCase);
        }

        private IAnalogyLogMessage ParseEvtTextOnlyMessage(string evtText, string raw)
        {
            var level = ParseLevelFromEvtText(evtText);
            var source = ParseSourceFromEvtText(evtText);
            var module = ParseModuleFromEvtText(evtText, level);

            var m = new AnalogyLogMessage()
            {
                Text = evtText,
                Source = source,
                Module = module,
                Level = level,
                Date = ParseDateTimeFromEvtText(evtText),
                RawTextType = AnalogyRowTextType.PlainText,
                RawText = raw,
            };

            return m;
        }

        private static string UnwrapCsvQuotedValue(string line)
        {
            var value = line.Trim();
            if (value.Length >= 2 && value[0] is '"' && value[value.Length - 1] is '"')
            {
#if NET
                value = value[1..^1].Replace("\"\"", "\"", StringComparison.Ordinal);
#else
                value = value.Substring(1, value.Length - 2).Replace("\"\"", "\"");
#endif
            }

            return value;
        }

        private static DateTimeOffset ParseDateTimeFromEvtText(string evtText)
        {
            var match = Regex.Match(evtText, @"\d{4}/\d{2}/\d{2} \d{2}:\d{2}:\d{2}");
            if (match.Success &&
                DateTimeOffset.TryParseExact(match.Value, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var dt))
            {
                LastDateTimeOffset = dt;
            }

            return LastDateTimeOffset;
        }

        private static AnalogyLogLevel ParseLevelFromEvtText(string evtText)
        {
            if (ContainsIgnoreCase(evtText, " ERR "))
            {
                return AnalogyLogLevel.Error;
            }

            if (ContainsIgnoreCase(evtText, " WARN "))
            {
                return AnalogyLogLevel.Warning;
            }

            return AnalogyLogLevel.Information;
        }

        private static string ParseSourceFromEvtText(string evtText)
        {
            var nodeMatch = Regex.Match(evtText, @"\):\s*(\d+):");
            return nodeMatch.Success ? nodeMatch.Groups[1].Value : string.Empty;
        }

        private static string ParseModuleFromEvtText(string evtText, AnalogyLogLevel level)
        {
            var restMatch = Regex.Match(evtText, @"\):\s*\d+:\s*\{\d+\}\s*(.+)$");
            if (restMatch.Success)
            {
                var rest = restMatch.Groups[1].Value.Trim();
                if (!string.IsNullOrEmpty(rest))
                {
                    if (rest.IndexOf(':') >= 0)
                    {
                        var idx = rest.IndexOf(':');
                        if (idx > 0)
                        {
#if NET
                            return rest[..idx].Trim();
#else
                            return rest.Substring(0, idx).Trim();
#endif
                        }
                    }

                    var firstSpace = rest.IndexOf(' ');
                    if (firstSpace > 0)
                    {
#if NET
                        return rest[..firstSpace].Trim();
#else
                        return rest.Substring(0, firstSpace).Trim();
#endif
                    }

                    return rest;
                }
            }

            return level.ToString();
        }

        private static bool ContainsIgnoreCase(string text, string value)
        {
#if NET
            return text.Contains(value, StringComparison.OrdinalIgnoreCase);
#else
            return text.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
#endif
        }

        private IAnalogyLogMessage ParseMessage(LightHouseEventRowRecord record, string raw)
        {
            string otherText = raw.Substring(raw.IndexOf(record.Message, StringComparison.Ordinal) + record.Message.Length);
            AnalogyLogLevel level = AnalogyLogLevel.Information;
            if (record.Message.StartsWith("WARN"))
            {
                level = AnalogyLogLevel.Warning;
            }
            if (record.Message.StartsWith("ERR"))
            {
                level = AnalogyLogLevel.Error;
            }

            string module = record.Message;
            var i = record.Message.IndexOf(' ');
            if (i > 0)
            {
#if NET
                module = record.Message[..record.Message.IndexOf(' ')];
#else
                module = record.Message.Substring(0, record.Message.IndexOf(' '));
#endif
            }
            var m = new AnalogyLogMessage()
            {
                Text = $"{record.Message} {otherText} (Time: {record.Time} ServoSync: {record.Servosync})",
                Source = record.Src,
                Module = module,
                Level = level,
                Date = ParseDateTime(record.Time),
                RawTextType = AnalogyRowTextType.PlainText,
                RawText = raw,
            };
            AddIfNotEmpty(m, "Servosync", record.Servosync);
            if (m.AdditionalProperties is not null)
            {
                if (m.AdditionalProperties.TryGetValue("Exception", out var er) && !string.IsNullOrEmpty(er))
                {
                    m.Text += Environment.NewLine + string.Format(CultureInfo.InvariantCulture, "Error: {0}", er);
                }
                if (m.AdditionalProperties.TryGetValue("StackTrace", out var ex) && !string.IsNullOrEmpty(ex))
                {
                    m.Text += Environment.NewLine + string.Format(CultureInfo.InvariantCulture, "Exception: {0}", ex);
                }
            }
            return m;
        }

        private void AddIfNotEmpty(AnalogyLogMessage msg, string key, string value)
        {
            if (string.IsNullOrEmpty(value) || value is "-")
            {
                return;
            }
            msg.AddOrReplaceAdditionalProperty(key, value, StringComparer.OrdinalIgnoreCase);
        }
        public static DateTimeOffset ParseDateTime(string timestamp)
        {
            if (DateTimeOffset.TryParse(timestamp, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var dt))
            {
                LastDateTimeOffset = dt;
            }
            return LastDateTimeOffset;
        }
    }

    public sealed class LightHouseEventRowRecordMapper : ClassMap<LightHouseEventRowRecord>
    {
        public LightHouseEventRowRecordMapper()
        {
            Map(m => m.Time).Name(["Time"]).Optional().Index(0);
            Map(m => m.Servosync).Name("Servosync").Optional().Index(1);
            Map(m => m.Src).Name("Src").Optional().Index(2);
            Map(m => m.Message).Name("Message").Optional().Index(3);
        }
    }
}