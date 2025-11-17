using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using Analogy.Interfaces.WinForms;
using Analogy.Interfaces.WinForms.Factories;
using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.Intuitive.IAnalogy.Technicians
{
    public class IntuitiveTechniciansUserControlFactory : IAnalogyCustomUserControlsFactoryWinForms
    {
        public Guid FactoryId { get; set; } = IntuitiveTechniciansFactories.Id;
        public string Title { get; set; } = "Intuitive Technicians Tools";

        public IEnumerable<IAnalogyCustomUserControlWinForms> UserControls { get; } = new List<IAnalogyCustomUserControlWinForms>
        {
            new BatchLogDecryption(),
        };
    }
}