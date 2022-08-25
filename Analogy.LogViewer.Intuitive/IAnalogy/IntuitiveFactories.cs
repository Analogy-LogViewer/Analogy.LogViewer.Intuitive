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
        internal static Guid Id = new Guid("D37AEA25-6CA3-40B2-8454-D13485887693");
        public override Guid FactoryId { get; set; } = Id;
        public override string Title { get; set; } = "Intuitive";
        public override Image SmallImage { get; set; } = Resources.Intuitive16x16;
        public override Image LargeImage { get; set; } = Resources.Intuitive32x32;
        public override IEnumerable<IAnalogyChangeLog> ChangeLog { get; set; } = GetChangeLog();
        public override IEnumerable<string> Contributors { get; set; } = new List<string> { "Lior Banai" };
        public override string About { get; set; } = "Intuitive Log Parsers";

        private static IEnumerable<IAnalogyChangeLog> GetChangeLog()
        {
            yield return new AnalogyChangeLog("Initial version", AnalogChangeLogType.None, "Lior Banai", new DateTime(2022, 07, 15));
        }
    }

    public class DataSourceFactory : DataProvidersFactory
    {
        public override Guid FactoryId { get; set; } = IntuitiveFactories.Id;
        public override string Title { get; set; } = "Intuitive Logs";

        public override IEnumerable<IAnalogyDataProvider> DataProviders { get; set; } = new List<IAnalogyDataProvider>
            {
               new IntuitiveECSDataProvider(),
               new IntuitiveLegacyOffLogsDataProvider()
            };
    }
}
