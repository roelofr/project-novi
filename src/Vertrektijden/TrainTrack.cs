using System;
using System.Xml;

namespace Vertrektijden
{
    public class TrainTrack
    {
        public TrainTrack(String trackName, Boolean modified)
        {
            TrackName = trackName;
            Modified = modified;
        }

        public String TrackName { get; private set; }
        public Boolean Modified { get; private set; }

        public static TrainTrack XmlToTrack(XmlNode node)
        {
            if (node == null)
                return null;

            var trackName = node.InnerText;
            var trackAttr = node.Attributes;
            var trackChanged = false;
            if (trackAttr != null && trackAttr["wijziging"] != null)
                trackChanged = trackAttr["wijziging"].InnerText.Equals("true");

            return new TrainTrack(trackName, trackChanged);
        }
    }
}