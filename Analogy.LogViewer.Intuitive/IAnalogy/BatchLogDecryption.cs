using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.Interfaces.WinForms;
using Analogy.Interfaces.WinForms.DataTypes;
using Analogy.LogViewer.Intuitive.Properties;
using Analogy.LogViewer.Intuitive.UserControls;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analogy.LogViewer.Intuitive.IAnalogy
{
    internal sealed class BatchLogDecryption : IAnalogyCustomUserControlWinForms
    {
        public UserControl UserControl { get; set; } = null!;
        public Guid Id { get; set; } = new Guid("d7bea4a7-cf14-4427-abea-51a3fcf18818");
        public Image? SmallImage { get; set; } = Resources.LockTracking16x16;
        public Image? LargeImage { get; set; } = Resources.LockTracking32x32;
        public string Title { get; set; } = "Batch Decryption";
        public AnalogyToolTipWithImages? ToolTip { get; set; }

        public Task InitializeUserControl(Control hostingControl, ILogger logger)
        {
            UserControl = new BatchDecryptionUc();
            return Task.CompletedTask;
        }

        public Task UserControlRemoved()
        {
            return Task.CompletedTask;
        }
    }
}