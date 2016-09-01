using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror
{
	class Weather
	{
		public string Day { get; set; }
		public double Temp { get; set; }
		public string Icon { get; set; }

		public Weather(string day, double temp, string icon)
		{
			Day = day;
			Temp = temp;
			Icon = icon;
		}
	}
}
