using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.LogViewer.Intuitive.Properties;
using Analogy.LogViewer.Intuitive.Types;
using Analogy.LogViewer.Template.WinForms;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Intuitive.LogsParser
{
    public class LightHouseEventsParser : OfflineDataProviderWinForms
    {
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
                        };
                        messagesHandler.AppendMessage(msg, fileName);
                        msgs.Add(msg);
                    },
                    Delimiter = ",",
                    WhiteSpaceChars = [],
                };
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
            var m = new AnalogyLogMessage()
            {
                Text = $"{record.Message} {otherText} (Time: {record.Time} ServoSync: {record.Servosync})",
                Source = record.Src,
                Module = record.Message.Substring(0, record.Message.IndexOf(' ')),
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
                return dt;
            }
            return DateTimeOffset.UtcNow;
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