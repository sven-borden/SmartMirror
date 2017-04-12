using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror.CFF
{
	public class Handler
	{
		public Handler()
		{
			GetBase();
		}

		private async void GetBase()
		{
			var http = new HttpClient();
			var response = await http.GetAsync(@"http://transport.opendata.ch/v1/locations?query=Basel");
			var result = await response.Content.ReadAsStringAsync();
			Location data = JsonConvert.DeserializeObject<Location>(result);
			string a = "sdfg";
		}
	}
}
