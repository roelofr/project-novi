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
    // This class allows you to generate audio from text input:
    //  Use method TextToSpeech() to input a text. 

    static class TTS
    {
        private const string URL = "http://translate.google.com/translate_tts?tl={0}&q={1}";
        private static string strURL;
        private static string[] sentences;
        private static int counter;


        //building the url to format text
        private static void BuildURL(string text)
        {
            strURL = string.Format(URL, "nl", text.ToLower().Replace(" ", "%20"));
        }

        //requesting the Google Translate service
        private static void GenerateSpeechFromText()
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

        //When file downloaded start audio
        private static void serviceRequest_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            if (e.Error == null && e.Result != null)
            {
                PlayMP3(e.Result);
            }
            else
            {
                //Service returns null or input more than 100 char
                Console.Write("Ik kan dit niet uitspreken!");
            }
        }

        //play audio
        private static void PlayMP3(byte[] soundDataArray)
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
            else
            {
                Console.WriteLine("Stupid fool");
            }
        }

        //when playing audio completed, generate next sentence
        private static void DirectSoundOut_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            counter++;
            GenerateSpeechFromText();
        }

        
        //split input string in sentences and generate first sentence
        public static void TextToSpeech(string text)
        {
            char[] splitters = { ',', '.', '?', '!' };
            sentences = text.Split(splitters);
            GenerateSpeechFromText();            
        }
    }
}
