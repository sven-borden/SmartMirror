using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		public async Task<Station> GetStation(string city)
		{
			HttpClient http = new HttpClient();
			string response = await http.GetStringAsync(@"http://transport.opendata.ch/v1/locations?query=" + city);
			Location tmp = JsonConvert.DeserializeObject<Location>(response);
			if (tmp == null)
				return null;
			Station station = tmp.Stations[0];
			return station;
		}

		private async void GetBase()
		{
			var http = new HttpClient();
			var response = await http.GetStringAsync(@"http://transport.opendata.ch/v1/connections?from=Lausanne&to=Genève");
			//var result = await response.Content.ReadAsStringAsync();
			Debug.WriteLine(response);
			var data = JsonConvert.DeserializeObject<Connections>(response);
			string a = "sdfg";
		}
	}
}
