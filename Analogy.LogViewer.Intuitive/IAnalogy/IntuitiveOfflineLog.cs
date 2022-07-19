using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Analogy.LogViewer.Intuitive.Properties;

namespace Analogy.LogViewer.Intuitive.IAnalogy
{
    public class IntuitiveOfflineLog : Analogy.LogViewer.Serilog.IAnalogy.OfflineDataProvider
    {
        public override string OptionalTitle { get; set; } = "Intuitive Serilog offline logs";
        public override Guid Id { get; set; } = new Guid("37E67AD9-109E-4E31-A9D7-F0C8D289DC08");
        public override string? InitialFolderFullPath { get; set; } = @"";
        public override Image LargeImage { get; set; } = Resources.Intuitive32x32OpenFile;
        public override Image SmallImage { get; set; } = Resources.Intuitive16x16OpenFile;
      
    }
}