﻿using System;
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
    public partial class Novi : Form
    {
        public Novi()
        {
            InitializeComponent();

            this.Visible = false;

            this.Load += OpenSplashWindow;
        }

        void OpenSplashWindow(object sender, EventArgs e)
        {
            Splash splash = new Splash();
            splash.ShowDialog();

            this.Visible = true;
        }
    }
}
