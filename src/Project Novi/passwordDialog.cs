using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Novi
{
    public partial class PasswordDialog : Form
    {
        public PasswordDialog()
        {
            InitializeComponent();
        }

        private void PasswordDialog_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (textboxPassInput.Text == "novi")
                    {
                        Controlpanel cp = new Controlpanel();
                        cp.Show();
                    }
                    break;
                case Keys.Escape:
                    Close();
                    break;

            }
        }

        private void PasswordDialog_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
