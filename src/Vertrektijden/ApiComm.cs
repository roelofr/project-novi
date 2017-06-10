using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;

namespace Vertrektijden
{
    public class ApiComm
    {
        public delegate void NsDataAvailableEvent(XmlDocument document, string station);

        private const string CredentialFile = "nsApi.xml";
        private const string CacheFileTemplate = "nsCache-{0}.xml";
        private const string ApiPath = "http://webservices.ns.nl/ns-api-avt?station={0}";
        private static readonly Regex NonFileRegex = new Regex("([^a-zA-Z0-9\\-]+)");
        private string _lastDownloaded;
        private readonly WebClient _client;
        /// <summary>
        /// Creates a new API handler, usng the given station as station name.
        /// </summary>
        /// <param name="station">The name to parse, is forwarded to the NS API</param>
        public ApiComm(string station)
        {
            var clientCred = GetApiCredentials();

            if (clientCred == null)
            {
                throw new PlatformNotSupportedException();
            }

            StationName = station;
            LastModifiedTime = new DateTime(1970,1,1);

            _client = new WebClient { Credentials = clientCred };
            _client.DownloadStringCompleted += DownloadCompleted;
        }

        public string StationName { get; private set; }
        public DateTime LastModifiedTime { get; private set; }
        public event NsDataAvailableEvent NsDataAvailable;
        /// <summary>
        /// Writes the default data to log in with to the settings file, in case it's deleted
        /// </summary>
        private static void WriteDefaultApiCredentials()
        {
            using (var xmlWriter = XmlWriter.Create(CredentialFile))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("loginData");

                xmlWriter.WriteElementString("user", "john.doe@example.com");
                xmlWriter.WriteElementString("pass", "mypasword");

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }
        }
        /// <summary>
        /// Returns a NetworkCredential manager containing the API credentials, or NULL if not available
        /// </summary>
        /// <returns></returns>
        private static NetworkCredential GetApiCredentials()
        {
            if (!File.Exists(CredentialFile))
            {
                WriteDefaultApiCredentials();
            }
            var xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.Load(CredentialFile);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Exception! {0}", e.Message);
                return null;
            }

            var body = xmlDocument.DocumentElement;
            var userNode = body != null ? body.SelectSingleNode("/loginData/user") : null;
            var passNode = body != null ? body.SelectSingleNode("/loginData/pass") : null;

            if (userNode == null || passNode == null)
                return null;


            var user = userNode.InnerText;
            var pass = passNode.InnerText;

            if (String.IsNullOrWhiteSpace(user) || String.IsNullOrWhiteSpace(pass))
                return null;

            return new NetworkCredential(user, pass);
        }
        /// <summary>
        /// Returns the name of the file which is used for caching
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        private string GetFile(string station)
        {
            if (string.IsNullOrWhiteSpace(station))
                return null;
            var formattedStation = NonFileRegex.Replace(station, "-");
            return string.Format(CacheFileTemplate, formattedStation);
        }
        /// <summary>
        /// Gets the information, using the cache if it is valid, then the web data and if that fails, falls back to the cache file ignoring it's validity
        /// </summary>
        public void GetInformation()
        {
            if (GetFromCache(false))
                return;
            if (GetFromWeb())
                return;
            GetFromCache(true);
        }

        /// <summary>
        ///     Gets the information from the cache file, optionally ignoring if the cache expired (in case of connection failure)
        /// </summary>
        /// <param name="ignoreExpire"></param>
        /// <returns></returns>
        public bool GetFromCache(bool ignoreExpire)
        {
            var file = GetFile(StationName);

            if (!File.Exists(file))
                return false;
            if (!ignoreExpire && (DateTime.UtcNow - File.GetLastWriteTimeUtc(file)).TotalMinutes > 2)
                return false;

            var data = File.ReadAllText(file);

            UseData(data, false, File.GetLastWriteTime(file));

            return true;
        }
        /// <summary>
        /// Gets the data for this station from the NS API
        /// </summary>
        /// <returns></returns>
        public bool GetFromWeb()
        {
            if (_lastDownloaded != null)
                return false;

            try
            {
                var url = new Uri(string.Format(ApiPath, StationName));
                _client.DownloadStringAsync(url);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Puts the retrieved content in this station's cache file.
        /// </summary>
        /// <param name="data"></param>
        private void PutInCache(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return;

            var file = GetFile(StationName);
            try
            {
                File.WriteAllText(file, data);
            }
            catch
            {
                // Do nothing on failure, too bad
            }
        }
        /// <summary>
        /// Checks if the given data is valid XML and if it is, caches it if required and then forwards it to the event handlers.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cachable"></param>
        /// <param name="lastUpdatedTime"></param>
        private void UseData(string data, bool cachable, DateTime lastUpdatedTime)
        {
            if (String.IsNullOrWhiteSpace(data))
                return;

            var XmlDoc = new XmlDocument();
            try
            {
                XmlDoc.LoadXml(data);
            }
            catch
            {
                return;
            }


            if (cachable)
                PutInCache(data);

            LastModifiedTime = lastUpdatedTime;

            if (NsDataAvailable != null)
            {
                NsDataAvailable(XmlDoc, StationName);
            }
        }
        /// <summary>
        /// Fired when the download is completed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
                return;

            if (e.Cancelled)
                return;

            UseData(e.Result, true, DateTime.Now);
            _lastDownloaded = null;
        }
    }
}