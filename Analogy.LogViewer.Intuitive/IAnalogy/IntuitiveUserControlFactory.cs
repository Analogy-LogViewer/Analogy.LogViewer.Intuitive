using Analogy.Interfaces.WinForms;
using Analogy.Interfaces.WinForms.Factories;
using System;
using System.Collections.Generic;

namespace Analogy.LogViewer.Intuitive.IAnalogy
{
    public class IntuitiveUserControlFactory : IAnalogyCustomUserControlsFactoryWinForms
    {
        public Guid FactoryId { get; set; } = IntuitiveFactories.Id;
        public string Title { get; set; } = "Intuitive Developers Tools";

        public IEnumerable<IAnalogyCustomUserControlWinForms> UserControls { get; set; } = new List<IAnalogyCustomUserControlWinForms>
        {
            new BatchLogDecryption(),
        };
    }
}