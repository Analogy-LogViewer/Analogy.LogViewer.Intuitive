using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Analogy.LogViewer.Intuitive.Properties;
using Analogy.LogViewer.Serilog.Managers;

namespace Analogy.LogViewer.Intuitive.IAnalogy
{
    public class IntuitiveLegacyOfflineLog : Analogy.LogViewer.Serilog.IAnalogy.OfflineDataProvider
    {
        public override string OptionalTitle { get; set; } = "Intuitive Legacy offline logs";
        public override Guid Id { get; set; } = new Guid("37E17AD9-109E-4E31-A9D7-F0C8D289DC08");
        public override string? InitialFolderFullPath { get; set; } = @"";
        public override Image LargeImage { get; set; } = Resources.Intuitive32x32OpenFile;
        public override Image SmallImage { get; set; } = Resources.Intuitive16x16OpenFile;

        private LegacyFileLoader LegacyFileLoader { get; set; }
        public override Task InitializeDataProviderAsync(IAnalogyLogger logger)
        {
            LogManager.Instance.SetLogger(logger);
            LegacyFileLoader = new LegacyFileLoader();
            return base.InitializeDataProviderAsync(logger);
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