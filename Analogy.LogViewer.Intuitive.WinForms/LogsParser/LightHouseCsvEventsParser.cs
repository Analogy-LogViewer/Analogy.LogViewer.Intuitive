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
        public override string FileOpenDialogFilters { get; set; } = "LightHouse event log files (*.csv)|*.csv";
        public override Guid Id { get; set; } = new Guid("D851928C-65F2-4625-B9E9-C58E487A481B");

        public override IEnumerable<string> SupportFormats { get; set; } = new[] { "*.csv" };
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
            var lines = await File.ReadAllLinesAsync(fileName, token);
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
            var firstLine = await reader.ReadLineAsync(token);
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
            if (value.Length >= 2 && value[0] is '"' && value[^1] is '"')
            {
                value = value[1..^1].Replace("\"\"", "\"", StringComparison.Ordinal);
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
            if (evtText.Contains(" ERR ", StringComparison.OrdinalIgnoreCase))
            {
                return AnalogyLogLevel.Error;
            }

            if (evtText.Contains(" WARN ", StringComparison.OrdinalIgnoreCase))
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
                    if (rest.Contains(':', StringComparison.Ordinal))
                    {
                        var idx = rest.IndexOf(':');
                        if (idx > 0)
                        {
                            return rest[..idx].Trim();
                        }
                    }

                    var firstSpace = rest.IndexOf(' ');
                    if (firstSpace > 0)
                    {
                        return rest[..firstSpace].Trim();
                    }

                    return rest;
                }
            }

            return level.ToString();
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
                module = record.Message[..record.Message.IndexOf(' ')];
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
                    m.Text += Environment.NewLine + string.Create(CultureInfo.InvariantCulture, $"Error: {er}");
                }
                if (m.AdditionalProperties.TryGetValue("StackTrace", out var ex) && !string.IsNullOrEmpty(ex))
                {
                    m.Text += Environment.NewLine + string.Create(CultureInfo.InvariantCulture, $"Exception: {ex}");
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