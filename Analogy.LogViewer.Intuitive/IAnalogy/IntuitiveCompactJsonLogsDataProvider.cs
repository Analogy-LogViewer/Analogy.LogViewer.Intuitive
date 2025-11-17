using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.LogViewer.Intuitive.Properties;
using Analogy.LogViewer.Serilog;
using Analogy.LogViewer.Serilog.DataTypes;
using Analogy.LogViewer.Template.Managers;
using Analogy.LogViewer.Template.WinForms;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Intuitive.IAnalogy
{
    public class IntuitiveCompactJsonLogsDataProvider : OfflineDataProviderWinForms
    {
        public override string? OptionalTitle { get; set; } = "CAST Compact Json Formatter logs";
        public override Guid Id { get; set; } = new Guid("287d85c5-33c5-4cdc-bd85-632854284a08");
        public override string? InitialFolderFullPath { get; set; } = Environment.CurrentDirectory;
        public override Image? LargeImage { get; set; } = Resources.Intuitive32x32OpenFile;
        public override Image? SmallImage { get; set; } = Resources.Intuitive16x16OpenFile;
        public override string FileOpenDialogFilters { get; set; } = "log files (*.log)|*.log|All files (*.*)|*.*";

        public override IEnumerable<string> SupportFormats { get; set; } = new[] { "*.log", "*.*" };
        private CompactJsonFormatParser CompactJsonFormatParser { get; set; } = null!;

        private CompactJsonFormatMessageFields CompactJsonFormatMessageFields { get; set; } = new CompactJsonFormatMessageFields();
        private StringWriter textWriter = new StringWriter();
        public override Task InitializeDataProvider(ILogger logger)
        {
            LogManager.Instance.SetLogger(logger);
            CompactJsonFormatParser = new CompactJsonFormatParser();
            return base.InitializeDataProvider(logger);
        }

        public override async Task<IEnumerable<IAnalogyLogMessage>> Process(string fileName, CancellationToken token,
            ILogMessageCreatedHandler messagesHandler)
        {
            long count = 0;
            var msgs = new List<AnalogyLogMessage>(0);
            if (CanOpenFile(fileName))
            {
                using (FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader strReader = new StreamReader(fileStream, detectEncodingFromByteOrderMarks: false))
                    {
                        while (await strReader.ReadLineAsync() is { } logEntry)
                        {
                            try
                            {
                                JObject obj = JsonConvert.DeserializeObject<JObject>(logEntry)!;
                                var evt = LogEventReader.ReadFromJObject(obj, CompactJsonFormatMessageFields);

                                evt.RenderMessage(textWriter);
                                await textWriter.FlushAsync();
                                var message = textWriter.GetStringBuilder().ToString();
                                textWriter.GetStringBuilder().Clear();

                                AnalogyLogMessage m = CommonParser.ParseLogEventProperties(evt);
                                m.Text = message;
                                m.RawText = logEntry;
                                m.RawTextType = AnalogyRowTextType.JSON;
                                msgs.Add(m);
                                count++;
                                messagesHandler.ReportFileReadProgress(new AnalogyFileReadProgress(AnalogyFileReadProgressType.Incremental, 1, count, count));
                                msgs.Add(m);
                                messagesHandler.AppendMessage(m, fileName);
                            }
                            catch (Exception ex)
                            {
                                var errorMsg = new AnalogyErrorMessage("Error parsing line: " + logEntry + "Exception: " + ex.ToString());
                                msgs.Add(errorMsg);
                                messagesHandler.AppendMessage(errorMsg, fileName);
                            }
                        }
                    }
                }
            }

            return msgs;
        }
    }
}