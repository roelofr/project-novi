using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Timers;
using System.Xml;

namespace News
{
    internal class RssHandler
    {
        public delegate void DownloadCompleteEvent(String url, String data);

        public delegate void DownloadFailedEvent(String url, String errorData);

        private const double RequestTimeout = 1500d;
        private bool _downloading;
        private readonly WebClient _client;
        private readonly List<String> _downloadQueue = new List<string>();
        private readonly Timer _webClientTimer;

        public RssHandler()
        {
            _client = new WebClient();
            _client.Encoding = Encoding.UTF8;
            _client.DownloadStringCompleted += DownloadComplete;

            _webClientTimer = new Timer
            {
                AutoReset = true, // It's safe to auto-reset, on failure the timer will stop anyway
                Interval = RequestTimeout, // Timeout after 1.5 seconds
                Enabled = false // Don't start just yet
            };
            _webClientTimer.Elapsed += DownloadFailed;
        }

        public event DownloadCompleteEvent RssDownloadFinished;
        public event DownloadFailedEvent RssDownloadFailed;

        /// <summary>
        ///     Fires when a download is finished, doesn't actually parse the content, just forwards it and starts a new download
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadComplete(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
                return;

            if (e.Cancelled)
                return;

            _webClientTimer.Stop();
            _webClientTimer.Interval = RequestTimeout;

            var lastUsedUrl = _downloadQueue[0];
            _downloadQueue.RemoveAt(0);

            if (RssDownloadFinished != null)
                RssDownloadFinished(lastUsedUrl ?? "none", e.Result);

            _downloading = false;

            DownloadNext();
        }

        /// <summary>
        ///     Fired when a download failes (due to timeout or w/e)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadFailed(object sender, ElapsedEventArgs e)
        {
            _webClientTimer.Stop();
            _client.CancelAsync();

            var lastUsedUrl = _downloadQueue[0];
            _downloadQueue.RemoveAt(0);

            if (RssDownloadFailed != null)
                RssDownloadFailed(lastUsedUrl ?? "none", "Request timed out");

            _downloading = false;

            DownloadNext();
        }

        /// <summary>
        ///     Triest to start the next download, if nothing is downloading and there is something in the queue, this will work
        /// </summary>
        public void DownloadNext()
        {
            if (_downloading)
                return;

            if (_downloadQueue.Count == 0)
                return;

            _downloading = true;

            _webClientTimer.Start();

            _client.DownloadStringAsync(new Uri(_downloadQueue[0]));
        }

        /// <summary>
        ///     Downloads the contents of the given URL, starting a new download thread if required
        /// </summary>
        /// <param name="url"></param>
        public void Download(string url)
        {
            if (InQueue(url))
                return;

            _downloadQueue.Add(url);

            DownloadNext();
        }

        /// <summary>
        ///     Returns TRUE if an item is in the download queue
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool InQueue(string url)
        {
            return _downloadQueue.Contains(url);
        }

        /// <summary>
        ///     Returns a list of RssEntry objects after processing the RSS data. Returns NULL if anything goes wrong.
        /// </summary>
        /// <param name="feedData"></param>
        /// <param name="feedUrl"></param>
        /// <returns></returns>
        public static List<RssEntry> ParseRssEntries(String feedData, String feedUrl)
        {
            var xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.LoadXml(feedData);
            }
            catch
            {
                return null;
            }

            var body = xmlDocument.DocumentElement;
            if (body == null)
                return null;

            var items = body.SelectNodes("/rss/channel/item");

            if (items == null)
                return null;


            var channelNode = body.SelectSingleNode("/rss/channel/title");
            var iconNode = body.SelectSingleNode("/rss/channel/image/url");

            var channelName = channelNode != null ? channelNode.InnerText : "Feed";
            var iconUrl = iconNode != null ? iconNode.InnerText : null;

            if (!String.IsNullOrEmpty(iconUrl))
            {
                var handler = RssImageHandler.Get(feedUrl);
                if (!handler.Exists)
                    handler.DownloadAsset(iconUrl);
            }

            var output = new List<RssEntry>();

            foreach (XmlNode item in items)
            {
                var titleNode = item.SelectSingleNode("title");
                var descNode = item.SelectSingleNode("description");
                var dateNode = item.SelectSingleNode("pubDate");

                if (titleNode == null || descNode == null || dateNode == null)
                    continue;

                var title = RssEntry.Utf8ToIso(titleNode.InnerText);
                var desc = RssEntry.Utf8ToIso(RssEntry.PurgeHtml(descNode.InnerText));
                var date = RssEntry.GetDateTime(dateNode.InnerText);

                output.Add(new RssEntry(title, desc, date, channelName, feedUrl));
            }

            return output;
        }
    }
}