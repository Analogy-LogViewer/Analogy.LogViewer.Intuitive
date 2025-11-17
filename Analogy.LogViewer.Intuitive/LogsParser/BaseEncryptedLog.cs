using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.LogViewer.Intuitive.Properties;
using Analogy.LogViewer.Template.WinForms;
using MediaManager.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Intuitive.LogsParser
{
    public abstract class BaseEncryptedLog : OfflineDataProviderWinForms
    {
        public override string? OptionalTitle { get; set; } = "Generic Encrypted logs";
        public override string? InitialFolderFullPath { get; set; } = Environment.CurrentDirectory;
        public override Image? LargeImage { get; set; } = Resources.Intuitive32x32OpenFile;
        public override Image? SmallImage { get; set; } = Resources.Intuitive16x16OpenFile;
        public override string FileOpenDialogFilters { get; set; } = "log files (*.log)|*.log|All files (*.*)|*.*";

        public override IEnumerable<string> SupportFormats { get; set; } = new[] { "*.log", "*.*" };

        protected EncryptionLogic EncryptionLogic { get; private set; } = null!;

        public override async Task InitializeDataProvider(ILogger logger)
        {
            EncryptionLogic = Container.Instance.EncryptionLogic;
            await base.InitializeDataProvider(logger);
        }

        public override async Task<IEnumerable<IAnalogyLogMessage>> Process(string fileName, CancellationToken token, ILogMessageCreatedHandler messagesHandler)
        {
            var msgs = new List<IAnalogyLogMessage>(0);
            if (CanOpenFile(fileName))
            {
                bool isKeyLine = false;

                using FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
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
                                    var logEntry = EncryptionLogic.Decrypt(encLine);
                                    if (string.IsNullOrEmpty(logEntry))
                                    {
                                        continue;
                                    }

                                    var entry = ParseMessage(logEntry);
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
                        catch (Exception ex)
                        {
                            msgs.Add(new AnalogyErrorMessage(ex.ToString()));
                        }
                    }
                }
            }
            return msgs;
        }

        protected abstract IAnalogyLogMessage ParseMessage(string logLine);
    }
}