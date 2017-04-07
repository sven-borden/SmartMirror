using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sonos.Client.Models
{
    [XmlRoot(ElementName = "ZoneGroupMember")]
    public class ZoneGroupMember
    {
        [XmlAttribute(AttributeName = "UUID")]
        public string UUID { get; set; }
        [XmlAttribute(AttributeName = "Location")]
        public string Location { get; set; }
        [XmlAttribute(AttributeName = "ZoneName")]
        public string ZoneName { get; set; }
        [XmlAttribute(AttributeName = "Icon")]
        public string Icon { get; set; }
        [XmlAttribute(AttributeName = "Configuration")]
        public string Configuration { get; set; }
        [XmlAttribute(AttributeName = "SoftwareVersion")]
        public string SoftwareVersion { get; set; }
        [XmlAttribute(AttributeName = "MinCompatibleVersion")]
        public string MinCompatibleVersion { get; set; }
        [XmlAttribute(AttributeName = "LegacyCompatibleVersion")]
        public string LegacyCompatibleVersion { get; set; }
        [XmlAttribute(AttributeName = "BootSeq")]
        public string BootSeq { get; set; }
        [XmlAttribute(AttributeName = "WirelessMode")]
        public string WirelessMode { get; set; }
        [XmlAttribute(AttributeName = "ChannelFreq")]
        public string ChannelFreq { get; set; }
        [XmlAttribute(AttributeName = "BehindWifiExtender")]
        public string BehindWifiExtender { get; set; }
        [XmlAttribute(AttributeName = "WifiEnabled")]
        public string WifiEnabled { get; set; }
    }

    [XmlRoot(ElementName = "ZoneGroup")]
    public class ZoneGroup
    {
        [XmlElement(ElementName = "ZoneGroupMember")]
        public List<ZoneGroupMember> ZoneGroupMember { get; set; }
        [XmlAttribute(AttributeName = "Coordinator")]
        public string Coordinator { get; set; }
        [XmlAttribute(AttributeName = "ID")]
        public string ID { get; set; }
    }

    [XmlRoot(ElementName = "ZoneGroups")]
    public class ZoneGroups
    {
        [XmlElement(ElementName = "ZoneGroup")]
        public ZoneGroup ZoneGroup { get; set; }
    }

    [XmlRoot(ElementName = "ZoneGroupState")]
    public class ZoneGroupState
    {
        [XmlElement(ElementName = "ZoneGroups")]
        public ZoneGroups ZoneGroups { get; set; }
    }

    [XmlRoot(ElementName = "UpdateItem", Namespace = "urn:schemas-rinconnetworks-com:update-1-0")]
    public class UpdateItem
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "Version")]
        public string Version { get; set; }
        [XmlAttribute(AttributeName = "UpdateURL")]
        public string UpdateURL { get; set; }
        [XmlAttribute(AttributeName = "DownloadSize")]
        public string DownloadSize { get; set; }
        [XmlAttribute(AttributeName = "ManifestURL")]
        public string ManifestURL { get; set; }
    }

    [XmlRoot(ElementName = "AvailableSoftwareUpdate")]
    public class AvailableSoftwareUpdate
    {
        [XmlElement(ElementName = "UpdateItem", Namespace = "urn:schemas-rinconnetworks-com:update-1-0")]
        public UpdateItem UpdateItem { get; set; }
    }

    [XmlRoot(ElementName = "propertyset", Namespace = "urn:schemas-upnp-org:event-1-0")]
    public class Propertyset
    {
        [XmlElement(ElementName = "property", Namespace = "urn:schemas-upnp-org:event-1-0")]
        public List<Property> Property { get; set; }
        [XmlAttribute(AttributeName = "e", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string E { get; set; }
    }
}
