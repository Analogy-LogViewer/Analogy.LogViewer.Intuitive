using System;
using System.Collections.Generic;
using System.Drawing;
using Analogy.LogViewer.Intuitive.Properties;

namespace Analogy.LogViewer.Intuitive.IAnalogy
{
    public class IntuitiveUserSettingsFactory : Analogy.LogViewer.Serilog.IAnalogy.UserSettingsFactory
    {
        public override Guid FactoryId { get; set; } = IntuitiveFactories.Id;
        public override Guid Id { get; set; } = new Guid("006c1f9b-6b27-4c42-ab03-77d0a514fc25");
        public override Image SmallImage { get; set; } = Resources.Kama16x16Settings;
        public override Image LargeImage { get; set; } = Resources.Kama32x32Settings;
        public override string Title { get; set; } = "Intuitive Settings";

        public IntuitiveUserSettingsFactory()
        {
          
        }
    }
}
