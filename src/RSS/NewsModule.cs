using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using News.Properties;
using Project_Novi.Api;

namespace News
{
    internal class NewsModule : IModule
    {
        public delegate void EntriesUpdatedEvent();

        private IController _controller;
        private RssHandler _rssHandler;
        private NewsSettings _settings;
        public bool EnableUpdate = true;
        public List<RssEntry> entries { get; private set; }

        public string Name
        {
            get { return "News"; }
        }

        public Bitmap Icon
        {
            get { return Resources.Icon; }
        }

        public string DisplayName
        {
            get { return "Nieuws"; }
        }

        public void Initialize(IController controller)
        {
            _controller = controller;

            if (entries == null)
                entries = new List<RssEntry>();

            if (_settings == null)
                _settings = new NewsSettings();

            if (_rssHandler == null)
            {
                _rssHandler = new RssHandler();
                _rssHandler.RssDownloadFinished += downloadFinished;
            }

            var thread = new Thread(UpdateThread);
            thread.IsBackground = true;
            thread.Start();

            var thread2 = new Thread(UpdateDataDelayed);
            thread2.IsBackground = true;
            thread2.Start();
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        public event EntriesUpdatedEvent EntriesUpdated;

        private void DoPostUpdate()
        {
            entries.Sort((e1, e2) => e2.Timestamp.CompareTo(e1.Timestamp));
            if (EntriesUpdated != null)
                EntriesUpdated();
        }

        private void UpdateData()
        {
            entries.Clear();

            foreach (var url in _settings.GetSettingsList())
            {
                if (CacheHandler.Contains(url))
                {
                    var data = CacheHandler.ReadFromCache(url);
                    foreach (var entry in data)
                        entries.Add(entry);
                }
                else
                {
                    _rssHandler.Download(url);
                }
            }

            DoPostUpdate();
        }

        private void UpdateDataDelayed()
        {
            Thread.Sleep(1000);
            UpdateData();
            Thread.CurrentThread.Abort();
        }

        private void UpdateThread()
        {
            while (true)
            {
                Thread.Sleep(60*1000);
                if(EnableUpdate)
                    UpdateData();
            }
        }

        private void downloadFinished(string url, string data)
        {
            var parsed = RssHandler.ParseRssEntries(data, url);

            if (parsed != null)
                CacheHandler.WriteToCache(url, parsed);

            foreach (var entry in parsed)
                entries.Add(entry);

            DoPostUpdate();
        }
    }
}