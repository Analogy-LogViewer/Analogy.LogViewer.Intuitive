namespace Analogy.LogViewer.Intuitive.UserControls
{
    partial class UserSettingsUc
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components is not null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            deleteKeyButton = new System.Windows.Forms.Button();
            exportPairKeysButton = new System.Windows.Forms.Button();
            exportCurrPrivateKeysButton = new System.Windows.Forms.Button();
            btnViewCurrentPublicKey = new System.Windows.Forms.Button();
            importPrivateKeyButton = new System.Windows.Forms.Button();
            openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            tabControl1 = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            groupBox1 = new System.Windows.Forms.GroupBox();
            btnExportCurrentPublicKey = new System.Windows.Forms.Button();
            feedbackLabel = new System.Windows.Forms.Label();
            tabPage2 = new System.Windows.Forms.TabPage();
            groupBox3 = new System.Windows.Forms.GroupBox();
            tnSelectVideo = new System.Windows.Forms.Button();
            txtbFFmpegEXELocation = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            tabPage3 = new System.Windows.Forms.TabPage();
            groupBox2 = new System.Windows.Forms.GroupBox();
            tbSubscribeSocket = new System.Windows.Forms.TextBox();
            tbPublishSocket = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            tabPage4 = new System.Windows.Forms.TabPage();
            tabControlFiles = new System.Windows.Forms.TabControl();
            tabPageFileDynamicsColumns = new System.Windows.Forms.TabPage();
            cbShowAllMetadataFields = new System.Windows.Forms.CheckBox();
            txtbIgnoreColumn = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            btnDeleteIgnoreColumn = new System.Windows.Forms.Button();
            btnIgnoreColumn = new System.Windows.Forms.Button();
            lstbAdditionalColumn = new System.Windows.Forms.ListBox();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            groupBox1.SuspendLayout();
            tabPage2.SuspendLayout();
            groupBox3.SuspendLayout();
            tabPage3.SuspendLayout();
            groupBox2.SuspendLayout();
            tabPage4.SuspendLayout();
            tabControlFiles.SuspendLayout();
            tabPageFileDynamicsColumns.SuspendLayout();
            SuspendLayout();
            // 
            // deleteKeyButton
            // 
            deleteKeyButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            deleteKeyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            deleteKeyButton.Location = new System.Drawing.Point(211, 79);
            deleteKeyButton.Margin = new System.Windows.Forms.Padding(26, 31, 26, 31);
            deleteKeyButton.Name = "deleteKeyButton";
            deleteKeyButton.Size = new System.Drawing.Size(312, 36);
            deleteKeyButton.TabIndex = 4;
            deleteKeyButton.Text = "Delete Private key from Analogy";
            deleteKeyButton.UseVisualStyleBackColor = true;
            deleteKeyButton.Click += deleteKeyButton_Click;
            // 
            // exportPairKeysButton
            // 
            exportPairKeysButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            exportPairKeysButton.Enabled = false;
            exportPairKeysButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            exportPairKeysButton.Location = new System.Drawing.Point(211, 210);
            exportPairKeysButton.Margin = new System.Windows.Forms.Padding(26, 31, 26, 31);
            exportPairKeysButton.Name = "exportPairKeysButton";
            exportPairKeysButton.Size = new System.Drawing.Size(312, 36);
            exportPairKeysButton.TabIndex = 5;
            exportPairKeysButton.Text = "Generate and export a pair of keys";
            exportPairKeysButton.UseVisualStyleBackColor = true;
            exportPairKeysButton.Click += exportPairKeysButton_Click;
            // 
            // exportCurrPrivateKeysButton
            // 
            exportCurrPrivateKeysButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            exportCurrPrivateKeysButton.Enabled = false;
            exportCurrPrivateKeysButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            exportCurrPrivateKeysButton.Location = new System.Drawing.Point(211, 254);
            exportCurrPrivateKeysButton.Margin = new System.Windows.Forms.Padding(26, 31, 26, 31);
            exportCurrPrivateKeysButton.Name = "exportCurrPrivateKeysButton";
            exportCurrPrivateKeysButton.Size = new System.Drawing.Size(312, 36);
            exportCurrPrivateKeysButton.TabIndex = 1;
            exportCurrPrivateKeysButton.Text = "Export current private key";
            exportCurrPrivateKeysButton.UseVisualStyleBackColor = true;
            exportCurrPrivateKeysButton.Click += exportCurrPrivateKeysButton_Click;
            // 
            // btnViewCurrentPublicKey
            // 
            btnViewCurrentPublicKey.Anchor = System.Windows.Forms.AnchorStyles.Top;
            btnViewCurrentPublicKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            btnViewCurrentPublicKey.Location = new System.Drawing.Point(211, 122);
            btnViewCurrentPublicKey.Margin = new System.Windows.Forms.Padding(26, 31, 26, 31);
            btnViewCurrentPublicKey.Name = "btnViewCurrentPublicKey";
            btnViewCurrentPublicKey.Size = new System.Drawing.Size(312, 36);
            btnViewCurrentPublicKey.TabIndex = 3;
            btnViewCurrentPublicKey.Text = "View Current public key";
            btnViewCurrentPublicKey.UseVisualStyleBackColor = true;
            btnViewCurrentPublicKey.Click += btnViewCurrentPublicKey_Click;
            // 
            // importPrivateKeyButton
            // 
            importPrivateKeyButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            importPrivateKeyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            importPrivateKeyButton.Location = new System.Drawing.Point(211, 35);
            importPrivateKeyButton.Margin = new System.Windows.Forms.Padding(26, 31, 26, 31);
            importPrivateKeyButton.Name = "importPrivateKeyButton";
            importPrivateKeyButton.Size = new System.Drawing.Size(312, 36);
            importPrivateKeyButton.TabIndex = 0;
            importPrivateKeyButton.Text = "Import Private key from file to Analogy";
            importPrivateKeyButton.UseVisualStyleBackColor = true;
            importPrivateKeyButton.Click += importPrivateKeyButton_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // folderBrowserDialog1
            // 
            folderBrowserDialog1.Description = "Choose folder to generate keys into";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl1.Location = new System.Drawing.Point(0, 0);
            tabControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(734, 528);
            tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(groupBox1);
            tabPage1.Location = new System.Drawing.Point(4, 29);
            tabPage1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            tabPage1.Size = new System.Drawing.Size(726, 495);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Settings";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnExportCurrentPublicKey);
            groupBox1.Controls.Add(feedbackLabel);
            groupBox1.Controls.Add(deleteKeyButton);
            groupBox1.Controls.Add(importPrivateKeyButton);
            groupBox1.Controls.Add(exportCurrPrivateKeysButton);
            groupBox1.Controls.Add(btnViewCurrentPublicKey);
            groupBox1.Controls.Add(exportPairKeysButton);
            groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox1.Location = new System.Drawing.Point(3, 4);
            groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            groupBox1.Size = new System.Drawing.Size(720, 487);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "Log Settings";
            // 
            // btnExportCurrentPublicKey
            // 
            btnExportCurrentPublicKey.Anchor = System.Windows.Forms.AnchorStyles.Top;
            btnExportCurrentPublicKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            btnExportCurrentPublicKey.Location = new System.Drawing.Point(211, 166);
            btnExportCurrentPublicKey.Margin = new System.Windows.Forms.Padding(26, 31, 26, 31);
            btnExportCurrentPublicKey.Name = "btnExportCurrentPublicKey";
            btnExportCurrentPublicKey.Size = new System.Drawing.Size(312, 36);
            btnExportCurrentPublicKey.TabIndex = 6;
            btnExportCurrentPublicKey.Text = "Export Current public key";
            btnExportCurrentPublicKey.UseVisualStyleBackColor = true;
            btnExportCurrentPublicKey.Click += btnExportCurrentPublicKey_Click;
            // 
            // feedbackLabel
            // 
            feedbackLabel.AutoSize = true;
            feedbackLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            feedbackLabel.Location = new System.Drawing.Point(14, 311);
            feedbackLabel.Margin = new System.Windows.Forms.Padding(14, 16, 14, 16);
            feedbackLabel.Name = "feedbackLabel";
            feedbackLabel.Size = new System.Drawing.Size(147, 17);
            feedbackLabel.TabIndex = 2;
            feedbackLabel.Text = "Feedback message";
            feedbackLabel.Visible = false;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(groupBox3);
            tabPage2.Location = new System.Drawing.Point(4, 29);
            tabPage2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            tabPage2.Size = new System.Drawing.Size(726, 495);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "FFmpeg/probe Settings";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(tnSelectVideo);
            groupBox3.Controls.Add(txtbFFmpegEXELocation);
            groupBox3.Controls.Add(label3);
            groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            groupBox3.Location = new System.Drawing.Point(3, 4);
            groupBox3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            groupBox3.Size = new System.Drawing.Size(720, 66);
            groupBox3.TabIndex = 9;
            groupBox3.TabStop = false;
            groupBox3.Text = "FFProbe Settings";
            // 
            // tnSelectVideo
            // 
            tnSelectVideo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            tnSelectVideo.Location = new System.Drawing.Point(657, 19);
            tnSelectVideo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            tnSelectVideo.Name = "tnSelectVideo";
            tnSelectVideo.Size = new System.Drawing.Size(56, 35);
            tnSelectVideo.TabIndex = 17;
            tnSelectVideo.Text = "...";
            tnSelectVideo.UseVisualStyleBackColor = true;
            tnSelectVideo.Click += tnSelectVideo_Click;
            // 
            // txtbFFmpegEXELocation
            // 
            txtbFFmpegEXELocation.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtbFFmpegEXELocation.Location = new System.Drawing.Point(211, 24);
            txtbFFmpegEXELocation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtbFFmpegEXELocation.Name = "txtbFFmpegEXELocation";
            txtbFFmpegEXELocation.Size = new System.Drawing.Size(439, 27);
            txtbFFmpegEXELocation.TabIndex = 3;
            txtbFFmpegEXELocation.TextChanged += txtbFFmpegEXELocation_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(16, 26);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(130, 20);
            label3.TabIndex = 1;
            label3.Text = "Executable Folder:";
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(groupBox2);
            tabPage3.Location = new System.Drawing.Point(4, 29);
            tabPage3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            tabPage3.Size = new System.Drawing.Size(726, 495);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Hub ZMQ Messages";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(tbSubscribeSocket);
            groupBox2.Controls.Add(tbPublishSocket);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(label1);
            groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox2.Location = new System.Drawing.Point(3, 4);
            groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            groupBox2.Size = new System.Drawing.Size(720, 487);
            groupBox2.TabIndex = 7;
            groupBox2.TabStop = false;
            groupBox2.Text = "IPC Settings";
            // 
            // tbSubscribeSocket
            // 
            tbSubscribeSocket.Location = new System.Drawing.Point(211, 21);
            tbSubscribeSocket.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            tbSubscribeSocket.Name = "tbSubscribeSocket";
            tbSubscribeSocket.Size = new System.Drawing.Size(312, 27);
            tbSubscribeSocket.TabIndex = 3;
            // 
            // tbPublishSocket
            // 
            tbPublishSocket.Location = new System.Drawing.Point(211, 56);
            tbPublishSocket.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            tbPublishSocket.Name = "tbPublishSocket";
            tbPublishSocket.Size = new System.Drawing.Size(312, 27);
            tbPublishSocket.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(16, 21);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(106, 20);
            label2.TabIndex = 1;
            label2.Text = "Subscribe Port:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(16, 60);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(89, 20);
            label1.TabIndex = 0;
            label1.Text = "Publish Port:";
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(tabControlFiles);
            tabPage4.Location = new System.Drawing.Point(4, 29);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new System.Windows.Forms.Padding(3);
            tabPage4.Size = new System.Drawing.Size(726, 495);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "ECS Settings";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabControlFiles
            // 
            tabControlFiles.Controls.Add(tabPageFileDynamicsColumns);
            tabControlFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControlFiles.Location = new System.Drawing.Point(3, 3);
            tabControlFiles.Name = "tabControlFiles";
            tabControlFiles.SelectedIndex = 0;
            tabControlFiles.Size = new System.Drawing.Size(720, 489);
            tabControlFiles.TabIndex = 50;
            // 
            // tabPageFileDynamicsColumns
            // 
            tabPageFileDynamicsColumns.Controls.Add(cbShowAllMetadataFields);
            tabPageFileDynamicsColumns.Controls.Add(txtbIgnoreColumn);
            tabPageFileDynamicsColumns.Controls.Add(label4);
            tabPageFileDynamicsColumns.Controls.Add(btnDeleteIgnoreColumn);
            tabPageFileDynamicsColumns.Controls.Add(btnIgnoreColumn);
            tabPageFileDynamicsColumns.Controls.Add(lstbAdditionalColumn);
            tabPageFileDynamicsColumns.ImageKey = "GridColumnHeader_32x32.png";
            tabPageFileDynamicsColumns.Location = new System.Drawing.Point(4, 29);
            tabPageFileDynamicsColumns.Name = "tabPageFileDynamicsColumns";
            tabPageFileDynamicsColumns.Padding = new System.Windows.Forms.Padding(3);
            tabPageFileDynamicsColumns.Size = new System.Drawing.Size(712, 456);
            tabPageFileDynamicsColumns.TabIndex = 1;
            tabPageFileDynamicsColumns.Text = "Dynamic Columns";
            tabPageFileDynamicsColumns.UseVisualStyleBackColor = true;
            // 
            // cbShowAllMetadataFields
            // 
            cbShowAllMetadataFields.AutoSize = true;
            cbShowAllMetadataFields.Location = new System.Drawing.Point(13, 13);
            cbShowAllMetadataFields.Name = "cbShowAllMetadataFields";
            cbShowAllMetadataFields.Size = new System.Drawing.Size(195, 24);
            cbShowAllMetadataFields.TabIndex = 64;
            cbShowAllMetadataFields.Text = "Show all metadata fields";
            cbShowAllMetadataFields.UseVisualStyleBackColor = true;
            // 
            // txtbIgnoreColumn
            // 
            txtbIgnoreColumn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtbIgnoreColumn.Location = new System.Drawing.Point(9, 84);
            txtbIgnoreColumn.Name = "txtbIgnoreColumn";
            txtbIgnoreColumn.Size = new System.Drawing.Size(637, 27);
            txtbIgnoreColumn.TabIndex = 63;
            // 
            // label4
            // 
            label4.AccessibleDescription = "";
            label4.AutoEllipsis = true;
            label4.Location = new System.Drawing.Point(3, 53);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(697, 24);
            label4.TabIndex = 62;
            label4.Text = "Show The following Columns from metadata Fields:";
            // 
            // btnDeleteIgnoreColumn
            // 
            btnDeleteIgnoreColumn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnDeleteIgnoreColumn.Location = new System.Drawing.Point(607, 136);
            btnDeleteIgnoreColumn.Name = "btnDeleteIgnoreColumn";
            btnDeleteIgnoreColumn.Size = new System.Drawing.Size(93, 25);
            btnDeleteIgnoreColumn.TabIndex = 61;
            btnDeleteIgnoreColumn.Text = "Delete";
            btnDeleteIgnoreColumn.UseVisualStyleBackColor = true;
            btnDeleteIgnoreColumn.Click += btnDeleteIgnoreColumn_Click;
            // 
            // btnIgnoreColumn
            // 
            btnIgnoreColumn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnIgnoreColumn.Location = new System.Drawing.Point(653, 84);
            btnIgnoreColumn.Name = "btnIgnoreColumn";
            btnIgnoreColumn.Size = new System.Drawing.Size(47, 25);
            btnIgnoreColumn.TabIndex = 60;
            btnIgnoreColumn.Text = "Add";
            btnIgnoreColumn.UseVisualStyleBackColor = true;
            btnIgnoreColumn.Click += btnIgnoreColumn_Click;
            // 
            // lstbAdditionalColumn
            // 
            lstbAdditionalColumn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lstbAdditionalColumn.FormattingEnabled = true;
            lstbAdditionalColumn.ItemHeight = 20;
            lstbAdditionalColumn.Location = new System.Drawing.Point(9, 136);
            lstbAdditionalColumn.Name = "lstbAdditionalColumn";
            lstbAdditionalColumn.Size = new System.Drawing.Size(583, 304);
            lstbAdditionalColumn.TabIndex = 59;
            // 
            // UserSettingsUC
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tabControl1);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "UserSettingsUc";
            Size = new System.Drawing.Size(734, 528);
            Load += UserSettingsUC_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabPage2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            tabPage3.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            tabPage4.ResumeLayout(false);
            tabControlFiles.ResumeLayout(false);
            tabPageFileDynamicsColumns.ResumeLayout(false);
            tabPageFileDynamicsColumns.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Button deleteKeyButton;
        private System.Windows.Forms.Button exportPairKeysButton;
        private System.Windows.Forms.Button exportCurrPrivateKeysButton;
        private System.Windows.Forms.Button btnViewCurrentPublicKey;
        private System.Windows.Forms.Button importPrivateKeyButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbSubscribeSocket;
        private System.Windows.Forms.TextBox tbPublishSocket;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtbFFmpegEXELocation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button tnSelectVideo;
        private System.Windows.Forms.Label feedbackLabel;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnExportCurrentPublicKey;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabControl tabControlFiles;
        private System.Windows.Forms.TabPage tabPageFileDynamicsColumns;
        private System.Windows.Forms.CheckBox cbShowAllMetadataFields;
        private System.Windows.Forms.TextBox txtbIgnoreColumn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnDeleteIgnoreColumn;
        private System.Windows.Forms.Button btnIgnoreColumn;
        private System.Windows.Forms.ListBox lstbAdditionalColumn;
    }
}
