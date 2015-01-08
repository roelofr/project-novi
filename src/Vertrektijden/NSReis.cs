using System;
using System.Collections.Generic;
using System.Xml;

namespace Vertrektijden
{
    public class NSReis
    {
        public NSReis(String destination, TrainTrack trackInfo, DateTime departureTime)
        {
            Destination = destination;
            DepartureTrack = trackInfo;
            DepartureTime = departureTime;
        }

        /// <summary>
        ///     The date and time this train departs
        /// </summary>
        public DateTime DepartureTime { get; private set; }

        /// <summary>
        ///     The destination of the train, not null
        /// </summary>
        public String Destination { get; private set; }

        /// <summary>
        ///     The track from which this train departs
        /// </summary>
        public TrainTrack DepartureTrack { get; private set; }

        /// <summary>
        ///     The type of train, such as "Sprinter" or "Intercity"
        /// </summary>
        public String TrainType { get; set; }

        /// <summary>
        ///     The train provider, such as "NS" or "Arriva"
        /// </summary>
        public String Provider { get; set; }

        /// <summary>
        ///     Shows extra trip details, such as "Amersfoort, Utrecht Centraal", can be NULL
        /// </summary>
        public String TripText { get; set; }

        /// <summary>
        ///     Contains the delay in a "+5 min" format, can be NULL
        /// </summary>
        public String DelayText { get; set; }

        /// <summary>
        ///     Contains the delay in a Timespan format, can be NULL
        /// </summary>
        public TimeSpan DelayTime { get; set; }

        /// <summary>
        ///     Contains extra information such as "Stopt op alle tussengelegen stations", can be NULL
        /// </summary>
        public String TravelAdvice { get; set; }

        /// <summary>
        ///     Contains extra information such as "Voor deze trein is een toeslag vereist", can be NULL
        /// </summary>
        public String[] Comments { get; set; }

        public static NSReis XmlToReis(XmlNode node)
        {
            if (node == null)
                return null;

            var depNode = node.SelectSingleNode("VertrekTijd");
            var destNode = node.SelectSingleNode("EindBestemming");
            var typeNode = node.SelectSingleNode("TreinSoort");
            var tripNode = node.SelectSingleNode("RouteTekst");
            var trackNode = node.SelectSingleNode("VertrekSpoor");
            var provNode = node.SelectSingleNode("Vervoerder");
            var delayNode = node.SelectSingleNode("VertrekVertragingTekst");
            var delayTimeNode = node.SelectSingleNode("VertrekVertraging");
            var travelTip = node.SelectSingleNode("ReisTip");
            var commentNodes = node.SelectNodes("Opmerkingen/Opmerking");

            if (depNode == null || destNode == null || trackNode == null)
                return null;

            var depTime = ParseDepartureTime(depNode.InnerText);
            var trackInfo = TrainTrack.XmlToTrack(trackNode);
            var dest = destNode.InnerText;

            if (depTime.Year == 1970 || String.IsNullOrWhiteSpace(dest) || trackInfo == null)
                return null;

            var trip = new NSReis(dest, trackInfo, depTime)
            {
                TrainType = typeNode != null ? typeNode.InnerText : "Intercity",
                Provider = provNode != null ? provNode.InnerText : "NS",
                TripText = tripNode != null ? tripNode.InnerText : null,
                DelayText = delayNode != null ? delayNode.InnerText : null,
                DelayTime = delayTimeNode != null ? XmlConvert.ToTimeSpan(delayTimeNode.InnerText) : new TimeSpan(0L),
                TravelAdvice = travelTip != null ? travelTip.InnerText : null
            };

            if (commentNodes != null)
            {
                var tripComments = new List<String>();
                foreach (XmlNode comment in commentNodes)
                {
                    if (comment != null)
                        tripComments.Add(comment.InnerText);
                }
                trip.Comments = tripComments.ToArray();
            }
            else
            {
                trip.Comments = null;
            }

            return trip;
        }

        /// <summary>
        ///     Parses a date, catching any errors that might occur
        /// </summary>
        /// <param name="timeString"></param>
        /// <returns></returns>
        private static DateTime ParseDepartureTime(String timeString)
        {
            try
            {
                var outTime = DateTime.Parse(timeString);
                return outTime;
            }
            catch
            {
                return new DateTime(1970, 1, 1);
            }
        }
    }
}