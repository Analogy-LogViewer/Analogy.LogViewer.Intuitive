using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.LogViewer.ElasticCommonSchema.Parsers;
using Analogy.LogViewer.Intuitive.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Intuitive.Loaders
{
    public class EcsTextFileLoader
    {
        public EcsTextFileLoader()
        {
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
                await using var stream = File.OpenRead(fileName);
                long count = 0;
                using var reader = new StreamReader(stream);
                while (!reader.EndOfStream)
                {
                    var logLine = await reader.ReadLineAsync(token);
                    var message = EcsDocumentUtils.ParseLine(logLine!,
                        UserSettingsManager.Instance.ShowAllColumnsFromMetaDataField,
                        UserSettingsManager.Instance.AdditionalColumnsFromMetaDataField);
                    messages.Add(message);
                    count++;
                    messagesHandler.AppendMessage(message, fileName);
                    messagesHandler.ReportFileReadProgress(
                        new AnalogyFileReadProgress(AnalogyFileReadProgressType.Incremental, 1, count, count));
                }
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