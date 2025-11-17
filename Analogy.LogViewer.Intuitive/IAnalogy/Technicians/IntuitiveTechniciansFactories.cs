#pragma warning disable SA1401
#pragma warning disable MA0048
#pragma warning disable RS0030

using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.LogViewer.Intuitive.LogsParser;
using Analogy.LogViewer.Intuitive.Properties;
using Analogy.LogViewer.Template;
using Analogy.LogViewer.Template.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Analogy.LogViewer.Intuitive.IAnalogy.Technicians
{
    public class IntuitiveTechniciansFactories : PrimaryFactoryWinForms
    {
        internal static Guid Id = new Guid("12a01191-ec1d-4a77-a8af-a3c0a05c6a5b");
        public override Guid FactoryId { get; set; } = Id;
        public override string Title { get; set; } = "Intuitive Technicians";
        public override Image? SmallImage { get; set; } = Resources.Intuitive16x16;
        public override Image? LargeImage { get; set; } = Resources.Intuitive32x32;
        public override IEnumerable<IAnalogyChangeLog> ChangeLog { get; set; } = GetChangeLog();
        public override IEnumerable<string> Contributors { get; set; } = new List<string> { "Lior Banai" };
        public override string About { get; set; } = "Intuitive Technicians";

        private static IEnumerable<IAnalogyChangeLog> GetChangeLog()
        {
            yield return new AnalogyChangeLog("Initial version", AnalogChangeLogType.None, "Lior Banai", new DateTime(2022, 08, 15), "1.0.0.0");
            yield return new AnalogyChangeLog("Disable Updates", AnalogChangeLogType.None, "Lior Banai", new DateTime(2023, 09, 05), "1.0.2.0");
            yield return new AnalogyChangeLog("Create self-contained MSI", AnalogChangeLogType.None, "Lior Banai", new DateTime(2023, 09, 05), "1.0.2.0");
            yield return new AnalogyChangeLog("[Simulator] Add VideoTriggerOn / VideoTriggerOff To Analogy", AnalogChangeLogType.None, "Lior Banai", new DateTime(2023, 09, 21), "1.0.4.1");
            yield return new AnalogyChangeLog("[Simulator] Start Zmq Server Inside Analogy", AnalogChangeLogType.None, "Lior Banai", new DateTime(2023, 09, 22), "1.0.5.0");
        }
    }
    public class DataSourceTechniciansFactory : DataProvidersFactoryWinForms
    {
        public override Guid FactoryId { get; set; } = IntuitiveTechniciansFactories.Id;
        public override string Title { get; set; } = "Intuitive Technicians";

        public override IEnumerable<IAnalogyDataProvider> DataProviders { get; set; } = new List<IAnalogyDataProvider>
        {
            new NonStructuredCombinedEncryptedAndNonEncryptedLog(new Guid("db726462-681a-46e8-a2d0-77a3c430fc4e")),
            new V2CombinedEncryptedAndNonEncryptedLog(new Guid("313622b4-9201-478d-9a7c-b8dd70351b69")),
            new V4CombinedEncryptedAndNonEncryptedLog(new Guid("9b8d2610-c2b3-4fe1-a479-fd24d936adbe")),
            new EcsCombinedEncryptedAndNonEncryptedLog(new Guid("3636b031-41c8-4909-b1cb-3a6fbd9b49e9")),
        };
    }
}