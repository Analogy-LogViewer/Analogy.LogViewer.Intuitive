namespace Analogy.LogViewer.Intuitive.UserControls
{
    partial class BatchDecryptionUc
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnDycrpt = new System.Windows.Forms.Button();
            this.tnSelectFolder = new System.Windows.Forms.Button();
            this.txtbLogFolder = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnDycrpt);
            this.groupBox3.Controls.Add(this.tnSelectFolder);
            this.groupBox3.Controls.Add(this.txtbLogFolder);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(995, 53);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Input Folder";
            // 
            // btnDycrpt
            // 
            this.btnDycrpt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDycrpt.Location = new System.Drawing.Point(888, 15);
            this.btnDycrpt.Margin = new System.Windows.Forms.Padding(4);
            this.btnDycrpt.Name = "btnDycrpt";
            this.btnDycrpt.Size = new System.Drawing.Size(100, 28);
            this.btnDycrpt.TabIndex = 17;
            this.btnDycrpt.Text = "Decrypt";
            this.btnDycrpt.UseVisualStyleBackColor = true;
            this.btnDycrpt.Click += new System.EventHandler(this.btnDycrpt_Click);
            // 
            // tnSelectFolder
            // 
            this.tnSelectFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tnSelectFolder.Location = new System.Drawing.Point(824, 15);
            this.tnSelectFolder.Margin = new System.Windows.Forms.Padding(4);
            this.tnSelectFolder.Name = "tnSelectFolder";
            this.tnSelectFolder.Size = new System.Drawing.Size(56, 28);
            this.tnSelectFolder.TabIndex = 16;
            this.tnSelectFolder.Text = "...";
            this.tnSelectFolder.UseVisualStyleBackColor = true;
            this.tnSelectFolder.Click += new System.EventHandler(this.tnSelectFolder_Click);
            // 
            // txtbLogFolder
            // 
            this.txtbLogFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtbLogFolder.Location = new System.Drawing.Point(76, 19);
            this.txtbLogFolder.Name = "txtbLogFolder";
            this.txtbLogFolder.Size = new System.Drawing.Size(741, 22);
            this.txtbLogFolder.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "Folder:";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 53);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(995, 336);
            this.richTextBox1.TabIndex = 12;
            this.richTextBox1.Text = "";
            // 
            // BatchDecryptionsUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.groupBox3);
            this.Name = "BatchDecryptionUc";
            this.Size = new System.Drawing.Size(995, 389);
            this.Load += new System.EventHandler(this.BatchDecryptionsUC_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnDycrpt;
        private System.Windows.Forms.Button tnSelectFolder;
        private System.Windows.Forms.TextBox txtbLogFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}
