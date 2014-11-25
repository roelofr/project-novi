using System;
using System.Windows.Forms;
using Project_Novi.Modules;

namespace Project_Novi
{
    public partial class Novi : Form
    {
        private NoviController _controller;
        internal IView View { get; set; }

        public Novi()
        {
            InitializeComponent();
            TTS.TTS TTS = new TTS.TTS();
            TTS.TextToSpeech("Dit is een hele lange zin zonder wat dan ook en ik ga nog even lekker door misschien haal ik wel honderd karakters of misschien ook niet dat zullen we wel zien");
            //TTS.TestTTS hoi = new TTS.TestTTS();
            //hoi.Show();
            Hide();
            var splash = new Splash();
            splash.ShowDialog();
            Show();
            _controller = new NoviController(this);
        }

        private void Novi_Paint(object sender, PaintEventArgs e)
        {
            if (!Visible) return;

            var rect = e.ClipRectangle;
            var g = e.Graphics;

            // Automatically scale drawing to the size of the form
            var scaleX = (float)(rect.Width / 1920d);
            var scaleY = (float)(rect.Height / 1080d);
            var scale = Math.Min(scaleX, scaleY);
            g.ScaleTransform(scale, scale);

            View.Render(g, rect);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Tekst.TextDisplayer text = new Tekst.TextDisplayer();
            text.Show();
        }
    }
}
