using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace News
{
    internal class NewsSettings
    {
        /// <summary>
        ///     The file in which our settings are stored
        /// </summary>
        private const String SettingsFile = "RssFeeds.xml";

        /// <summary>
        ///     Get a path to the app data folder, so we can put our data somewhere
        /// </summary>
        public static readonly String BasePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Project Novi");

        /// <summary>
        ///     The list with URLs to our RSS feeds
        /// </summary>
        private List<String> _urlList = new List<string>();

        /// <summary>
        ///     Checks if the settings file exists
        /// </summary>
        /// <returns>TRUE if the settings file exists, FALSE otherwise</returns>
        private bool CreateSettingsDirectory()
        {
            var directoryName = Path.GetDirectoryName(Path.Combine(BasePath, SettingsFile));
            if (directoryName == null)
                return false;
            if (!Directory.Exists(directoryName))
            {
                try
                {
                    Directory.CreateDirectory(directoryName);
                }
                catch
                {
                    return false;
                }
            }

            return Directory.Exists(directoryName);
        }

        /// <summary>
        ///     Writes the settings to the settings file
        /// </summary>
        /// <returns></returns>
        private bool WriteSettings()
        {
            if (!CreateSettingsDirectory())
                return false;

            // Use Using so system resoruces are always freed
            using (var xmlDocument = XmlWriter.Create(Path.Combine(BasePath, SettingsFile)))
            {
                xmlDocument.WriteStartDocument();
                xmlDocument.WriteStartElement("feeds");

                foreach (var feed in _urlList)
                {
                    xmlDocument.WriteElementString("feed", feed);
                }

                xmlDocument.WriteEndElement();
                xmlDocument.WriteEndDocument();
            }
            return true;
        }

        /// <summary>
        ///     Reads the settings from file
        /// </summary>
        /// <returns></returns>
        private bool ReadSettings()
        {
            if (!File.Exists(Path.Combine(BasePath, SettingsFile)))
            {
                _urlList.Add("http://feeds.nos.nl/nosop3");
                _urlList.Add("http://feeds.feedburner.com/tweakers/mixed");
                _urlList.Add("http://www.windesheim.nl/over-windesheim/nieuws/rss/");

                if (!WriteSettings())
                    return false;
            }

            var xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(Path.Combine(BasePath, SettingsFile));
            }
            catch
            {
                return false;
            }

            var docBody = xmlDoc.DocumentElement;

            var nodeList = docBody != null ? docBody.SelectNodes("/feeds/feed") : null;

            if (nodeList == null)
                return false;

            _urlList.Clear();

            foreach (XmlNode node in nodeList)
            {
                if (!String.IsNullOrEmpty(node.InnerText))
                    _urlList.Add(node.InnerText);
            }

            return true;
        }

        /// <summary>
        ///     Gets the URLs from the news feeds
        /// </summary>
        /// <returns></returns>
        public List<string> GetSettingsList()
        {
            return !ReadSettings() ? null : _urlList;
        }

        /// <summary>
        ///     Sets the URLs of the news feeds
        /// </summary>
        /// <param name="settingList"></param>
        /// <returns></returns>
        public bool SetSettingsList(List<string> settingList)
        {
            _urlList = settingList;
            return WriteSettings();
        }
    }
}