using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TS4HQConverter
{
    public partial class DupFileDialog : Form
    {
        public bool ApplyToAll = false;

        public DupFileDialog(string warning)
        {
            InitializeComponent();
            this.DupWarning_label.Text = warning;
        }

        private void Replace_button_Click(object sender, EventArgs e)
        {
            ApplyToAll = ApplyAll_checkBox.Checked;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Rename_button_Click(object sender, EventArgs e)
        {
            ApplyToAll = ApplyAll_checkBox.Checked;
            this.DialogResult = DialogResult.Retry;
            this.Close();
        }

        private void Discard_button_Click(object sender, EventArgs e)
        {
            ApplyToAll = ApplyAll_checkBox.Checked;
            this.DialogResult = DialogResult.Ignore;
            this.Close();
        }

        private void Cancel_button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
