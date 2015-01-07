using System;
using System.Drawing;
using System.Text;
using System.Xml;

namespace News
{
    internal class RssEntry
    {
        /// <summary>
        ///     Creates a new RssEntry, which is a container for data for a single entry of an RSS feed
        /// </summary>
        /// <param name="title">The title of this feed item</param>
        /// <param name="content">The content of this feed item</param>
        /// <param name="timestamp">The timestamp of this feed item</param>
        /// <param name="channel">The channel name of this feed item</param>
        /// <param name="url">The URL of the feed</param>
        public RssEntry(String title, String content, DateTime timestamp, String channel, String URL)
        {
            Title = title;
            Content = content;
            Timestamp = timestamp;
            Channel = channel;

            ImageHandler = RssImageHandler.Get(URL);
        }

        public String Title { get; private set; }
        public String Content { get; private set; }
        public DateTime Timestamp { get; private set; }
        public String Channel { get; private set; }

        public RssImageHandler ImageHandler { get; private set; }

        public Image Image
        {
            get { return ImageHandler != null && ImageHandler.Exists ? ImageHandler.Image : null; }
        }

        /// <summary>
        ///     Tries to parse a string date into a datetime object.
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(String dateString)
        {
            var standardOut = new DateTime(1970, 1, 1, 0, 0, 0);
            DateTime dtOut;
            return DateTime.TryParse(dateString, out dtOut) ? dtOut : standardOut;
        }

        public static RssEntry GetFromXML(XmlNode node, String feedURL)
        {
            var titleNode = node.SelectSingleNode("title");
            var contentNode = node.SelectSingleNode("content");
            var dateNode = node.SelectSingleNode("date");
            var chnlNode = node.SelectSingleNode("channel");

            var title = titleNode != null ? titleNode.InnerText : null;
            var content = contentNode != null ? contentNode.InnerText : null;
            var date = dateNode != null ? dateNode.InnerText : null;
            var channel = chnlNode != null ? chnlNode.InnerText : null;

            if (String.IsNullOrEmpty(title) || String.IsNullOrEmpty(content) || String.IsNullOrEmpty(date) ||
                String.IsNullOrEmpty(channel))
                return null;

            var dateObj = GetDateTime(date);
            return dateObj.Year < 2000 ? null : new RssEntry(title, content, dateObj, channel, feedURL);
        }

        /// <summary>
        ///     Purges the HTML from a string, making it safe to display and disabling any beacons that are embedded in the RSS
        ///     feed.
        /// </summary>
        /// <param name="unsafeString">The unsafe string that needs to be cleaned up</param>
        /// <returns></returns>
        /// <seealso cref="http://www.dotnetperls.com/remove-html-tags" />
        public static String PurgeHtml(String unsafeString)
        {
            var array = new char[unsafeString.Length];
            var arrayIndex = 0;
            var inside = false;

            for (var i = 0; i < unsafeString.Length; i++)
            {
                var let = unsafeString[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                // Check for escaped HTML tags, which will make the tag show up (which is even more annoying)
                if (let == '&')
                {
                    if (unsafeString.Substring(i + 1, 3) == "lt;")
                    {
                        i += 3;
                        inside = true;
                        continue;
                    }
                    if (unsafeString.Substring(i + 1, 3) == "rt;")
                    {
                        i += 3;
                        inside = false;
                        continue;
                    }
                }
                if (inside) continue;
                array[arrayIndex] = @let;
                arrayIndex++;
            }
            return new string(array, 0, arrayIndex);
        }

        public static String Utf8ToIso(String utf8String)
        {
            var iso = Encoding.GetEncoding("ISO-8859-1");
            var utf8 = Encoding.UTF8;
            var utfBytes = iso.GetBytes(utf8String);
            var isoBytes = Encoding.Convert(iso, utf8, utfBytes);
            return utf8.GetString(isoBytes);
        }
    }
}