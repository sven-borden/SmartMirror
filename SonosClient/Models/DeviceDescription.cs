using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sonos.Client.Models
{

    [XmlRoot(ElementName = "root", Namespace = "urn:schemas-upnp-org:device-1-0")]
    public class DeviceDescription
    {
        [XmlElement(ElementName = "specVersion", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public SpecVersion SpecVersion { get; set; }
        [XmlElement(ElementName = "device", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public Device Device { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }

    [XmlRoot(ElementName = "specVersion", Namespace = "urn:schemas-upnp-org:device-1-0")]
    public class SpecVersion
    {
        [XmlElement(ElementName = "major", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public string Major { get; set; }
        [XmlElement(ElementName = "minor", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public string Minor { get; set; }
    }

    [XmlRoot(ElementName = "icon", Namespace = "urn:schemas-upnp-org:device-1-0")]
    public class Icon
    {
        [XmlElement(ElementName = "id", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public string Id { get; set; }
        [XmlElement(ElementName = "mimetype", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public string Mimetype { get; set; }
        [XmlElement(ElementName = "width", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public string Width { get; set; }
        [XmlElement(ElementName = "height", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public string Height { get; set; }
        [XmlElement(ElementName = "depth", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public string Depth { get; set; }
        [XmlElement(ElementName = "url", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public string Url { get; set; }
    }

    [XmlRoot(ElementName = "iconList", Namespace = "urn:schemas-upnp-org:device-1-0")]
    public class IconList
    {
        [XmlElement(ElementName = "icon", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public Icon Icon { get; set; }
    }

    [XmlRoot(ElementName = "service", Namespace = "urn:schemas-upnp-org:device-1-0")]
    public class Service
    {
        [XmlElement(ElementName = "serviceType", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public string ServiceType { get; set; }
        [XmlElement(ElementName = "serviceId", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public string ServiceId { get; set; }
        [XmlElement(ElementName = "controlURL", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public string ControlURL { get; set; }
        [XmlElement(ElementName = "eventSubURL", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public string EventSubURL { get; set; }
        [XmlElement(ElementName = "SCPDURL", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public string SCPDURL { get; set; }
    }

    [XmlRoot(ElementName = "serviceList", Namespace = "urn:schemas-upnp-org:device-1-0")]
    public class ServiceList
    {
        [XmlElement(ElementName = "service", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public List<Service> Service { get; set; }
    }

    [XmlRoot(ElementName = "device", Namespace = "urn:schemas-upnp-org:device-1-0")]
    public class Device
    {
        [XmlElement(ElementName = "deviceType", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public string DeviceType { get; set; }
        [XmlElement(ElementName = "friendlyName", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public string FriendlyName { get; set; }
        [XmlElement(ElementName = "manufacturer", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public string Manufacturer { get; set; }
        [XmlElement(ElementName = "manufacturerURL", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public string ManufacturerURL { get; set; }
        [XmlElement(ElementName = "modelNumber", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public string ModelNumber { get; set; }
        [XmlElement(ElementName = "modelDescription", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public string ModelDescription { get; set; }
        [XmlElement(ElementName = "modelName", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public string ModelName { get; set; }
        [XmlElement(ElementName = "modelURL", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public string ModelURL { get; set; }
        [XmlElement(ElementName = "softwareVersion", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public string SoftwareVersion { get; set; }
        [XmlElement(ElementName = "hardwareVersion", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public string HardwareVersion { get; set; }
        [XmlElement(ElementName = "serialNum", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public string SerialNumber { get; set; }
        [XmlElement(ElementName = "UDN", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public string UDN { get; set; }
        [XmlElement(ElementName = "roomName", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public string RoomName { get; set; }



        [XmlElement(ElementName = "serviceList", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public ServiceList ServiceList { get; set; }
        [XmlElement(ElementName = "X_Rhapsody-Extension", Namespace = "http://www.real.com/rhapsody/xmlns/upnp-1-0")]
        public X_RhapsodyExtension X_RhapsodyExtension { get; set; }
        [XmlElement(ElementName = "X_QPlay_SoftwareCapability", Namespace = "http://www.tencent.com")]
        public X_QPlay_SoftwareCapability X_QPlay_SoftwareCapability { get; set; }
        [XmlElement(ElementName = "iconList", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public IconList IconList { get; set; }
    }

    [XmlRoot(ElementName = "interactionPattern", Namespace = "http://www.real.com/rhapsody/xmlns/upnp-1-0")]
    public class InteractionPattern
    {
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
    }

    [XmlRoot(ElementName = "deviceCapabilities", Namespace = "http://www.real.com/rhapsody/xmlns/upnp-1-0")]
    public class DeviceCapabilities
    {
        [XmlElement(ElementName = "interactionPattern", Namespace = "http://www.real.com/rhapsody/xmlns/upnp-1-0")]
        public InteractionPattern InteractionPattern { get; set; }
    }

    [XmlRoot(ElementName = "X_Rhapsody-Extension", Namespace = "http://www.real.com/rhapsody/xmlns/upnp-1-0")]
    public class X_RhapsodyExtension
    {
        [XmlElement(ElementName = "deviceID", Namespace = "http://www.real.com/rhapsody/xmlns/upnp-1-0")]
        public string DeviceID { get; set; }
        [XmlElement(ElementName = "deviceCapabilities", Namespace = "http://www.real.com/rhapsody/xmlns/upnp-1-0")]
        public DeviceCapabilities DeviceCapabilities { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }

    [XmlRoot(ElementName = "X_QPlay_SoftwareCapability", Namespace = "http://www.tencent.com")]
    public class X_QPlay_SoftwareCapability
    {
        [XmlAttribute(AttributeName = "qq", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Qq { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "deviceList", Namespace = "urn:schemas-upnp-org:device-1-0")]
    public class DeviceList
    {
        [XmlElement(ElementName = "device", Namespace = "urn:schemas-upnp-org:device-1-0")]
        public List<Device> Device { get; set; }
    }
    
}
