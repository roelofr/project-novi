using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace News
{
    /// <summary>
    ///     Handles the download of the little icons for each RSS feed.
    /// </summary>
    internal class RssImageHandler : CacheCrypto
    {

        private static Dictionary<String, RssImageHandler> cacheDictionary = new Dictionary<string, RssImageHandler>();

        public static RssImageHandler Get(String Url)
        {
            if (cacheDictionary.ContainsKey(Url))
                return cacheDictionary[Url];

            var handler = new RssImageHandler(Url);
            cacheDictionary.Add(Url, handler);
            return handler;
        }

        private const string StorageFileTemplate = "icon-{0}.png";
        private readonly String _storageFile;

        /// <summary>
        ///     Creates a new RSS image handler, the feed URL is the URL of the RSS feed, which is used for the caching of icons.
        /// </summary>
        /// <param name="feedURL"></param>
        private RssImageHandler(String feedURL)
        {
            var fileHash = GetHash(feedURL);
            var fileName = String.Format(StorageFileTemplate, fileHash);
            _storageFile = Path.Combine(NewsSettings.BasePath, fileName);

            Image = null;

            ReadFromDisk();
        }

        public Image Image { get; private set; }

        public Boolean Exists
        {
            get { return File.Exists(_storageFile); }
        }

        private void DownloadCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            var memoryStream = new MemoryStream(e.Result);
            Image outImage;
            try
            {
                outImage = Image.FromStream(memoryStream);
            }
            catch
            {
                return;
            }
            finally
            {
                memoryStream.Dispose();
            }

            try
            {
                outImage.Save(_storageFile, ImageFormat.Png);

            }
            catch
            {

            }
            ReadFromDisk();
        }

        public void DownloadAsset(String assetUrl)
        {
            Console.WriteLine("Downloading {0}...", assetUrl);
            var client = new WebClient();
            client.DownloadDataCompleted += DownloadCompleted;
            client.DownloadDataAsync(new Uri(assetUrl));
        }

        private void ReadFromDisk()
        {
            if (Exists && Image == null)
            {
                try
                {
                    Image = Image.FromFile(_storageFile);
                }
                catch
                {
                    Image = null;
                    File.Delete(_storageFile);
                }
            }
        }
    }
}