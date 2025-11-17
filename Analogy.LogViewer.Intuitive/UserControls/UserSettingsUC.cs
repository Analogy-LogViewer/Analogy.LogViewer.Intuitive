#pragma warning disable SA1300 // Element should begin with upper-case letter
using Analogy.LogViewer.Intuitive.Managers;
using MediaManager.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Analogy.LogViewer.Intuitive.UserControls
{
    public partial class UserSettingsUc : UserControl
    {
        public EncryptionLogic EncryptionLogic => Analogy.LogViewer.Intuitive.Container.Instance.EncryptionLogic;

        public UserSettingsUc()
        {
            InitializeComponent();
        }

        private void importPrivateKeyButton_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
            string message = "Importing new private key may override current key and make log files unreadable. You may want to back up current key before proceed." +
                             "\r\n" + "Do you wish to proceed?";
            string caption = "Override current key warning";
            var result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Warning);
            if (result is DialogResult.OK)
            {
                if (openFileDialog1.ShowDialog() is DialogResult.OK)
                {
                    if (openFileDialog1.FileName is not null)
                    {
                        bool importResult = EncryptionLogic.ImportPrivateKeyFromFile(openFileDialog1.FileName, overrideCurrentKey: true);
                        if (importResult is true)
                        {
                            feedbackLabel.ForeColor = Color.ForestGreen;
                            feedbackLabel.Text = "Key imported  successfully";
                            feedbackLabel.Visible = true;
                        }
                        else
                        {
                            feedbackLabel.ForeColor = Color.Red;
                            feedbackLabel.Text = "false";
                            feedbackLabel.Visible = true;
                        }
                    }
                }
            }
        }

        private void exportPairKeysButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() is DialogResult.OK)
            {
                if (folderBrowserDialog1.SelectedPath is not null)
                {
                    if (EncryptionLogic.GenerateAndExporKeyXmlToFileRsa(folderBrowserDialog1.SelectedPath))
                    {
                        feedbackLabel.ForeColor = Color.ForestGreen;
                        feedbackLabel.Text = "Keys generated successfully and exported to\r\n\"" + folderBrowserDialog1.SelectedPath + "\"";
                        feedbackLabel.Visible = true;
                    }
                    else
                    {
                        feedbackLabel.ForeColor = Color.Red;
                        feedbackLabel.Text = "Unable to generate and export keys";
                        feedbackLabel.Visible = true;
                    }
                }
            }
        }

        private void btnViewCurrentPublicKey_Click(object sender, EventArgs e)
        {
            if (EncryptionLogic.TryGetCurrentKey(includePrivateKey: false, out string key))
            {
                feedbackLabel.Text = key;
                MessageBox.Show(key, "Key");
            }
            else
            {
                MessageBox.Show("Error: Key Was not found", "Key");
            }
        }

        private void exportCurrPrivateKeysButton_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
            string message = "Exporting a current private to file may compromise data security. Please do not leave it unprotected on the disk." +
                             "\r\n" + "Do you wish to proceed?";
            string caption = "Exporting current private key";
            var result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Warning);

            if (folderBrowserDialog1.ShowDialog() is DialogResult.OK)
            {
                if (folderBrowserDialog1.SelectedPath is not null)
                {
                    if (EncryptionLogic.ExportCurrentKeyXmlToFileRsa(folderBrowserDialog1.SelectedPath, includePrivateKey: true) && result is DialogResult.OK)
                    {
                        feedbackLabel.ForeColor = Color.ForestGreen;
                        feedbackLabel.Text = "Private key exported successfully to\r\n\"" + folderBrowserDialog1.SelectedPath + "\"";
                        feedbackLabel.Visible = true;
                    }
                    else
                    {
                        feedbackLabel.ForeColor = Color.Red;
                        feedbackLabel.Text = "Failed to export private key";
                        feedbackLabel.Visible = true;
                    }
                }
            }
        }

        private void deleteKeyButton_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
            string message = "You are about to delete current private key from CSP. Log files might become unreadable. You may want to back up current key before proceed." +
                             "\r\n" + "Do you wish to proceed?";
            string caption = "Delete current private key warning";
            var result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Warning);
            if (result is DialogResult.OK)
            {
                if (EncryptionLogic.DeleteKeysFromContainerRsa())
                {
                    feedbackLabel.ForeColor = Color.ForestGreen;
                    feedbackLabel.Text = "Private key was deleted successfully. \r\n"
                                         + "The key is still in memory, please close the application \r\n to see it disappear totally.";
                    feedbackLabel.Visible = true;
                }
                else
                {
                    feedbackLabel.ForeColor = Color.Red;
                    feedbackLabel.Text = "Failed to delete private key";
                    feedbackLabel.Visible = true;
                }
            }
        }

        private void UserSettingsUC_Load(object sender, EventArgs e)
        {
            txtbFFmpegEXELocation.Text = UserSettingsManager.Instance.FFmpegBinaryFolder;
            tbPublishSocket.Text = UserSettingsManager.Instance.PublishPort;
            tbSubscribeSocket.Text = UserSettingsManager.Instance.SubscribePort;
            cbShowAllMetadataFields.Checked = UserSettingsManager.Instance.ShowAllColumnsFromMetaDataField;
            lstbAdditionalColumn.Items.Clear();
            lstbAdditionalColumn.Items.AddRange(UserSettingsManager.Instance.AdditionalColumnsFromMetaDataField.ToArray());
            tbPublishSocket.TextChanged += (s, e) =>
            {
                UserSettingsManager.Instance.PublishPort = tbPublishSocket.Text;
                SaveSettings();
            };
            tbSubscribeSocket.TextChanged += (s, e) =>
            {
                UserSettingsManager.Instance.PublishPort = tbSubscribeSocket.Text;
                SaveSettings();
            };
            cbShowAllMetadataFields.CheckedChanged += (_, __) =>
            {
                UserSettingsManager.Instance.ShowAllColumnsFromMetaDataField = cbShowAllMetadataFields.Checked;
                SaveSettings();
            };
        }

        private void SaveSettings()
        {
            UserSettingsManager.Instance.Save();
        }

        private void tnSelectVideo_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog() { ShowNewFolderButton = false })
            {
                DialogResult result = folderBrowserDialog.ShowDialog(); // Show the dialog.
                if (result is DialogResult.OK) // Test result.
                {
                    txtbFFmpegEXELocation.Text = UserSettingsManager.Instance.FFmpegBinaryFolder = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void txtbFFmpegEXELocation_TextChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void btnExportCurrentPublicKey_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() is DialogResult.OK)
            {
                if (folderBrowserDialog1.SelectedPath is not null)
                {
                    if (EncryptionLogic.ExportCurrentKeyXmlToFileRsa(folderBrowserDialog1.SelectedPath, includePrivateKey: false))
                    {
                        feedbackLabel.ForeColor = Color.ForestGreen;
                        feedbackLabel.Text = "Public key exported successfully to\r\n \"" + folderBrowserDialog1.SelectedPath + "\"";
                        feedbackLabel.Visible = true;
                    }
                    else
                    {
                        feedbackLabel.ForeColor = Color.Red;
                        feedbackLabel.Text = "Failed to export public key";
                        feedbackLabel.Visible = true;
                    }
                }
            }
        }

        private void btnIgnoreColumn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtbIgnoreColumn.Text))
            {
                return;
            }

            lstbAdditionalColumn.Items.Add(txtbIgnoreColumn.Text);
            UserSettingsManager.Instance.AdditionalColumnsFromMetaDataField = lstbAdditionalColumn.Items.Count > 0 ? lstbAdditionalColumn.Items.Cast<string>().ToList() : new List<string>();
            UserSettingsManager.Instance.Save();
        }

        private void btnDeleteIgnoreColumn_Click(object sender, EventArgs e)
        {
            if (lstbAdditionalColumn.SelectedItem is string additional)
            {
                lstbAdditionalColumn.Items.Remove(lstbAdditionalColumn.SelectedItem);
                UserSettingsManager.Instance.AdditionalColumnsFromMetaDataField = lstbAdditionalColumn.Items.Count > 0 ? lstbAdditionalColumn.Items.Cast<string>().ToList() : new List<string>();
                UserSettingsManager.Instance.Save();
            }
        }
    }
}