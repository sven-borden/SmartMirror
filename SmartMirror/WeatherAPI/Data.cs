using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror.WeatherAPI
{
	public class Data
	{
		public Coord coord { get; set; }

		public Weather weather { get; set; }

		public Main main { get; set; }

		public Wind wind { get; set; }

		/*
		public Rain rain { get; set; }
		*/

		public Clouds clouds { get; set; }

		public int dt { get; set; }

		public Sys sys { get; set; }

		public int id { get; set; }

		public string name { get; set; }

		public int cod { get; set; }

		public Data()
		{
			this.coord = new Coord() { lat = 0, lon = 0 };
			this.weather = new Weather() { description = string.Empty, icon = string.Empty, id = 0, main = string.Empty };
			this.main = new Main() { humidity = 0, pressure = 0, temp = 0, temp_max = 0, temp_min = 0 };
			this.wind = new Wind() { deg = 0, speed = 0 };
			this.clouds = new Clouds() { all = 0 };
			this.dt = 0;
			this.sys = new Sys() { country = string.Empty, id = 0, message = 0, sunrise = 0, sunset = 0, type = 1 };
			this.id = 0;
			this.name = string.Empty;
			this.cod = 0;
		}

		public Data(WeatherData _w)
		{
			this.coord = _w.coord;
			this.weather = _w.weather[0];
			this.main = _w.main;
			this.wind = _w.wind;
			this.clouds = _w.clouds;
			this.dt = _w.dt;
			this.sys = _w.sys;
			this.id = _w.id;
			this.name = _w.name;
			this.cod = _w.cod;
		}
	}
}
