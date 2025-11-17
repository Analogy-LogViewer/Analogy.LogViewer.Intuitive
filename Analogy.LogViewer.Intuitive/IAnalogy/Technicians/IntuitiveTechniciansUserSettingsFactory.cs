using Analogy.Interfaces;
using Analogy.LogViewer.Intuitive.Properties;
using Analogy.LogViewer.Intuitive.UserControls;
using Analogy.LogViewer.Template.WinForms;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analogy.LogViewer.Intuitive.IAnalogy.Technicians
{
    public class IntuitiveTechniciansUserSettingsFactory : TemplateUserSettingsFactoryWinForms
    {
        public override Guid FactoryId { get; set; } = IntuitiveTechniciansFactories.Id;
        public override Guid Id { get; set; } = new Guid("006c1f9b-6b27-4c42-ab03-77d0b514fc25");
        public override UserControl DataProviderSettings { get; set; } = null!;
        public override Image? SmallImage { get; set; } = Resources.Intuitive16x16Settings;
        public override Image? LargeImage { get; set; } = Resources.Intuitive32x32Settings;
        public override string Title { get; set; } = "Intuitive Settings";

        public IntuitiveTechniciansUserSettingsFactory()
        {
        }

        public override void CreateUserControl(ILogger logger)
        {
            DataProviderSettings = new UserSettingsUc();
        }

        public override Task SaveSettingsAsync()
        {
            return Task.CompletedTask;
        }
    }
}