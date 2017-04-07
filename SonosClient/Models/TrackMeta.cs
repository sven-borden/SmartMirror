using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sonos.Client.Models
{

    [XmlRoot(ElementName = "res")]
    public class Res
    {
        [XmlAttribute(AttributeName = "protocolInfo")]
        public string ProtocolInfo { get; set; }
        [XmlAttribute(AttributeName = "duration")]
        public string Duration { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "item")]
    public class Item
    {
        [XmlElement(ElementName = "res")]
        public Res Res { get; set; }
        [XmlElement(ElementName = "streamContent")]
        public string StreamContent { get; set; }
        [XmlElement(ElementName = "radioShowMd")]
        public string RadioShowMd { get; set; }
        [XmlElement(ElementName = "albumArtURI")]
        public string AlbumArtURI { get; set; }
        public string AlbumArtURIClean
        {
            get
            {
                return WebUtility.UrlDecode(AlbumArtURI);
            }
        }
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }
        [XmlElement(ElementName = "class")]
        public string Class { get; set; }
        [XmlElement(ElementName = "creator")]
        public string Creator { get; set; }
        [XmlElement(ElementName = "album")]
        public string Album { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "parentID")]
        public string ParentID { get; set; }
        [XmlAttribute(AttributeName = "restricted")]
        public string Restricted { get; set; }
    }

    [XmlRoot(ElementName = "DIDL-Lite")]
    public class TrackMeta
    {
        [XmlElement(ElementName = "item")]
        public Item Item { get; set; }
    }

}
