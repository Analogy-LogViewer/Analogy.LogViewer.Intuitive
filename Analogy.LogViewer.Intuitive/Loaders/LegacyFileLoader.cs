#pragma warning disable CS8604 // Possible null reference argument.
using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Intuitive.Loaders
{
    public class LegacyFileLoader
    {
        private LegacyParser Parser { get; set; }

        public LegacyFileLoader()
        {
            Parser = new LegacyParser();
        }
        public async Task<IEnumerable<IAnalogyLogMessage>> Process(string fileName, CancellationToken token, ILogMessageCreatedHandler messagesHandler)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                AnalogyLogMessage empty = new AnalogyLogMessage($"File is null or empty. Aborting.",
                    AnalogyLogLevel.Critical, AnalogyLogClass.General, "Analogy", "None")
                {
                    Source = "Analogy",
                    Module = System.Diagnostics.Process.GetCurrentProcess().ProcessName,
                };
                messagesHandler.AppendMessage(empty, Utils.GetFileNameAsDataSource(fileName));
                return new List<IAnalogyLogMessage> { empty };
            }

            List<IAnalogyLogMessage> messages = new List<IAnalogyLogMessage>();
            try
            {
                AnalogyLogMessage? entry = null;
                await using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    long count = 0;
                    using (var reader = new StreamReader(stream))
                    {
                        var line = await reader.ReadLineAsync();

                        while (!token.IsCancellationRequested && !reader.EndOfStream)
                        {
                            var nextLine = await reader.ReadLineAsync(token);
                            var hasSeparators = nextLine != null && Parser.Splitters.Any(nextLine.Contains);
                            if (!hasSeparators) // handle multi-line messages
                            {
                                if (entry is not null)
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
                                if (entry.Level is AnalogyLogLevel.Unknown)
                                {
                                    entry.Level = AnalogyLogLevel.Information;
                                }

                                if (entry.Text != null && entry.Text.StartsWith("\n\r", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    entry.Text = entry.Text.Remove(0, 1);
                                }

                                if (entry.Text != null && (entry.Text.StartsWith("Exception", StringComparison.InvariantCultureIgnoreCase) || entry.Text.StartsWith("*** exception", StringComparison.InvariantCultureIgnoreCase) ||
                                                           entry.Text.Contains("Exception:")))
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
                    Module = System.Diagnostics.Process.GetCurrentProcess().ProcessName,
                };
                messagesHandler.AppendMessage(empty, Utils.GetFileNameAsDataSource(fileName));
                return new List<AnalogyLogMessage> { empty };
            }
        }
    }
}