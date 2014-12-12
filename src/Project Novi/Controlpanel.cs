using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Project_Novi
{
    public partial class Controlpanel : Form
    {
        private string _usernameTwitter;
        public Controlpanel()
        {
            InitializeComponent();
           

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            _usernameTwitter = textboxUsernameTwitter1.Text;
        }

        private void Controlpanel_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    Close();
                    break;
                case Keys.Escape:
                    Close();
                    break;
            }
        }
    }
}
