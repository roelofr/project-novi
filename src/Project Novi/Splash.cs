using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project_Novi
{
    public class Splash : Form
    {
        Timer _closeTimer;
        private const int VisibleTime = 400;

        int _visibleFor;

        private void InitializeWindow()
        {
            AutoScaleMode = AutoScaleMode.Font;
            Text = "Project Novi";
            BackColor = Color.FromArgb(166, 166, 166);
            ControlBox = false;

            BackgroundImage = Properties.Resources.novi_logo;
            BackgroundImageLayout = ImageLayout.Center;

            Size = new Size(300, 150);
            AutoSize = false;
            FormBorderStyle = FormBorderStyle.None;

            CenterToScreen();
        }

        public Splash()
        {
            InitializeWindow();

            _visibleFor = 0;

            Load += SplashLoaded;
            KeyDown += Debug_KeyPressed;
            MouseDown += Debug_MouseClicked;
            Opacity = 0;
        }

        /// <summary>
        /// Triggered when the splash screen is loaded. it starts a timer so the splash closes after a certain interval.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SplashLoaded(object sender, EventArgs e)
        {
            _closeTimer = new Timer { Interval = 10 };
            _closeTimer.Tick += AnimationTick;
            _closeTimer.Start();
        }

        /// <summary>
        /// Increases the opacity of the window every tick.
        /// Stops fading in when the window is fully visible.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AnimationTick(object sender, EventArgs e)
        {
            _visibleFor += _closeTimer.Interval;

            if (_visibleFor > VisibleTime)
            {
                _closeTimer.Stop();
                return;
            }

            Opacity = Math.Min(1, (float)_visibleFor / VisibleTime);
        }

        /// <summary>
        /// Debug functionality that closes the splash if a key is pressed.
        /// </summary>
        void Debug_KeyPressed(object sender, KeyEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Debug functionality that closes the splash if a mouse button is pressed.
        /// </summary>
        void Debug_MouseClicked(object sender, MouseEventArgs e)
        {
            Close();
        }
    }
}
