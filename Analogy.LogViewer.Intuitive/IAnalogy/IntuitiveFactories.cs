#pragma warning disable SA1401
using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.LogViewer.Intuitive.LogsParser;
using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.Intuitive.IAnalogy
{
    public class IntuitiveFactories : Analogy.LogViewer.Template.PrimaryFactory
    {
        internal static Guid Id = new Guid("D37AEA25-6CA3-40B2-8454-D13485887693");
        public override Guid FactoryId { get; set; } = Id;
        public override string Title { get; set; } = "Intuitive DEV";
        public override IEnumerable<IAnalogyChangeLog> ChangeLog { get; set; } = GetChangeLog();
        public override IEnumerable<string> Contributors { get; set; } = new List<string> { "Lior Banai" };
        public override string About { get; set; } = "Intuitive Log Parsers";
        private static IEnumerable<IAnalogyChangeLog> GetChangeLog()
        {
#pragma warning disable RS0030
            yield return new AnalogyChangeLog("Initial version", AnalogChangeLogType.None, "Lior Banai", new DateTime(2022, 08, 15), "1.0.0.0");
            yield return new AnalogyChangeLog("Disable Updates", AnalogChangeLogType.None, "Lior Banai", new DateTime(2023, 09, 05), "1.0.2.0");
            yield return new AnalogyChangeLog("Create self-contained MSI", AnalogChangeLogType.None, "Lior Banai", new DateTime(2023, 09, 05), "1.0.2.0");
            yield return new AnalogyChangeLog("[Simulator] Add VideoTriggerOn / VideoTriggerOff To Analogy", AnalogChangeLogType.None, "Lior Banai", new DateTime(2023, 09, 21), "1.0.4.1");
            yield return new AnalogyChangeLog("[Simulator] Start Zmq Server Inside Analogy", AnalogChangeLogType.None, "Lior Banai", new DateTime(2023, 09, 22), "1.0.5.0");
#pragma warning restore RS0030
        }
    }

#pragma warning disable MA0048
    public class DataSourceFactory : Analogy.LogViewer.Template.DataProvidersFactory
#pragma warning restore MA0048
    {
        public override Guid FactoryId { get; set; } = IntuitiveFactories.Id;
        public override string Title { get; set; } = "Intuitive Surgical";
        public sealed override IEnumerable<IAnalogyDataProvider> DataProviders { get; set; }

        public DataSourceFactory()
        {
            var p1 = new NonStructuredCombinedEncryptedAndNonEncryptedLog(new Guid("7957ed73-7ceb-4ae8-98ea-1e308afa57cb"));
            var p2 = new V2CombinedEncryptedAndNonEncryptedLog(new Guid("8d4b45a9-f52d-4bb3-89c6-d23880188ca6"));
            var p3 = new V4CombinedEncryptedAndNonEncryptedLog(new Guid("95be4367-d978-446b-ba74-41df8332709f"));
            var p4 = new EcsCombinedEncryptedAndNonEncryptedLog(new Guid("91819680-6e6a-4f6f-abee-3486174353ba"));
            var p5 = new ElasticCsvDataProvider();
            var p6 = new LightHouseNodeTraceParser();
            var p7 = new LightHouseCsvEventsParser();
            var p8 = new LightHouseJsonEventsParser();
            DataProviders = new List<IAnalogyDataProvider>
            {
                p1,
                p2,
                p3,
                p4,
                p5,
                p6,
                p7,
                p8,
            };
        }
    }
}