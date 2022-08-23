using Analogy.Interfaces.DataTypes;
using Analogy.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Intuitive
{
    public class LegacyFileLoader
    {
        private LegacyParser Parser { get; set; }

        public LegacyFileLoader()
        {
            Parser = new LegacyParser();
        }
        public async Task<IEnumerable<AnalogyLogMessage>> Process(string fileName, CancellationToken token, ILogMessageCreatedHandler messagesHandler)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                AnalogyLogMessage empty = new AnalogyLogMessage($"File is null or empty. Aborting.",
                    AnalogyLogLevel.Critical, AnalogyLogClass.General, "Analogy", "None")
                {
                    Source = "Analogy",
                    Module = System.Diagnostics.Process.GetCurrentProcess().ProcessName
                };
                messagesHandler.AppendMessage(empty, Utils.GetFileNameAsDataSource(fileName));
                return new List<AnalogyLogMessage> { empty };
            }
            //if (!_logFileSettings.IsConfigured)
            //{
            //    AnalogyLogMessage empty = new AnalogyLogMessage($"File Parser is not configured. Please set it first in the settings Window",
            //        AnalogyLogLevel.Critical, AnalogyLogClass.General, "Analogy", "None")
            //    {
            //        Source = "Analogy",
            //        Module = System.Diagnostics.Process.GetCurrentProcess().ProcessName
            //    };
            //    messagesHandler.AppendMessage(empty, Utils.GetFileNameAsDataSource(fileName));
            //    return new List<AnalogyLogMessage> { empty };
            //}
            //if (!_logFileSettings.CanOpenFile(fileName))
            //{
            //    AnalogyLogMessage empty = new AnalogyLogMessage($"File {fileName} Is not supported or not configured correctly in the windows settings",
            //        AnalogyLogLevel.Critical, AnalogyLogClass.General, "Analogy", "None")
            //    {
            //        Source = "Analogy",
            //        Module = System.Diagnostics.Process.GetCurrentProcess().ProcessName
            //    };
            //    messagesHandler.AppendMessage(empty, Utils.GetFileNameAsDataSource(fileName));
            //    return new List<AnalogyLogMessage> { empty };
            //}
            List<AnalogyLogMessage> messages = new List<AnalogyLogMessage>();
            try
            {
                AnalogyLogMessage entry = null;
                using (var stream = File.OpenRead(fileName))
                {
                    long count = 0;
                    using (var reader = new StreamReader(stream))
                    {
                        var line = await reader.ReadLineAsync();

                        while (!reader.EndOfStream)
                        {
                            var nextLine = await reader.ReadLineAsync();
                            var hasSeparators = Parser.splitters.Any(nextLine.Contains);
                            if (!hasSeparators) // handle multi-line messages
                            {
                                if (entry != null)
                                {
                                    entry.Text = entry.Text + Environment.NewLine + nextLine;
                                }
                                else
                                {
                                    line = line + Environment.NewLine + nextLine;
                                }
                            }
                            else
                            {
                                entry = Parser.Parse(line);
                                if (entry.Level == AnalogyLogLevel.Unknown)
                                {
                                    entry.Level = AnalogyLogLevel.Information;
                                }

                                if (entry.Text.StartsWith("\n\r"))
                                {
                                    entry.Text = entry.Text.Remove(0, 1);
                                }

                                if (entry.Text.StartsWith("Exception"))
                                {
                                    entry.Level = AnalogyLogLevel.Error;
                                }
                                messages.Add(entry);
                                line = nextLine;
                            }

                        }
                        var entry1 = Parser.Parse(line);
                        messages.Add(entry1);
                        count++;
                        messagesHandler.ReportFileReadProgress(new AnalogyFileReadProgress(AnalogyFileReadProgressType.Incremental, 1, count, count));
                    }
                }
                messagesHandler.AppendMessages(messages, fileName);
                return messages;
            }
            catch (Exception e)
            {
                AnalogyLogMessage empty = new AnalogyLogMessage($"Error occurred processing file {fileName}. Reason: {e.Message}",
                    AnalogyLogLevel.Critical, AnalogyLogClass.General, "Analogy", "None")
                {
                    Source = "Analogy",
                    Module = System.Diagnostics.Process.GetCurrentProcess().ProcessName
                };
                messagesHandler.AppendMessage(empty, Utils.GetFileNameAsDataSource(fileName));
                return new List<AnalogyLogMessage> { empty };
            }
        }
    }
}
