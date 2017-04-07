using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonos.Client.Models.Spotify
{
    public class Track
    {
        public string provider_url { get; set; }
        public string version { get; set; }
        public int thumbnail_width { get; set; }
        public int height { get; set; }
        public int thumbnail_height { get; set; }
        public string title { get; set; }
        public int width { get; set; }
        public string thumbnail_url { get; set; }
        public string provider_name { get; set; }
        public string type { get; set; }
        public string html { get; set; }
        public string large_url { get
            {
                return thumbnail_url.Replace("/cover", "/640");
            }
        }
    }
}
