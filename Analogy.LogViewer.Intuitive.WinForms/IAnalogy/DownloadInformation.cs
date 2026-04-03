using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Versioning;

namespace Analogy.LogViewer.Intuitive.IAnalogy
{
    public class DownloadInformation : Analogy.LogViewer.Template.AnalogyDownloadInformation
    {
        protected override string RepositoryURL { get; set; } = "https://api.github.com/repos/Analogy-LogViewer/Analogy.LogViewer.Intuitive";
        public override TargetFrameworkAttribute CurrentFrameworkAttribute { get; set; } = (TargetFrameworkAttribute)Assembly.GetExecutingAssembly().GetCustomAttribute(typeof(TargetFrameworkAttribute))!;

        public override Guid FactoryId { get; set; } = IntuitiveFactories.Id;
        public override string Name { get; set; } = "Intuitive";

        private string? installedVersionNumber;
        public override string InstalledVersionNumber
        {
            get
            {
                if (installedVersionNumber is not null)
                {
                    return installedVersionNumber;
                }
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                installedVersionNumber = fvi.FileVersion;
                return installedVersionNumber!;
            }
        }
    }
}