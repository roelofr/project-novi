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
    public partial class Novi : Form
    {
        public Novi()
        {
            InitializeComponent();
            TTS.TTS TTS = new TTS.TTS();
            TTS.TextToSpeech("Dit is een hele lange zin zonder wat dan ook en ik ga nog even lekker door misschien haal ik wel honderd karakters of misschien ook niet dat zullen we wel zien");
            //TTS.TestTTS hoi = new TTS.TestTTS();
            //hoi.Show();
        }
    }
}
