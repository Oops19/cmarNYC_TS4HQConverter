namespace TS4HQConverter
{
    partial class Form1
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.PackageSelect_button = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.FolderSelect_button = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.OutputSelect_button = new System.Windows.Forms.Button();
            this.FolderName = new System.Windows.Forms.TextBox();
            this.OutputName = new System.Windows.Forms.TextBox();
            this.FolderGo_button = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.KeepMipMaps_checkBox = new System.Windows.Forms.CheckBox();
            this.Sharpen_panel = new System.Windows.Forms.Panel();
            this.SkipGrayscale_checkBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Sharpen_maskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.Sharpen_checkBox = new System.Windows.Forms.CheckBox();
            this.KeepSize_checkBox = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.HQ_radioButton = new System.Windows.Forms.RadioButton();
            this.NonHQ_radioButton = new System.Windows.Forms.RadioButton();
            this.RLESdimensions_comboBox = new System.Windows.Forms.ComboBox();
            this.RLE2dimensions_comboBox = new System.Windows.Forms.ComboBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Subfolders_checkBox = new System.Windows.Forms.CheckBox();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.Sharpen_panel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // PackageSelect_button
            // 
            this.PackageSelect_button.Location = new System.Drawing.Point(22, 21);
            this.PackageSelect_button.Name = "PackageSelect_button";
            this.PackageSelect_button.Size = new System.Drawing.Size(658, 64);
            this.PackageSelect_button.TabIndex = 2;
            this.PackageSelect_button.Text = "Select Package - diffuse, shadow, specular, and skin textures will be resized";
            this.PackageSelect_button.UseVisualStyleBackColor = true;
            this.PackageSelect_button.Click += new System.EventHandler(this.PackageSelect_button_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3});
            this.statusStrip1.Location = new System.Drawing.Point(0, 448);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(703, 22);
            this.statusStrip1.TabIndex = 3;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel3.Text = "toolStripStatusLabel3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(339, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "OR";
            // 
            // FolderSelect_button
            // 
            this.FolderSelect_button.Location = new System.Drawing.Point(0, 19);
            this.FolderSelect_button.Name = "FolderSelect_button";
            this.FolderSelect_button.Size = new System.Drawing.Size(165, 31);
            this.FolderSelect_button.TabIndex = 7;
            this.FolderSelect_button.Text = "Select Input Folder";
            this.FolderSelect_button.UseVisualStyleBackColor = true;
            this.FolderSelect_button.Click += new System.EventHandler(this.FolderSelect_button_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(168, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Diffuse/Shadow/Skin:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(168, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Specular:";
            // 
            // OutputSelect_button
            // 
            this.OutputSelect_button.Location = new System.Drawing.Point(0, 56);
            this.OutputSelect_button.Name = "OutputSelect_button";
            this.OutputSelect_button.Size = new System.Drawing.Size(165, 31);
            this.OutputSelect_button.TabIndex = 22;
            this.OutputSelect_button.Text = "Select Output Folder";
            this.OutputSelect_button.UseVisualStyleBackColor = true;
            this.OutputSelect_button.Click += new System.EventHandler(this.OutputSelect_button_Click);
            // 
            // FolderName
            // 
            this.FolderName.Location = new System.Drawing.Point(171, 25);
            this.FolderName.Name = "FolderName";
            this.FolderName.Size = new System.Drawing.Size(424, 20);
            this.FolderName.TabIndex = 23;
            // 
            // OutputName
            // 
            this.OutputName.Location = new System.Drawing.Point(171, 62);
            this.OutputName.Name = "OutputName";
            this.OutputName.Size = new System.Drawing.Size(424, 20);
            this.OutputName.TabIndex = 24;
            // 
            // FolderGo_button
            // 
            this.FolderGo_button.Location = new System.Drawing.Point(601, 25);
            this.FolderGo_button.Name = "FolderGo_button";
            this.FolderGo_button.Size = new System.Drawing.Size(57, 57);
            this.FolderGo_button.TabIndex = 26;
            this.FolderGo_button.Text = "Go";
            this.FolderGo_button.UseVisualStyleBackColor = true;
            this.FolderGo_button.Click += new System.EventHandler(this.FolderGo_button_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.KeepMipMaps_checkBox);
            this.groupBox1.Controls.Add(this.Sharpen_panel);
            this.groupBox1.Controls.Add(this.Sharpen_checkBox);
            this.groupBox1.Controls.Add(this.KeepSize_checkBox);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.RLESdimensions_comboBox);
            this.groupBox1.Controls.Add(this.RLE2dimensions_comboBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(22, 256);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(658, 174);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Texture output options:";
            // 
            // KeepMipMaps_checkBox
            // 
            this.KeepMipMaps_checkBox.AutoSize = true;
            this.KeepMipMaps_checkBox.Checked = true;
            this.KeepMipMaps_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.KeepMipMaps_checkBox.Location = new System.Drawing.Point(6, 144);
            this.KeepMipMaps_checkBox.Name = "KeepMipMaps_checkBox";
            this.KeepMipMaps_checkBox.Size = new System.Drawing.Size(200, 17);
            this.KeepMipMaps_checkBox.TabIndex = 25;
            this.KeepMipMaps_checkBox.Text = "Use existing mipmaps where possible";
            this.KeepMipMaps_checkBox.UseVisualStyleBackColor = true;
            // 
            // Sharpen_panel
            // 
            this.Sharpen_panel.Controls.Add(this.SkipGrayscale_checkBox);
            this.Sharpen_panel.Controls.Add(this.label1);
            this.Sharpen_panel.Controls.Add(this.Sharpen_maskedTextBox);
            this.Sharpen_panel.Location = new System.Drawing.Point(287, 113);
            this.Sharpen_panel.Name = "Sharpen_panel";
            this.Sharpen_panel.Size = new System.Drawing.Size(365, 36);
            this.Sharpen_panel.TabIndex = 24;
            // 
            // SkipGrayscale_checkBox
            // 
            this.SkipGrayscale_checkBox.AutoSize = true;
            this.SkipGrayscale_checkBox.Location = new System.Drawing.Point(174, 10);
            this.SkipGrayscale_checkBox.Name = "SkipGrayscale_checkBox";
            this.SkipGrayscale_checkBox.Size = new System.Drawing.Size(140, 17);
            this.SkipGrayscale_checkBox.TabIndex = 24;
            this.SkipGrayscale_checkBox.Text = "Don\'t sharpen grayscale";
            this.toolTip1.SetToolTip(this.SkipGrayscale_checkBox, "Can be used when converting packages that contain textures but not CASPs.\r\nUsed t" +
        "o identify and avoid sharpening shadows but may cause black and \r\nwhite or gray " +
        "clothing to not be sharpened.");
            this.SkipGrayscale_checkBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Sharpen strength: ";
            // 
            // Sharpen_maskedTextBox
            // 
            this.Sharpen_maskedTextBox.Location = new System.Drawing.Point(103, 5);
            this.Sharpen_maskedTextBox.Mask = "00";
            this.Sharpen_maskedTextBox.Name = "Sharpen_maskedTextBox";
            this.Sharpen_maskedTextBox.Size = new System.Drawing.Size(45, 20);
            this.Sharpen_maskedTextBox.TabIndex = 22;
            this.Sharpen_maskedTextBox.ValidatingType = typeof(int);
            this.Sharpen_maskedTextBox.TextChanged += new System.EventHandler(this.Sharpen_maskedTextBox_TextChanged);
            // 
            // Sharpen_checkBox
            // 
            this.Sharpen_checkBox.AutoSize = true;
            this.Sharpen_checkBox.Checked = true;
            this.Sharpen_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Sharpen_checkBox.Location = new System.Drawing.Point(6, 120);
            this.Sharpen_checkBox.Name = "Sharpen_checkBox";
            this.Sharpen_checkBox.Size = new System.Drawing.Size(259, 17);
            this.Sharpen_checkBox.TabIndex = 20;
            this.Sharpen_checkBox.Text = "Sharpen rle2 and lrle textures (slow / better detail)";
            this.Sharpen_checkBox.UseVisualStyleBackColor = true;
            this.Sharpen_checkBox.CheckedChanged += new System.EventHandler(this.Sharpen_checkBox_CheckedChanged);
            // 
            // KeepSize_checkBox
            // 
            this.KeepSize_checkBox.AutoSize = true;
            this.KeepSize_checkBox.Checked = true;
            this.KeepSize_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.KeepSize_checkBox.Location = new System.Drawing.Point(6, 96);
            this.KeepSize_checkBox.Name = "KeepSize_checkBox";
            this.KeepSize_checkBox.Size = new System.Drawing.Size(208, 17);
            this.KeepSize_checkBox.TabIndex = 17;
            this.KeepSize_checkBox.Text = "Keep original size if larger than needed";
            this.KeepSize_checkBox.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.HQ_radioButton);
            this.panel1.Controls.Add(this.NonHQ_radioButton);
            this.panel1.Location = new System.Drawing.Point(0, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(162, 61);
            this.panel1.TabIndex = 19;
            // 
            // HQ_radioButton
            // 
            this.HQ_radioButton.AutoSize = true;
            this.HQ_radioButton.Location = new System.Drawing.Point(3, 3);
            this.HQ_radioButton.Name = "HQ_radioButton";
            this.HQ_radioButton.Size = new System.Drawing.Size(99, 17);
            this.HQ_radioButton.TabIndex = 15;
            this.HQ_radioButton.Text = "Standard to HQ";
            this.HQ_radioButton.UseVisualStyleBackColor = true;
            this.HQ_radioButton.CheckedChanged += new System.EventHandler(this.HQ_radioButton_CheckedChanged);
            // 
            // NonHQ_radioButton
            // 
            this.NonHQ_radioButton.AutoSize = true;
            this.NonHQ_radioButton.Location = new System.Drawing.Point(3, 26);
            this.NonHQ_radioButton.Name = "NonHQ_radioButton";
            this.NonHQ_radioButton.Size = new System.Drawing.Size(97, 17);
            this.NonHQ_radioButton.TabIndex = 16;
            this.NonHQ_radioButton.TabStop = true;
            this.NonHQ_radioButton.Text = "HQ to standard";
            this.NonHQ_radioButton.UseVisualStyleBackColor = true;
            this.NonHQ_radioButton.CheckedChanged += new System.EventHandler(this.NonHQ_radioButton_CheckedChanged);
            // 
            // RLESdimensions_comboBox
            // 
            this.RLESdimensions_comboBox.FormattingEnabled = true;
            this.RLESdimensions_comboBox.Location = new System.Drawing.Point(287, 49);
            this.RLESdimensions_comboBox.Name = "RLESdimensions_comboBox";
            this.RLESdimensions_comboBox.Size = new System.Drawing.Size(308, 21);
            this.RLESdimensions_comboBox.TabIndex = 14;
            // 
            // RLE2dimensions_comboBox
            // 
            this.RLE2dimensions_comboBox.FormattingEnabled = true;
            this.RLE2dimensions_comboBox.Location = new System.Drawing.Point(287, 22);
            this.RLE2dimensions_comboBox.Name = "RLE2dimensions_comboBox";
            this.RLE2dimensions_comboBox.Size = new System.Drawing.Size(308, 21);
            this.RLE2dimensions_comboBox.TabIndex = 13;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Subfolders_checkBox);
            this.groupBox2.Controls.Add(this.FolderSelect_button);
            this.groupBox2.Controls.Add(this.OutputSelect_button);
            this.groupBox2.Controls.Add(this.FolderGo_button);
            this.groupBox2.Controls.Add(this.FolderName);
            this.groupBox2.Controls.Add(this.OutputName);
            this.groupBox2.Location = new System.Drawing.Point(22, 120);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(658, 121);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "All packages in input folder will be processed and saved in output folder";
            // 
            // Subfolders_checkBox
            // 
            this.Subfolders_checkBox.AutoSize = true;
            this.Subfolders_checkBox.Checked = true;
            this.Subfolders_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Subfolders_checkBox.Location = new System.Drawing.Point(0, 93);
            this.Subfolders_checkBox.Name = "Subfolders_checkBox";
            this.Subfolders_checkBox.Size = new System.Drawing.Size(115, 17);
            this.Subfolders_checkBox.TabIndex = 27;
            this.Subfolders_checkBox.Text = "Include sub-folders";
            this.Subfolders_checkBox.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 470);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.PackageSelect_button);
            this.Name = "Form1";
            this.Text = "TS4 HQ Texture Converter v2.7 64-bit";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.Sharpen_panel.ResumeLayout(false);
            this.Sharpen_panel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button PackageSelect_button;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button FolderSelect_button;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button OutputSelect_button;
        private System.Windows.Forms.TextBox FolderName;
        private System.Windows.Forms.TextBox OutputName;
        private System.Windows.Forms.Button FolderGo_button;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ComboBox RLESdimensions_comboBox;
        private System.Windows.Forms.ComboBox RLE2dimensions_comboBox;
        private System.Windows.Forms.RadioButton HQ_radioButton;
        private System.Windows.Forms.RadioButton NonHQ_radioButton;
        private System.Windows.Forms.CheckBox KeepSize_checkBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox Sharpen_checkBox;
        private System.Windows.Forms.Panel Sharpen_panel;
        private System.Windows.Forms.CheckBox SkipGrayscale_checkBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox Sharpen_maskedTextBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox KeepMipMaps_checkBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox Subfolders_checkBox;
    }
}

