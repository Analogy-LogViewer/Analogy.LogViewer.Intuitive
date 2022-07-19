using System;
using System.Drawing;
using Analogy.Interfaces;
using Analogy.LogViewer.gRPC.IAnalogy;
using Analogy.LogViewer.Intuitive.Properties;

namespace Analogy.LogViewer.Intuitive.IAnalogy
{
    public class IntuitiveOnlineLog : gRPCServerClient
    {
        public override string OptionalTitle { get; set; } = "Intuitive gRPC online logs";
        public override Guid Id { get; set; } = new Guid("E37EEA25-6CA3-40B2-8454-D53485887693");
        public override IAnalogyOfflineDataProvider FileOperationsHandler { get; set; } = new IntuitiveOfflineLog();
        public override Image ConnectedLargeImage { get; set; } = Resources.Kama32x32connected;
        public override Image ConnectedSmallImage { get; set; } = Resources.Kama16x16connected;
        public override Image DisconnectedLargeImage { get; set; } = Resources.Kama32x32disconnected;
        public override Image DisconnectedSmallImage { get; set; } = Resources.Kama16x16disconnected;

    }
}
