using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.LogViewer.Template;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Intuitive.LogsParser
{
    public class LightHouseJsonEventsParser : OfflineDataProvider
    {
        public override string? OptionalTitle { get; set; } = "LightHouse Events JSON Log";
        public override string? InitialFolderFullPath { get; set; } = Environment.CurrentDirectory;
        public override string FileOpenDialogFilters { get; set; } = "LightHouse event log files (*.json)|*.json";
        public override Guid Id { get; set; } = new Guid("ce31b4bb-6489-4a16-ad0c-8ca857287b6d");

        public override IEnumerable<string> SupportFormats { get; set; } = new[] { "*.json" };

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
                try
                {
                    var json = await File.ReadAllTextAsync(fileName, token);
                    using var doc = JsonDocument.Parse(json);
                    if (doc.RootElement.ValueKind is not JsonValueKind.Array)
                    {
                        var err = new AnalogyErrorMessage("Invalid JSON format. Expected root array of event entries.");
                        messagesHandler.AppendMessage(err, "Analogy");
                        msgs.Add(err);
                        return msgs;
                    }

                    foreach (var record in doc.RootElement.EnumerateArray())
                    {
                        token.ThrowIfCancellationRequested();
                        try
                        {
                            var entry = ParseMessage(record);
                            entry.RawText = record.GetRawText();
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
                catch (Exception e)
                {
                    var err = new AnalogyErrorMessage("Error Decrypting: " + e);
                    messagesHandler.AppendMessage(err, "Analogy");
                    msgs.Add(err);
                }
            }

            return msgs;
        }

        private static IAnalogyLogMessage ParseMessage(JsonElement record)
        {
            string eventLabel = GetString(record, "evt_label");
            string eventText = GetString(record, "evt_text");
            string source = GetNestedString(record, "record_header", "node_name");
            if (string.IsNullOrEmpty(source))
            {
                source = GetNestedString(record, "record_header", "node_id");
            }

            string text = string.IsNullOrWhiteSpace(eventText) ? eventLabel : eventText;
            var message = new AnalogyLogMessage()
            {
                Text = text,
                Source = source,
                Module = eventLabel,
                Level = ParseLevel(eventLabel, eventText),
                Date = ParseDateTime(record),
                RawTextType = AnalogyRowTextType.PlainText,
            };

            AddIfNotEmpty(message, "Id", GetString(record, "id"));
            AddIfNotEmpty(message, "Index", GetString(record, "index"));
            AddIfNotEmpty(message, "PowerCycleId", GetString(record, "power_cycle_id"));
            AddIfNotEmpty(message, "NodeId", GetNestedString(record, "record_header", "node_id"));
            AddIfNotEmpty(message, "NodeName", GetNestedString(record, "record_header", "node_name"));
            AddIfNotEmpty(message, "MscSystemState", GetNestedString(record, "record_header", "msc_system_state"));
            AddIfNotEmpty(message, "MscProcedureState", GetNestedString(record, "record_header", "msc_procedure_state"));

            AddEventEntryProperties(message, record);

            return message;
        }

        private static void AddEventEntryProperties(AnalogyLogMessage message, JsonElement record)
        {
            if (!record.TryGetProperty("event_entry", out var eventEntry) || eventEntry.ValueKind is not JsonValueKind.Object)
            {
                return;
            }

            foreach (var eventType in eventEntry.EnumerateObject())
            {
                AddIfNotEmpty(message, "EventEntryType", eventType.Name);
                if (eventType.Value.ValueKind is not JsonValueKind.Object)
                {
                    continue;
                }

                AddIfNotEmpty(message, "ErrorName", GetString(eventType.Value, "error_name"));
                AddIfNotEmpty(message, "ErrorCode", GetString(eventType.Value, "error_code"));
                AddIfNotEmpty(message, "ErrorClass", GetString(eventType.Value, "error_class"));
                AddIfNotEmpty(message, "FileName", GetString(eventType.Value, "file_name"));
                AddIfNotEmpty(message, "LineNumber", GetString(eventType.Value, "line_number"));
                AddIfNotEmpty(message, "LocalTimestamp", GetString(eventType.Value, "local_timestamp"));
                break;
            }
        }

        private static string GetNestedString(JsonElement element, string firstName, string secondName)
        {
            if (element.TryGetProperty(firstName, out var first) && first.ValueKind is JsonValueKind.Object)
            {
                return GetString(first, secondName);
            }

            return string.Empty;
        }

        private static string GetString(JsonElement element, string propertyName)
        {
            if (!element.TryGetProperty(propertyName, out var value))
            {
                return string.Empty;
            }

            return value.ValueKind switch
            {
                JsonValueKind.String => value.GetString() ?? string.Empty,
                JsonValueKind.Number => value.ToString(),
                JsonValueKind.True => bool.TrueString,
                JsonValueKind.False => bool.FalseString,
                _ => string.Empty,
            };
        }

        private static AnalogyLogLevel ParseLevel(string eventLabel, string eventText)
        {
            if ((!string.IsNullOrEmpty(eventLabel) && eventLabel.Contains("error", StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(eventText) && eventText.Contains("ERR", StringComparison.OrdinalIgnoreCase)))
            {
                return AnalogyLogLevel.Error;
            }

            if ((!string.IsNullOrEmpty(eventLabel) && eventLabel.Contains("warn", StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(eventText) && eventText.Contains("WARN", StringComparison.OrdinalIgnoreCase)))
            {
                return AnalogyLogLevel.Warning;
            }

            return AnalogyLogLevel.Information;
        }

        private static DateTimeOffset ParseDateTime(JsonElement record)
        {
            if (record.TryGetProperty("record_header", out var header) && header.ValueKind is JsonValueKind.Object)
            {
                if (header.TryGetProperty("gmt_timestamp_sec", out var secondsEl) && secondsEl.ValueKind is JsonValueKind.Number && secondsEl.TryGetInt64(out var seconds))
                {
                    var dateTime = DateTimeOffset.FromUnixTimeSeconds(seconds);
                    if (header.TryGetProperty("gmt_timestamp_usec", out var usecEl) && usecEl.ValueKind is JsonValueKind.Number && usecEl.TryGetInt64(out var usec))
                    {
                        return dateTime.AddTicks(usec * 10);
                    }

                    return dateTime;
                }

                if (header.TryGetProperty("gmt_timestamp", out var timestamp) && timestamp.ValueKind is JsonValueKind.Number && timestamp.TryGetDouble(out var epochSeconds))
                {
                    return DateTimeOffset.FromUnixTimeMilliseconds((long)(epochSeconds * 1000));
                }
            }

            return DateTimeOffset.UtcNow;
        }

        private static void AddIfNotEmpty(AnalogyLogMessage msg, string key, string value)
        {
            if (string.IsNullOrEmpty(value) || value is "-")
            {
                return;
            }

            msg.AddOrReplaceAdditionalProperty(key, value, StringComparer.OrdinalIgnoreCase);
        }
    }
}