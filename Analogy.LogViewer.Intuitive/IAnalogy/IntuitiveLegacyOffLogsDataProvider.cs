﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Analogy.LogViewer.Intuitive.Properties;
using Analogy.LogViewer.Template.Managers;

namespace Analogy.LogViewer.Intuitive.IAnalogy
{
    public class IntuitiveLegacyOffLogsDataProvider : Analogy.LogViewer.Template.OfflineDataProvider
    {
        public override string OptionalTitle { get; set; } = "CAST Legacy logs";
        public override Guid Id { get; set; } = new Guid("37E17AD9-109E-4E31-A9D7-F0C8D289DC08");
        public override string? InitialFolderFullPath { get; set; } = Environment.CurrentDirectory;
        public override Image LargeImage { get; set; } = Resources.Intuitive32x32OpenFile;
        public override Image SmallImage { get; set; } = Resources.Intuitive16x16OpenFile;
        public override string FileOpenDialogFilters { get; set; } = "log files (*.log)|*.log|All files (*.*)|*.*";

        public override IEnumerable<string> SupportFormats { get; set; } = new[] { "*.log", "*.*" };
        private LegacyFileLoader LegacyFileLoader { get; set; }
        public override Task InitializeDataProvider(IAnalogyLogger logger)
        {
            LogManager.Instance.SetLogger(logger);
            LegacyFileLoader = new LegacyFileLoader();
            return base.InitializeDataProvider(logger);
        }

        public override async Task<IEnumerable<AnalogyLogMessage>> Process(string fileName, CancellationToken token, ILogMessageCreatedHandler messagesHandler)
        {
            if (CanOpenFile(fileName))
            {
                return await LegacyFileLoader.Process(fileName, token, messagesHandler);
            }

            return new List<AnalogyLogMessage>(0);

        }

    }
}