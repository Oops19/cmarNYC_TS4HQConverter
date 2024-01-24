namespace TS4HQConverter
{
    partial class DupFileDialog
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
            this.DupWarning_label = new System.Windows.Forms.Label();
            this.Replace_button = new System.Windows.Forms.Button();
            this.Rename_button = new System.Windows.Forms.Button();
            this.Discard_button = new System.Windows.Forms.Button();
            this.Cancel_button = new System.Windows.Forms.Button();
            this.ApplyAll_checkBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // DupWarning_label
            // 
            this.DupWarning_label.AutoSize = true;
            this.DupWarning_label.Location = new System.Drawing.Point(30, 20);
            this.DupWarning_label.Name = "DupWarning_label";
            this.DupWarning_label.Size = new System.Drawing.Size(35, 13);
            this.DupWarning_label.TabIndex = 0;
            this.DupWarning_label.Text = "label1";
            // 
            // Replace_button
            // 
            this.Replace_button.Location = new System.Drawing.Point(33, 70);
            this.Replace_button.Name = "Replace_button";
            this.Replace_button.Size = new System.Drawing.Size(326, 30);
            this.Replace_button.TabIndex = 1;
            this.Replace_button.Text = "Replace old package with new";
            this.Replace_button.UseVisualStyleBackColor = true;
            this.Replace_button.Click += new System.EventHandler(this.Replace_button_Click);
            // 
            // Rename_button
            // 
            this.Rename_button.Location = new System.Drawing.Point(33, 106);
            this.Rename_button.Name = "Rename_button";
            this.Rename_button.Size = new System.Drawing.Size(326, 30);
            this.Rename_button.TabIndex = 2;
            this.Rename_button.Text = "Rename new package";
            this.Rename_button.UseVisualStyleBackColor = true;
            this.Rename_button.Click += new System.EventHandler(this.Rename_button_Click);
            // 
            // Discard_button
            // 
            this.Discard_button.Location = new System.Drawing.Point(33, 142);
            this.Discard_button.Name = "Discard_button";
            this.Discard_button.Size = new System.Drawing.Size(326, 30);
            this.Discard_button.TabIndex = 3;
            this.Discard_button.Text = "Discard new package";
            this.Discard_button.UseVisualStyleBackColor = true;
            this.Discard_button.Click += new System.EventHandler(this.Discard_button_Click);
            // 
            // Cancel_button
            // 
            this.Cancel_button.Location = new System.Drawing.Point(33, 178);
            this.Cancel_button.Name = "Cancel_button";
            this.Cancel_button.Size = new System.Drawing.Size(326, 30);
            this.Cancel_button.TabIndex = 4;
            this.Cancel_button.Text = "Cancel";
            this.Cancel_button.UseVisualStyleBackColor = true;
            this.Cancel_button.Click += new System.EventHandler(this.Cancel_button_Click);
            // 
            // ApplyAll_checkBox
            // 
            this.ApplyAll_checkBox.AutoSize = true;
            this.ApplyAll_checkBox.Location = new System.Drawing.Point(157, 225);
            this.ApplyAll_checkBox.Name = "ApplyAll_checkBox";
            this.ApplyAll_checkBox.Size = new System.Drawing.Size(77, 17);
            this.ApplyAll_checkBox.TabIndex = 5;
            this.ApplyAll_checkBox.Text = "Apply to all";
            this.ApplyAll_checkBox.UseVisualStyleBackColor = true;
            // 
            // DupFileDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 258);
            this.Controls.Add(this.ApplyAll_checkBox);
            this.Controls.Add(this.Cancel_button);
            this.Controls.Add(this.Discard_button);
            this.Controls.Add(this.Rename_button);
            this.Controls.Add(this.Replace_button);
            this.Controls.Add(this.DupWarning_label);
            this.Name = "DupFileDialog";
            this.Text = "Duplicate File Dialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label DupWarning_label;
        private System.Windows.Forms.Button Replace_button;
        private System.Windows.Forms.Button Rename_button;
        private System.Windows.Forms.Button Discard_button;
        private System.Windows.Forms.Button Cancel_button;
        private System.Windows.Forms.CheckBox ApplyAll_checkBox;
    }
}