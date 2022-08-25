using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Analogy.LogViewer.Intuitive.ECS;
using Analogy.LogViewer.Template;
using Analogy.LogViewer.Template.Managers;
using Newtonsoft.Json;

namespace Analogy.LogViewer.Intuitive.IAnalogy
{
    internal class IntuitiveECSDataProvider : Analogy.LogViewer.Template.OfflineDataProvider
    {
        public override Guid Id { get; set; } = new Guid("D89316C6-306A-48D8-90A0-7C2C49EFDA82");
        public override Image LargeImage { get; set; } = null;
        public override Image SmallImage { get; set; } = null;
        public override string OptionalTitle { get; set; } = "ECS offline reader";
        public override bool CanSaveToLogFile { get; set; } = false;
        public override string FileOpenDialogFilters { get; set; } = "log files (*.log)|*.log|All files (*.*)|*.*";
        public override string FileSaveDialogFilters { get; set; } = string.Empty;
        public override IEnumerable<string> SupportFormats { get; set; } = new[] { "*.log", "*.*" };
        public override bool DisableFilePoolingOption { get; set; } = false;

        public override string InitialFolderFullPath => Directory.Exists(@"C:\MVD2\Logs")
                ? @"C:\MVD2\Logs"
                : Environment.CurrentDirectory;
        private JsonFormatterParser JsonPerLineParser { get; }

        public override bool UseCustomColors { get; set; } = false;
        public override IEnumerable<(string originalHeader, string replacementHeader)> GetReplacementHeaders()
            => Array.Empty<(string, string)>();

        public override (Color backgroundColor, Color foregroundColor) GetColorForMessage(IAnalogyLogMessage logMessage)
            => (Color.Empty, Color.Empty);
        public IntuitiveECSDataProvider()
        {
            JsonPerLineParser = new JsonFormatterParser(new ECSFormatMessageFields());
        }
        public override Task InitializeDataProviderAsync(IAnalogyLogger logger)
        {
            LogManager.Instance.SetLogger(logger);
            return base.InitializeDataProviderAsync(logger);
        }

        public override async Task<IEnumerable<AnalogyLogMessage>> Process(string fileName, CancellationToken token, ILogMessageCreatedHandler messagesHandler)
        {
            if (CanOpenFile(fileName))
            {
                   return await JsonPerLineParser.Process(fileName, token, messagesHandler);
            }
            LogManager.Instance.LogError($"Unsupported File {fileName}", nameof(OfflineDataProvider));
            return new List<AnalogyLogMessage>(0);
        }

        public override bool CanOpenFile(string fileName)
        {
            foreach (string pattern in SupportFormats)
            {
                if (CommonUtilities.Files.FilesPatternMatcher.StrictMatchPattern(pattern, fileName))
                {
                    return true;
                }
            }
            return false;
        }

        protected override List<FileInfo> GetSupportedFilesInternal(DirectoryInfo dirInfo, bool recursive)
        {
            List<FileInfo> files = new List<FileInfo>();
            foreach (string pattern in SupportFormats)
            {
                files.AddRange(dirInfo.GetFiles(pattern).ToList());
            }

            if (!recursive)
            {
                return files;
            }

            try
            {
                foreach (DirectoryInfo dir in dirInfo.GetDirectories())
                {
                    files.AddRange(GetSupportedFilesInternal(dir, true));
                }
            }
            catch (Exception)
            {
                return files;
            }

            return files;
        }

        private static string SafeReadAllLines(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(stream))
            {
                string data = sr.ReadToEnd();
                return data;
            }
        }
    }
}