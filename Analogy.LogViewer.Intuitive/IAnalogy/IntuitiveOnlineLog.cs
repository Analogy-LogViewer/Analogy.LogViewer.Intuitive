using System;
using System.Drawing;
using Analogy.Interfaces;
using Analogy.LogViewer.gRPC.IAnalogy;
using Analogy.LogViewer.Intuitive.Properties;

namespace Analogy.LogViewer.Intuitive.IAnalogy
{
    public class IntuitiveOnlineLog : gRPCServerClient
    {
        public override string OptionalTitle { get; set; } = "Intuitive Real Time logs";
        public override Guid Id { get; set; } = new Guid("E37EEA25-6CA3-40B1-8454-D53485887693");
        public override IAnalogyOfflineDataProvider FileOperationsHandler { get; set; } = new IntuitiveOfflineLog();
        public override Image? ConnectedLargeImage { get; set; } = null;
        public override Image? ConnectedSmallImage { get; set; } = null;
        public override Image? DisconnectedLargeImage { get; set; } = null;
        public override Image? DisconnectedSmallImage { get; set; } = null;

    }
}
