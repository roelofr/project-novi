using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Project_Novi.TTS
{
    class TTS
    {
        //private const string URL = "http://translate.google.com/translate_tts?tl={0}&q={1}";
        //string strURL;
        //string text;
        
        //public TestTTS()
        //{
        //    InitializeComponent();
        //    text = "Welkom in gebouw t!";
        //}

        //private void BuildURL()
        //{
        //    strURL = string.Format(URL, "nl", text.ToLower().Replace(" ", "%20"));
        //}

        //private void GenerateSpeechFromText()
        //{
        //    WebClient serviceRequest = new WebClient();
        //    serviceRequest.DownloadDataCompleted += serviceRequest_DownloadDataCompleted;

        //    try
        //    {
        //        serviceRequest.DownloadDataAsync(new Uri(strURL));
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("Tekst is te lang!");
        //    }
        //}


        //void serviceRequest_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        //{
        //    if (e.Error == null && e.Result != null)
        //    {
        //            // play MP3 using nAudio lib
        //            PlayMP3(e.Result);
        //    }
        //    else
        //    {
        //        //te lang
        //        MessageBox.Show("Ik kan dit niet uitspreken!");
        //    }
        //}

        //private void PlayMP3(byte[] soundDataArray)
        //{
        //    Stream stream = new MemoryStream(soundDataArray);
        //    if (stream != null)
        //    {
        //        Mp3FileReader reader = new Mp3FileReader(stream);
        //        var waveOut = new WaveOut();
        //        waveOut.Init(reader);
        //        waveOut.Play();
        //    }
        //}

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    BuildURL();
        //    GenerateSpeechFromText();
        //}
    }
}
