using NAudio.Wave;
using System;
using System.IO;
using System.Net;

namespace Project_Novi.TTS
{
    /// <summary>
    /// Allows you to generate audio from text input.
    /// Use the method TextToSpeech() to speak text.
    /// 
    /// This class calls out to Google Translate to generate speech.
    /// </summary>
    static class TTS
    {
        private const string BaseUrl = "http://translate.google.com/translate_tts?tl={0}&q={1}";

        /// <summary>
        /// Keep track of a list of sentences to speak.
        /// This is needed because long text can't be submitted to Google Translate in one go, but has to be split up.
        /// </summary>
        private static string[] _sentences;

        /// <summary>
        /// The index of the currently spoken sentence.
        /// Sometimes later parts of a text arrive before earlier parts, jumbling up the spoken order.
        /// To fix that every sentence is requested only when the previous one has finished.
        /// </summary>
        private static int _counter;

        internal delegate void PlaybackFinished();

        private static PlaybackFinished _finishedCallback;

        /// <summary>
        /// Build the Url to make a request to Google Translate.
        /// </summary>
        private static string BuildUrl(string text)
        {
            return string.Format(BaseUrl, "nl", Uri.EscapeUriString(text.ToLower()));
        }

        /// <summary>
        /// Make a request to Google Translate based on the variables _sentences and _counter.
        /// </summary>
        private static void GenerateSpeechFromText()
        {
            if (_sentences == null || _counter >= _sentences.Length) return;

            var serviceRequest = new WebClient();
            serviceRequest.DownloadDataCompleted += serviceRequest_DownloadDataCompleted;
            try
            {
                serviceRequest.DownloadDataAsync(new Uri(BuildUrl(_sentences[_counter])));
            }
            catch (Exception)
            {
                Console.Write("Tekst is te lang!");
            }
        }

        /// <summary>
        /// Called when a request completes. Starts playing the result.
        /// </summary>
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

        /// <summary>
        /// Play audio from an array of MP3 bytes.
        /// </summary>
        /// <param name="soundDataArray">The bytes containing a valid MP3 sequence</param>
        private static void PlayMP3(byte[] soundDataArray)
        {
            Stream stream = new MemoryStream(soundDataArray);
            var reader = new Mp3FileReader(stream);
            var directSoundOut = new DirectSoundOut();
            directSoundOut.Init(reader);
            directSoundOut.Play();
            directSoundOut.PlaybackStopped += DirectSoundOut_PlaybackStopped;
        }

        /// <summary>
        /// Request the next sentence when the previous one has finished playing.
        /// </summary>
        private static void DirectSoundOut_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            _counter++;
            if (_counter >= _sentences.Length - 1 && _finishedCallback != null)
            {
                _finishedCallback();
                _finishedCallback = null;
            }
            else
            {
                GenerateSpeechFromText();
            }
        }


        /// <summary>
        /// Split up the text into sentences and start playing the first sentence.
        /// </summary>
        public static void TextToSpeech(string text)
        {
            char[] splitters = { ',', '.', '?', '!' };
            _sentences = text.Split(splitters);
            GenerateSpeechFromText();
        }

        public static void TextToSpeech(string text, PlaybackFinished finished)
        {
            _finishedCallback = finished;
            TextToSpeech(text);
        }
    }
}
