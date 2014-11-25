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
        private const string URL = "http://translate.google.com/translate_tts?tl={0}&q={1}";
        private string strURL;
        private string[] sentences;
        private int counter;

        public TTS()
        {
        }

        private void BuildURL(string text)
        {
            strURL = string.Format(URL, "nl", text.ToLower().Replace(" ", "%20"));
        }

        private void GenerateSpeechFromText()
        {
            if (sentences != null)
            {
                if (counter < sentences.Length)
                {
                    BuildURL(sentences[counter]);
                    WebClient serviceRequest = new WebClient();
                    serviceRequest.DownloadDataCompleted += serviceRequest_DownloadDataCompleted;
                    try
                    {
                        serviceRequest.DownloadDataAsync(new Uri(strURL));
                    }
                    catch (Exception)
                    {
                        Console.Write("Tekst is te lang!");
                    }
                }                
            }            
            
        }


        void serviceRequest_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            if (e.Error == null && e.Result != null)
            {
                    // play MP3 using nAudio lib
                PlayMP3(e.Result);
            }
            else
            {
                //te lang
                Console.Write("Ik kan dit niet uitspreken!");
            }
        }

        private void PlayMP3(byte[] soundDataArray)
        {
            Stream stream = new MemoryStream(soundDataArray);
            if (stream != null)
            {
                Mp3FileReader reader = new Mp3FileReader(stream);
                var DirectSoundOut = new DirectSoundOut();
                DirectSoundOut.Init(reader);
                DirectSoundOut.Play();
                DirectSoundOut.PlaybackStopped += DirectSoundOut_PlaybackStopped;                
            }
        }

        void DirectSoundOut_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            counter++;
            GenerateSpeechFromText();
        }

        

        public void TextToSpeech(string text)
        {
            
            //var regex = new System.Text.RegularExpressions.Regex("[,.?!]");
            //var sentences = regex.Split(text);
            char[] splitters = { ',', '.', '?', '!' };
            sentences = text.Split(splitters);
            GenerateSpeechFromText();            
        }
    }
}
