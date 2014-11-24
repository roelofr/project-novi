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
        int visibleTime { get { return 400; } }

        int visibleFor;

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
        }

        public Splash()
        {
            InitializeWindow();

            visibleFor = 0;

            this.Load += SplashLoaded;
            this.KeyDown += Debug_KeyPressed;
            this.Opacity = 0;
        }

        /// <summary>
        /// Triggered when the splash screen is loaded. it starts a timer so the splash closes after a certain interval.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SplashLoaded(object sender, EventArgs e)
        {
            closeTimer = new Timer();
            closeTimer.Interval = 10;
            closeTimer.Tick += animationTick;
            closeTimer.Start();
        }

        /// <summary>
        /// Closes the window at the first tick of the timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void animationTick(object sender, EventArgs e)
        {
            visibleFor += closeTimer.Interval;

            if (visibleFor > visibleTime)
            {
                closeTimer.Stop();
                return;
            }

            this.Opacity = Math.Min(1, (float)visibleFor / (float)visibleTime);
        }
        /// <summary>
        /// Debug functionality that closes the splash if a key is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Debug_KeyPressed(object sender, KeyEventArgs e)
        {
            if (closeTimer.Enabled)
                return;
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
