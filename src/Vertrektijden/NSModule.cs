using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using Project_Novi.Api;
using Vertrektijden.Properties;

namespace Vertrektijden
{
    public class NSModule : IModule
    {
        public delegate void DataUpdatedEvent(List<NSReis> travelInformation);

        public const string StationName = "Zwolle";

        private ApiComm _comm;
        public readonly List<NSReis> TripList = new List<NSReis>();
        public bool ApiMalConfigured { get; private set; }
        public DateTime LastDownload { get; private set; }

        public string Name
        {
            get { return "NS"; }
        }

        public Bitmap Icon
        {
            get { return Resources.train_icon; }
        }

        public string DisplayName
        {
            get { return "Actuele vertrektijd"; }
        }

        public bool Rotatable
        {
            get { return true; }
        }

        public DateTime DataLastModified
        {
            get { return _comm != null ? _comm.LastModifiedTime : new DateTime(1970, 1, 1); }
        }

        public void Initialize(IController controller)
        {
            ApiMalConfigured = false;
            _comm = null;
            LastDownload = new DateTime(1970, 1, 1);
        }

        public void Start()
        {
            // Not doing anything on start
        }

        public void Stop()
        {
            // Not doing anything on stop
        }

        private void NsDataAvailable(XmlDocument document, string station)
        {
            if (document == null)
                return;

            var body = document.DocumentElement;
            if (body == null)
                return;

            var nodes = body.SelectNodes("/ActueleVertrekTijden/VertrekkendeTrein");

            if (nodes == null)
                return;

            TripList.Clear();
            foreach (XmlNode node in nodes)
            {
                if (node == null)
                    continue;

                var reisNode = NSReis.XmlToReis(node);
                if (reisNode != null)
                    TripList.Add(reisNode);
            }

            TripList.Sort((e1, e2) => e1.DepartureTime.CompareTo(e2.DepartureTime));

            if (DataUpdated != null)
                DataUpdated(TripList);
        }

        private void CreateComm()
        {
            if (_comm != null || ApiMalConfigured)
                return;

            try
            {
                _comm = new ApiComm(StationName);
                _comm.NsDataAvailable += NsDataAvailable;
                ApiMalConfigured = false;
            }
            catch (PlatformNotSupportedException e)
            {
                _comm = null;
                ApiMalConfigured = true;
            }
            catch
            {
                // Ignore further errors
            }
        }

        public void UpdateData()
        {
            CreateComm();

            if (ApiMalConfigured)
                return;

            LastDownload = DateTime.Now;

            _comm.GetInformation();
        }

        public event DataUpdatedEvent DataUpdated;
    }
}