using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace News
{
    internal class CacheHandler : CacheCrypto
    {
        /// <summary>
        ///     The file template for our cache files. The first argument is replaced with a hash from the URL
        /// </summary>
        private const String CacheFile = "Cache-{0}.xml";

        /// <summary>
        ///     How long should the cache be valid. 5 minutes by default.
        /// </summary>
        private const long CacheTimeout = 300L;

        /// <summary>
        ///     Returns TRUE if the cache file exists
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool Contains(String url)
        {
            var filename = String.Format(CacheFile, GetHash(url));
            var path = Path.Combine(NewsSettings.BasePath, filename);
            if (!File.Exists(path))
                return false;

            var difference = (long) (DateTime.UtcNow - File.GetLastWriteTimeUtc(path)).TotalSeconds;
            return difference < CacheTimeout;
        }

        /// <summary>
        ///     Reads the feed entries from cache file, returning a list or NULL on error
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static List<RssEntry> ReadFromCache(String url)
        {
            var filename = String.Format(CacheFile, GetHash(url));
            var path = Path.Combine(NewsSettings.BasePath, filename);

            if (!File.Exists(path))
                return null;

            var xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.Load(path);
            }
            catch
            {
                return null;
            }

            var docBody = xmlDocument.DocumentElement;

            var nodeList = docBody != null ? docBody.SelectNodes("/cache/feed") : null;

            if (nodeList == null)
                return null;

            var output = new List<RssEntry>();

            foreach (XmlNode node in nodeList)
            {
                var rssNode = RssEntry.GetFromXML(node, url);
                if (rssNode != null)
                    output.Add(rssNode);
            }

            return output.Count == 0 ? null : output;
        }

        /// <summary>
        ///     Writes the feed entries to the cache file.
        ///     Should only be called from the web download or else the cache file will remain valid with outdated data.
        /// </summary>
        /// <param name="url">The URL of the feed</param>
        /// <param name="entries"></param>
        /// <returns></returns>
        public static bool WriteToCache(String url, IEnumerable<RssEntry> entries)
        {
            var filename = String.Format(CacheFile, GetHash(url));
            var path = Path.Combine(NewsSettings.BasePath, filename);
            // Use Using so system resoruces are always freed
            using (var xmlDocument = XmlWriter.Create(path))
            {
                xmlDocument.WriteStartDocument();
                xmlDocument.WriteStartElement("cache");

                foreach (var feed in entries)
                {
                    xmlDocument.WriteStartElement("feed");
                    xmlDocument.WriteElementString("title", feed.Title);
                    xmlDocument.WriteElementString("content", feed.Content);
                    xmlDocument.WriteElementString("date", feed.Timestamp.ToString(CultureInfo.CurrentCulture));
                    xmlDocument.WriteElementString("channel", feed.Channel);
                    xmlDocument.WriteEndElement();
                }

                xmlDocument.WriteEndElement();
                xmlDocument.WriteEndDocument();
            }
            return true;
        }
    }
}