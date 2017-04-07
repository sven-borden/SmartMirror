using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sonos.Client.Models
{
    [XmlRoot(ElementName = "GetPositionInfoResponse")]
    public class PositionInfoResponse
    {
        [XmlElement(ElementName = "Track")]
        public string Track { get; set; }
        [XmlElement(ElementName = "TrackDuration")]
        public string TrackDuration { get; set; }
        [XmlElement(ElementName = "TrackMetaData")]
        public string TrackMetaData { get; set; }
        [XmlElement(ElementName = "TrackURI")]
        public string TrackURI { get; set; }
        [XmlElement(ElementName = "RelTime")]
        public string RelTime { get; set; }
        [XmlElement(ElementName = "AbsTime")]
        public string AbsTime { get; set; }
        [XmlElement(ElementName = "RelCount")]
        public string RelCount { get; set; }
        [XmlElement(ElementName = "AbsCount")]
        public string AbsCount { get; set; }

        public TimeSpan ElapsedTime
        {
            get
            {
                return TimeSpan.Parse(RelTime);
            }
        }
    }

    [XmlRoot(ElementName = "Body")]
    public class Body
    {
        [XmlElement(ElementName = "GetPositionInfoResponse")]
        public PositionInfoResponse PositionInfoResponse { get; set; }
    }

    [XmlRoot(ElementName = "Envelope")]
    public class Envelope
    {
        [XmlElement(ElementName = "Body")]
        public Body Body { get; set; }
    }
}
