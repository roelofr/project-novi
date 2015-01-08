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

        private const string CredentialFile = "nsApiCredentials.xml";
        public const string DefaultStation = "Zwolle";
        private const string CacheFileTemplate = "nsCache-{0}.xml";
        private const string ApiCredentials = "nsApiCredentials.xml";
        private const string ApiPath = "http://webservices.ns.nl/ns-api-avt?station={0}";
        private static readonly Regex NonFileRegex = new Regex("([^a-zA-Z0-9\\-]+)");
        private string _lastDownloaded;
        private readonly WebClient _client;

        public ApiComm()
        {
            var clientCred = GetApiCredentials();

            if (clientCred == null)
            {
                throw new PlatformNotSupportedException();
            }

            _client = new WebClient();
            _client.Credentials = clientCred;
            _client.DownloadStringCompleted += DownloadCompleted;
        }

        public event NsDataAvailableEvent NsDataAvailable;

        private static void WriteDefaultApiCredentials()
        {
            using (var xmlWriter = XmlWriter.Create(CredentialFile))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("loginData");

                xmlWriter.WriteElementString("user", "***REMOVED***");
                xmlWriter.WriteElementString("pass", "***REMOVED***");

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }
        }

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

        public string GetFile(string station)
        {
            if (string.IsNullOrWhiteSpace(station))
                return null;
            var formattedStation = NonFileRegex.Replace(station, "-");
            return string.Format(CacheFileTemplate, formattedStation);
        }

        public void GetInformation(string station)
        {
            if (GetFromCache(station, false))
                return;
            if (GetFromWeb(station))
                return;
            GetFromCache(station, true);
        }

        public void GetInformation()
        {
            GetInformation(DefaultStation);
        }

        /// <summary>
        ///     Gets the information from the cache file, optionally ignoring if the cache expired (in case of connection failure)
        /// </summary>
        /// <param name="station"></param>
        /// <param name="ignoreExpire"></param>
        /// <returns></returns>
        public bool GetFromCache(string station, bool ignoreExpire)
        {
            var file = GetFile(station);

            if (!File.Exists(file))
                return false;
            if (!ignoreExpire && (DateTime.UtcNow - File.GetLastWriteTimeUtc(file)).TotalMinutes > 5)
                return false;

            var data = File.ReadAllText(file);

            UseData(data, false, station);

            return true;
        }

        public bool GetFromWeb(string station)
        {
            if (_lastDownloaded != null || string.IsNullOrWhiteSpace(station))
                return false;

            _lastDownloaded = station;

            try
            {
                var url = new Uri(string.Format(ApiPath, station));
                _client.DownloadStringAsync(url);
                return true;
            }
            catch (Exception exception)
            {
                return false;
            }
        }

        private void putInCache(string data, string station)
        {
            if (string.IsNullOrWhiteSpace(data) || string.IsNullOrWhiteSpace(station))
                return;

            var file = GetFile(station);
            try
            {
                File.WriteAllText(file, data);
            }
            catch
            {
                // Do nothing on failure, too bad
            }
        }

        private void UseData(string data, bool cachable, string station)
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


            if (cachable && station != null)
                putInCache(data, station);

            if (NsDataAvailable != null)
            {
                NsDataAvailable(XmlDoc, station ?? DefaultStation);
            }
        }

        private void DownloadCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            UseData(e.Result, true, _lastDownloaded);
            _lastDownloaded = null;
        }
    }
}