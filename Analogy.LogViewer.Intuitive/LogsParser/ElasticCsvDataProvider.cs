#pragma warning disable MA0048
#pragma warning disable RS0030
using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.LogViewer.Intuitive.Properties;
using Analogy.LogViewer.Template.WinForms;
using CsvHelper;
using CsvHelper.Configuration;
using MediaManager.Logging;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
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
    public sealed class ElasticCsvDataProvider : OfflineDataProviderWinForms
    {
        public override string? OptionalTitle { get; set; } = "Hawk-Eye CSV Log";
        public override string? InitialFolderFullPath { get; set; } = Environment.CurrentDirectory;
        public override Image? LargeImage { get; set; } = Resources.elasticsearch32x32;
        public override Image? SmallImage { get; set; } = Resources.elasticsearch16x16;
        public override string FileOpenDialogFilters { get; set; } = "log files (*.csv)|*.csv";
        public override Guid Id { get; set; } = new Guid("417535d6-ed53-42aa-a0b2-f3a0cf96241f");

        public override IEnumerable<string> SupportFormats { get; set; } = new[] { "*.csv" };

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
                using (var reader = new StreamReader(fileName))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<ElasticRowRecordMapper>();
                    await foreach (var record in csv.GetRecordsAsync<ElasticRowRecord>(token))
                    {
                        try
                        {
                            var entry = ParseMessage(record);
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

        private IAnalogyLogMessage ParseMessage(ElasticRowRecord record)
        {
            var m = new AnalogyLogMessage()
            {
                Text = $"{ParseDateTime(record.Timestamp)}: {record.Text}",
                MachineName = record.MachineName,
                User = record.User,
                Module = record.ProcessName,
                Source = record.LoggerName,
                ThreadId = int.TryParse(record.ThreadId, NumberStyles.Any, new CultureInfo("en-US"), out var ti) ? ti : -1,
                ProcessId = int.TryParse(record.ProcessId, NumberStyles.Any, new CultureInfo("en-US"), out var pi) ? pi : -1,
                Level = AnalogyLogMessage.ParseLogLevelFromString(record.LogLevel),
                Date = ParseDateTime(record.Timestamp),
                LineNumber = long.TryParse(record.Line, NumberStyles.Any, new CultureInfo("en-US"), out var ln) ? ln : -1,
                MethodName = record.LabelMethod,
                RawTextType = AnalogyRowTextType.None,
            };
            ResetFieldsIfNeeded(record);
            AddIfNotEmpty(m, "Application Instance", record.ApplicationInstance);
            AddIfNotEmpty(m, "Session Id", record.MetaSessionId);
            AddIfNotEmpty(m, "Session Id", record.SessionId);
            AddIfNotEmpty(m, "Message Template", record.MessageTemplate);
            AddIfNotEmpty(m, "Exception", record.ExceptionMessage);
            AddIfNotEmpty(m, "StackTrace", record.ExceptionStack);
            AddIfNotEmpty(m, "PowerCycleUuid", record.PowerCycleUuid);
            AddIfNotEmpty(m, "ProcedureUuid", record.ProcedureUuid);
            AddIfNotEmpty(m, "PairingUuid", record.PairingUuid);
            AddIfNotEmpty(m, "Msix", record.Msix);
            AddIfNotEmpty(m, "MMA", record.MMAVersion);
            AddIfNotEmpty(m, "DaVinciSoftwareVersion", record.DaVinciSoftwareVersion);
            AddIfNotEmpty(m, "Session Summary", record.SessionSummary);
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

            if (!string.IsNullOrEmpty(record.SessionId))
            {
                m.Text += Environment.NewLine + $"Session: {record.SessionId}";
            }
            if (!string.IsNullOrEmpty(record.MetaSessionId))
            {
                m.Text += Environment.NewLine + $"Session: {record.MetaSessionId}";
            }
            return m;
        }

        private static void ResetFieldsIfNeeded(ElasticRowRecord record)
        {
            if (record.MetaSessionId is "-")
            {
                record.MetaSessionId = string.Empty;
            }
            if (record.SessionId is "-")
            {
                record.SessionId = string.Empty;
            }
            if (record.ProcedureUuid is "-")
            {
                record.ProcedureUuid = string.Empty;
            }
            if (record.ExceptionStack is "-")
            {
                record.ExceptionStack = string.Empty;
            }
            if (record.ExceptionMessage is "-")
            {
                record.ExceptionMessage = string.Empty;
            }
            if (record.LabelMethod is "-")
            {
                record.LabelMethod = string.Empty;
            }
            if (record.LabelMessage is "-")
            {
                record.LabelMessage = string.Empty;
            }
            if (record.ProcedureInstance is "-")
            {
                record.ProcedureInstance = string.Empty;
            }
            if (record.DaVinciSoftwareVersion is "-")
            {
                record.DaVinciSoftwareVersion = string.Empty;
            }
            if (record.MMAVersion is "-")
            {
                record.MMAVersion = string.Empty;
            }
            if (record.User is "-")
            {
                record.User = string.Empty;
            }
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
            if (DateTimeOffset.TryParseExact(timestamp, "MMM dd, yyyy @ HH:mm:ss.fffK", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTimeOffset dt))
            {
                return dt;
            }
            if (DateTimeOffset.TryParseExact(timestamp, "MMM d, yyyy @ HH:mm:ss.fffK", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out dt))
            {
                return dt;
            }
            return DateTimeOffset.UtcNow;
        }
    }

    public class ElasticRowRecord
    {
        public string Timestamp { get; set; } = "";
        public string MachineName { get; set; } = "";
        public string LogLevel { get; set; } = "";
        public string Text { get; set; } = "";
        public string MessageTemplate { get; set; } = "";
        public string LabelMessage { get; set; } = "";
        public string ExceptionMessage { get; set; } = "";
        public string ExceptionStack { get; set; } = "";
        public string User { get; set; } = "";
        public string ThreadId { get; set; } = "";
        public string ProcessId { get; set; } = "";
        public string ProcessName { get; set; } = "";
        public string LoggerName { get; set; } = "";
        public string LabelMethod { get; set; } = "";
        public string Line { get; set; } = "";
        public string ApplicationInstance { get; set; } = "";
        public string ProcedureInstance { get; set; } = "";
        public string SessionId { get; set; } = "";
        public string MetaSessionId { get; set; } = "";
        public string PowerCycleUuid { get; set; } = "";
        public string ProcedureUuid { get; set; } = "";
        public string PairingUuid { get; set; } = "";
        public string Msix { get; set; } = "";
        public string MMAVersion { get; set; } = "";
        public string DaVinciSoftwareVersion { get; set; } = "";
        public string SessionSummary { get; set; } = "";
    }

    public sealed class ElasticRowRecordMapper : ClassMap<ElasticRowRecord>
    {
        public ElasticRowRecordMapper()
        {
            Map(m => m.Timestamp).Optional().Name("@timestamp");
            Map(m => m.MachineName).Optional().Name("agent.name");
            Map(m => m.LogLevel).Optional().Name("log.level");
            Map(m => m.Text).Optional().Name("message");
            Map(m => m.LabelMessage).Optional().Name("labels.msg");
            Map(m => m.LabelMethod).Optional().Name("labels.method");
            Map(m => m.ExceptionMessage).Optional().Name("metadata.ExceptionDetail.Message");
            Map(m => m.User).Optional().Name("user.name");
            Map(m => m.ThreadId).Optional().Name("process.thread.id");
            Map(m => m.ProcessId).Optional().Name("process.pid");
            Map(m => m.ProcessName).Optional().Name("process.name");
            Map(m => m.LoggerName).Optional().Name("log.logger");
            Map(m => m.MessageTemplate).Optional().Name("labels.MessageTemplate");
            Map(m => m.ProcedureInstance).Optional().Name("metadata.ProcedureInstance");
            Map(m => m.ApplicationInstance).Optional().Name("metadata.ApplicationInstance");
            Map(m => m.ExceptionStack).Optional().Name("error.stack_trace");
            Map(m => m.SessionId).Optional().Name("labels.SessionId");
            Map(m => m.MetaSessionId).Optional().Name("metadata.SessionId");
            Map(m => m.PowerCycleUuid).Optional().Name("labels.PowerCycleUuid");
            Map(m => m.ProcedureUuid).Optional().Name("labels.ProcedureUuid");
            Map(m => m.PairingUuid).Optional().Name("labels.PairingUuid");
            Map(m => m.Msix).Optional().Name("labels.MsixVersion");
            Map(m => m.MMAVersion).Optional().Name("labels.MMAVersion");
            Map(m => m.DaVinciSoftwareVersion).Optional().Name("labels.DaVinciSoftwareVersion");
            Map(m => m.SessionSummary).Optional().Name("labels.SessionSummary");
        }
    }
}