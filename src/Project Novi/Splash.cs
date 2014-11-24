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
    public class Splash : Form
    {
        Timer closeTimer;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeWindow()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Text = "Project novi";
            this.BackColor = Color.FromArgb(166, 166, 166);
            this.ControlBox = false;

            this.BackgroundImage = Properties.Resources.novi_logo;
            this.BackgroundImageLayout = ImageLayout.Center;

            this.Size = new Size(300, 150);
            this.AutoSize = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            this.CenterToScreen();

            this.Load += SplashLoaded;
        }

        public Splash()
        {
            InitializeWindow();
        }

        /// <summary>
        /// Triggered when the splash screen is loaded. it starts a timer so the splash closes after a certain interval.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SplashLoaded(object sender, EventArgs e)
        {
            closeTimer = new Timer();
            closeTimer.Interval = 1000 * 4;
            closeTimer.Tick += closeTimerTrigger;
            closeTimer.Start();
        }

        /// <summary>
        /// Closes the window at the first tick of the timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void closeTimerTrigger(object sender, EventArgs e)
        {
            closeTimer.Stop();
            this.Close();
        }


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
    }
}
