using Newtonsoft.Json;
using SmartMirror.Content;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace SmartMirror.CFF
{
	public class Handler
	{
		private bool DEBUG = false;
		private StationBoard currentConnectionsLausanne = new StationBoard();
		public StationBoard CurrentConnectionsLausanne { get { return currentConnectionsLausanne; } set { currentConnectionsLausanne = value; } }
		private StationBoard currentConnectionsGenf = new StationBoard();
		public StationBoard CurrentConnectionsGenf { get { return currentConnectionsGenf; } set { currentConnectionsGenf = value; } }
		private Message Message;

		public Handler(Message _message)
		{
			Message = _message;
			CurrentConnectionsLausanne.StationBoards = new ObservableCollection<StationBoards>();
			CurrentConnectionsGenf.StationBoards = new ObservableCollection<StationBoards>();
			DispatcherTimer t = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 30) };
			t.Tick += async (o, e) =>
			{
				await GetStationBoard("St-Prex");
			};
			t.Start();
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

		public async Task<StationBoard> GetStationBoard(string city)
		{
			HttpClient http = new HttpClient();
			string response = await http.GetStringAsync($@"http://transport.opendata.ch/v1/stationboard?station={city}&limit=6");
			StationBoard tmp = JsonConvert.DeserializeObject<StationBoard>(response);

			foreach (var s in tmp.StationBoards)
			{
				if (s.Name.Contains("NFB"))
					continue;
				if (s.To != "Allaman")
					CurrentConnectionsLausanne.StationBoards.Add(s);
				else
					CurrentConnectionsGenf.StationBoards.Add(s);
			}
			while (CurrentConnectionsGenf.StationBoards.Count > 2)
				CurrentConnectionsGenf.StationBoards.RemoveAt(CurrentConnectionsGenf.StationBoards.Count - 1);
			CurrentConnectionsGenf.StationBoards.Reverse();
			while (CurrentConnectionsLausanne.StationBoards.Count > 2)
				CurrentConnectionsLausanne.StationBoards.RemoveAt(CurrentConnectionsLausanne.StationBoards.Count - 1);
			CurrentConnectionsLausanne.StationBoards.Reverse();

			return tmp;
		}

		public async Task<Connections> GetConnection(string from, string to)
		{
			var http = new HttpClient();
			var response = await http.GetStringAsync($"http://transport.opendata.ch/v1/connections?from={from}&to={to}");
			Connections data = JsonConvert.DeserializeObject<Connections>(response);
			return data;
		}

		private async void GetBase()
		{
			
		}
	}

	public class TimeLeftCFF : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			string time = (string)value;
			if (time == string.Empty || time == null)
				return "NA";
			DateTime t = TimeConverter(time);
			return t.Subtract(DateTime.Now).Minutes.ToString() + "min";
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}

		private DateTime TimeConverter(string ISO)
		{
			DateTime d = DateTime.Parse(ISO, null, DateTimeStyles.RoundtripKind);
			return d;
		}
	}
}
