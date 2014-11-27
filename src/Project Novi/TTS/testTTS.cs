using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Novi.TTS
{
    public partial class TestTTS : Form
    {
        private const string URL = "http://translate.google.com/translate_tts?tl={0}&q={1}";
        string strURL;
        string text;
        
        public TestTTS()
        {
            InitializeComponent();
            text = "Welkom in gebouw t!";
        }

        //building the url to format text
        private void BuildURL()
        {
            strURL = string.Format(URL, "nl", textBox1.Text.ToLower().Replace(" ", "%20"));
        }

        //requesting the Google Translate service
        private void GenerateSpeechFromText()
        {
            WebClient serviceRequest = new WebClient();
            serviceRequest.DownloadDataCompleted += serviceRequest_DownloadDataCompleted;

            try
            {
                serviceRequest.DownloadDataAsync(new Uri(strURL));
            }
            catch (Exception)
            {
                MessageBox.Show("Tekst is te lang!");
            }
        }

        //When file downloaded start audio
        void serviceRequest_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            if (e.Error == null && e.Result != null)
            {
                    PlayMP3(e.Result);
            }
            else
            {
                //Service returns null or input more than 100 char
                MessageBox.Show("Ik kan dit niet uitspreken!");
            }
        }

        //playing audio
        private void PlayMP3(byte[] soundDataArray)
        {
            Stream stream = new MemoryStream(soundDataArray);
            if (stream != null)
            {
                Mp3FileReader reader = new Mp3FileReader(stream);
                var waveOut = new WaveOut();
                waveOut.Init(reader);
                waveOut.Play();
            }
        }

        //for testing purposes
        private void button1_Click(object sender, EventArgs e)
        {
            BuildURL();
            GenerateSpeechFromText();
        }
    }
}



