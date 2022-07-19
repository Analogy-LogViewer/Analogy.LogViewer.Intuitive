using System;
using System.Collections.Generic;
using System.Drawing;
using Analogy.Interfaces;
using Analogy.LogViewer.Intuitive.Properties;
using Analogy.LogViewer.Template;

namespace Analogy.LogViewer.Intuitive.IAnalogy
{
    public class IntuitiveFactories : PrimaryFactory
    {
        internal static Guid Id = new Guid("D37AEA25-6CA3-40B2-8454-D53485887693");
        public override Guid FactoryId { get; set; } = Id;
        public override string Title { get; set; } = "Intuitive";
        public override Image SmallImage { get; set; } = Resources.Kama16x16;
        public override Image LargeImage { get; set; } = Resources.Kama32x32;
        public override IEnumerable<IAnalogyChangeLog> ChangeLog { get; set; } = GetChangeLog();
        public override IEnumerable<string> Contributors { get; set; } = new List<string> { "Lior Banai" };
        public override string About { get; set; } = "Kama Research Analogy Implementation";

        private static IEnumerable<IAnalogyChangeLog> GetChangeLog()
        {
            yield return new AnalogyChangeLog("Initial version", AnalogChangeLogType.None, "Lior Banai", new DateTime(2019, 09, 01));
        }
    }

    public class DataSourceFactory : DataProvidersFactory
    {
        public override Guid FactoryId { get; set; } = IntuitiveFactories.Id;
        public override string Title { get; set; } = "Kama Research Logs";

        public override IEnumerable<IAnalogyDataProvider> DataProviders { get; set; } = new List<IAnalogyDataProvider>
            {
               new IntuitiveOnlineLog(),
               new IntuitiveOfflineLog()
            };
    }
}
