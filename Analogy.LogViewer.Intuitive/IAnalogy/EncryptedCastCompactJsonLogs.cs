using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.LogViewer.Intuitive.Properties;
using Analogy.LogViewer.Serilog;
using Analogy.LogViewer.Serilog.DataTypes;
using Analogy.LogViewer.Template.Managers;
using Analogy.LogViewer.Template.WinForms;
using MediaManager.Logging;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using LogEventReader = Analogy.LogViewer.Intuitive.Serilog.LogEventReader;

namespace Analogy.LogViewer.Intuitive.IAnalogy
{
    public class EncryptedCastCompactJsonLogs : OfflineDataProviderWinForms
    {
        public override string? OptionalTitle { get; set; } = "Encrypted CAST Compact Json Formatter  logs";
        public override Guid Id { get; set; } = new Guid("519fc83b-17b7-4d48-b1f1-968b55e6f1be");
        public override string? InitialFolderFullPath { get; set; } = Environment.CurrentDirectory;
        public override Image? LargeImage { get; set; } = Resources.Intuitive32x32OpenFile;
        public override Image? SmallImage { get; set; } = Resources.Intuitive16x16OpenFile;
        public override string FileOpenDialogFilters { get; set; } = "log files (*.log)|*.log|All files (*.*)|*.*";

        public override IEnumerable<string> SupportFormats { get; set; } = new[] { "*.log", "*.*" };
        private CompactJsonFormatParser CompactJsonFormatParser { get; set; } = null!;
        private EncryptionLogic EncryptionLogic => Container.Instance.EncryptionLogic;

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
            string logEntry = string.Empty;
            long count = 0;
            var msgs = new List<AnalogyLogMessage>(0);
            if (CanOpenFile(fileName))
            {
                bool isKeyLine = false;
                using (FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    bool invalidKey = false;
                    using (StreamReader strReader = new StreamReader(fileStream, detectEncodingFromByteOrderMarks: false))
                    {
                        while (await strReader.ReadLineAsync() is { } encLine && !invalidKey)
                        {
                            try
                            {
                                if (encLine.Equals("#"))
                                {
                                    isKeyLine = true;
                                    continue;
                                }
                                if (isKeyLine)
                                {
                                    isKeyLine = false;
                                    var success = EncryptionLogic.CreateDecryptor(encLine);
                                    if (!success)
                                    {
                                        AnalogyErrorMessage err = new AnalogyErrorMessage(
                                            "Invalid decryption key. Please import correct keys via settings");
                                        messagesHandler.AppendMessage(err, "Analogy");
                                        msgs.Add(err);
                                        invalidKey = true;
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        logEntry = EncryptionLogic.Decrypt(encLine);
                                    }
                                    catch (Exception e)
                                    {
                                        AnalogyErrorMessage err = new AnalogyErrorMessage("Error Decrypting: " + e);
                                        messagesHandler.AppendMessage(err, "Analogy");
                                        msgs.Add(err);
                                    }
                                }

                                if (string.IsNullOrEmpty(logEntry))
                                {
                                    continue;
                                }

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