using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sonos.Client
{
    public class SonosUtils
    {

        public static string CleanSonosResponse(string content)
        {
            content = content.Replace("s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\"", "");
            content = content.Replace("upnp:", "");
            content = content.Replace("/upnp:", "/");
            content = content.Replace("<dc:", "<");
            content = content.Replace("</dc:", "</");
            content = content.Replace("<r:", "<");
            content = content.Replace("</r:", "</");
            content = content.Replace("<u:", "<");
            content = content.Replace("</u:", "</");
            content = content.Replace("<s:", "<");
            content = content.Replace("</s:", "</");
            content = content.Replace("  ", "");
            content = content.Replace("\t", "");
            content = content.Replace("xmlns:dc=&quot;http://purl.org/dc/elements/1.1/&quot; xmlns:upnp=&quot;urn:schemas-upnp-org:metadata-1-0/upnp/&quot; xmlns:r=&quot;urn:schemas-rinconnetworks-com:metadata-1-0/&quot; xmlns=&quot;urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/&quot;", "");

            Regex namespaceRegex = new Regex("xmlns:*(.*?)=(\".*?\")");
            content = namespaceRegex.Replace(content, "");

            return content;
        }

        public static string CleanSonosNotification(string content)
        {
            content = content.Replace("<e:propertyset xmlns:e=\"urn:schemas-upnp-org:event-1-0\"><e:property><LastChange>", "");
            content = content.Replace("</LastChange></e:property></e:propertyset>", "");
            content = content.Replace("&lt;", "<");
            content = content.Replace("&gt;", ">");
            content = content.Replace("&quot;", "\"");
            content = content.Replace("&amp;", "&");

            content = CleanSonosResponse(content);

            if (!content.Contains("\"/></InstanceID></Event>"))
                content += "\"/></InstanceID></Event>";

            return content;
        }
    }
}
