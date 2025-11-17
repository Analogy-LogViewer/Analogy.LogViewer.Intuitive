using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.LogViewer.Intuitive.Loaders;
using Analogy.LogViewer.Intuitive.Properties;
using Analogy.LogViewer.Template.Managers;
using Analogy.LogViewer.Template.WinForms;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Intuitive.IAnalogy
{
    public class V4NonEncryptedLogs : OfflineDataProviderWinForms
    {
        public override string? OptionalTitle { get; set; } = "Media Manager App V4.X Non Encrypted logs";
        public override Guid Id { get; set; } = new Guid("5c7f024f-ea18-45f6-b911-c40d03e5ab63");
        public override string? InitialFolderFullPath { get; set; } = Environment.CurrentDirectory;
        public override Image? LargeImage { get; set; } = Resources.Intuitive32x32OpenFile;
        public override Image? SmallImage { get; set; } = Resources.Intuitive16x16OpenFile;
        public override string FileOpenDialogFilters { get; set; } = "log files (*.log)|*.log|All files (*.*)|*.*";

        public override IEnumerable<string> SupportFormats { get; set; } = new[] { "*.log", "*.*" };
        private EcsTextFileLoader ECSTextFileLoader { get; set; } = null!;
        public override Task InitializeDataProvider(ILogger logger)
        {
            LogManager.Instance.SetLogger(logger);
            ECSTextFileLoader = new EcsTextFileLoader();
            return base.InitializeDataProvider(logger);
        }

        public override async Task<IEnumerable<IAnalogyLogMessage>> Process(string fileName, CancellationToken token, ILogMessageCreatedHandler messagesHandler)
        {
            if (CanOpenFile(fileName))
            {
                return await ECSTextFileLoader.Process(fileName, token, messagesHandler);
            }

            return new List<AnalogyLogMessage>(0);
        }
    }
}