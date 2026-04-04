using Analogy.LogViewer.Template;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Intuitive.IAnalogy
{
    public class IntuitiveUserSettingsFactory : TemplateUserSettingsFactory
    {
        public override Guid FactoryId { get; set; } = IntuitiveFactories.Id;
        public override Guid Id { get; set; } = new Guid("006c1f9b-6b27-4c42-ab03-77d0b514fc25");
        public override string Title { get; set; } = "Intuitive Settings";

        public IntuitiveUserSettingsFactory()
        {
        }

        public override void CreateUserControl(ILogger logger)
        {
        }

        public override Task SaveSettingsAsync()
        {
            return Task.CompletedTask;
        }
    }
}