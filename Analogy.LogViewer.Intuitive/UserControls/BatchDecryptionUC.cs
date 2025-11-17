#pragma warning disable SA1300 // Element should begin with upper-case letter
using Analogy.Interfaces;
using MediaManager.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analogy.LogViewer.Intuitive.UserControls
{
    public partial class BatchDecryptionUc : UserControl
    {
        private EncryptionLogic EncryptionLogic { get; set; } = null!;

        public BatchDecryptionUc()
        {
            InitializeComponent();
        }

        private void tnSelectFolder_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog(); // Show the dialog.
                if (result is DialogResult.OK) // Test result.
                {
                    txtbLogFolder.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private async void btnDycrpt_Click(object sender, EventArgs e)
        {
            if (Path.Exists(txtbLogFolder.Text))
            {
                var files = Directory.GetFiles(txtbLogFolder.Text, "*.log");
                if (files.Length is not 0)
                {
                    foreach (string log in files)
                    {
                        var result = await ProcessFile(log);
                        if (result.Success)
                        {
                            richTextBox1.Text += $"File {log} was decrypted and saved to {result.Result}{Environment.NewLine}";
                        }
                        else
                        {
                            richTextBox1.Text += $"Error dycrypting File {log}. Error: {result.Result}{Environment.NewLine}";
                        }
                    }
                }
                else
                {
                    richTextBox1.Text = "No log files in folder " + txtbLogFolder.Text;
                }
            }
            else
            {
                richTextBox1.Text = "Missing folder";
            }
        }

        private void BatchDecryptionsUC_Load(object sender, EventArgs e)
        {
            EncryptionLogic = Intuitive.Container.Instance.EncryptionLogic;
        }

        private async Task<(bool Success, string Result)> ProcessFile(string fileName)
        {
            bool isKeyLine = false;
            string fileDecrypted = Path.Combine(Path.GetDirectoryName(fileName)!,
                Path.GetFileNameWithoutExtension(fileName) + "_decrypted.log");
            StringBuilder sb = new StringBuilder();
            using (FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                bool invalidKey = false;
                using (StreamReader strReader = new StreamReader(fileStream, detectEncodingFromByteOrderMarks: false))
                {
                    while (await strReader.ReadLineAsync() is { } encLine && !invalidKey)
                    {
                        try
                        {
                            if (encLine.Equals("#"))
                            {
                                isKeyLine = true;
                                continue;
                            }

                            if (isKeyLine)
                            {
                                isKeyLine = false;
                                var success = EncryptionLogic.CreateDecryptor(encLine);
                                if (!success)
                                {
                                    return (false, fileDecrypted);
                                }
                            }
                            else
                            {
                                try
                                {
                                    var logEntry = EncryptionLogic.Decrypt(encLine);
                                    sb.Append(logEntry);
                                }
                                catch (Exception e)
                                {
                                    sb.AppendLine(e.Message);
                                    return (false, e.Message);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            sb.AppendLine(ex.Message);
                            return (false, ex.Message);
                        }
                    }

                    await File.WriteAllTextAsync(fileDecrypted, sb.ToString());
                    return (true, fileDecrypted);
                }
            }
        }
    }
}