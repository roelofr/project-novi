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
            TTS.TestTTS hoi = new TTS.TestTTS();
            hoi.Show();
        }
    }
}
