using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.LogViewer.Intuitive.Properties;
using Analogy.LogViewer.Template.WinForms;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Intuitive.LogsParser
{
    public class LightHouseNodeTraceParser : OfflineDataProviderWinForms
    {
        public override string? OptionalTitle { get; set; } = "LightHouse Node Trace CSV Log";
        public override string? InitialFolderFullPath { get; set; } = Environment.CurrentDirectory;
        public override Image? LargeImage { get; set; } = Resources.Intuitive32x32;
        public override Image? SmallImage { get; set; } = Resources.Intuitive16x16;
        public override string FileOpenDialogFilters { get; set; } = "LightHouse log files (*.csv)|*.csv";
        public override Guid Id { get; set; } = new Guid("fc14e03d-1820-4a97-88d2-6d92c413472e");

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
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    MissingFieldFound = null,
                    BadDataFound = (BadDataFoundArgs args) =>
                    {
                        var msg = new AnalogyLogMessage()
                        {
                            Text = args.RawRecord,
                            Level = AnalogyLogLevel.Information,
                            RawText = args.RawRecord,
                            RawTextType = AnalogyRowTextType.PlainText,
                        };
                        messagesHandler.AppendMessage(msg, fileName);
                        msgs.Add(msg);
                    },
                    Delimiter = " ",
                    WhiteSpaceChars = [],
                };
                using (var reader = new StreamReader(fileName))
                using (var csv = new CsvReader(reader, config))
                {
                    csv.Context.RegisterClassMap<LightHouseRowRecordMapper>();
                    await foreach (var record in csv.GetRecordsAsync<LightHouseRowRecord>(token))
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

        private IAnalogyLogMessage ParseMessage(LightHouseRowRecord record, string raw)
        {
            string text = $"{record.PositionZero} {record.PositionOne} {record.PositionTwo} {record.PositionThree}";
            if (record.PositionZero.StartsWith("#"))
            {
                if (record.PositionOne.Contains("power_cycle_uuid"))
                {
                    PowerCycle = record.PositionTwo;
                }
                var header = new AnalogyLogMessage()
                {
                    Text = text,
                    Level = AnalogyLogLevel.Information,
                    RawText = raw,
                    RawTextType = AnalogyRowTextType.PlainText,
                };
                AddIfNotEmpty(header, "PowerCycleUuid", PowerCycle);
                return header;
            }
            string start = $"{record.PositionZero} {record.PositionOne} {record.PositionTwo}";
            string otherText = raw.Replace(start, "");
            AnalogyLogLevel level = AnalogyLogLevel.Information;
            if (otherText.Contains("WARN"))
            {
                level = AnalogyLogLevel.Warning;
            }
            var m = new AnalogyLogMessage()
            {
                Text = otherText,
                Module = record.PositionOne,
                Source = record.PositionTwo,
                Level = level,
                Date = ParseDateTime(record.PositionZero),
                
                //MachineName = record.MachineName,
                //User = record.User,
                //PositionOne = record.LoggerName,
                //ThreadId = int.TryParse(record.ThreadId, NumberStyles.Any, new CultureInfo("en-US"), out var ti) ? ti : -1,
                //ProcessId = int.TryParse(record.ProcessId, NumberStyles.Any, new CultureInfo("en-US"), out var pi) ? pi : -1,
                //Level = AnalogyLogMessage.ParseLogLevelFromString(record.LogLevel),
                //LineNumber = long.TryParse(record.Line, NumberStyles.Any, new CultureInfo("en-US"), out var ln) ? ln : -1,
                //MethodName = record.LabelMethod,
                RawTextType = AnalogyRowTextType.PlainText,
                RawText = raw,
            };
            AddIfNotEmpty(m, "PowerCycleUuid", PowerCycle);
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
            if (DateTimeOffset.TryParseExact(timestamp, "yyyy-MM-ddTHH:mm:ss.ffffffK", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var dt))
            {
                return dt;
            }
            return DateTimeOffset.UtcNow;
        }
    }

    public class LightHouseRowRecord
    {
        public string PositionZero { get; set; } = "";
        public string PositionOne { get; set; } = "";
        public string PositionTwo { get; set; } = "";
        public string PositionThree { get; set; } = "";
    }

    public sealed class LightHouseRowRecordMapper : ClassMap<LightHouseRowRecord>
    {
        public LightHouseRowRecordMapper()
        {
            Map(m => m.PositionZero).Optional().Index(0);
            Map(m => m.PositionOne).Optional().Index(1);
            Map(m => m.PositionTwo).Optional().Index(2);
            Map(m => m.PositionThree).Optional().Index(3);
        }
    }
}