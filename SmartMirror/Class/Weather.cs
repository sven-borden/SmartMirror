using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherNet;
using WeatherNet.Clients;
using WeatherNet.Model;

namespace SmartMirror
{
	class Weather
	{
		public DateTime Day { get; set; }
		public double Temp { get; set; }
		public double TempMin { get; set; }
		public double TempMax { get; set; }
		public double Humidity { get; set; }
		public string Description { get; set; }
		public double WindSpeed { get; set; }
		public string Icon { get; set; }

		

		public Weather()
		{

		}

		/// <summary>
		/// Return the current weather for parameter given
		/// </summary>
		/// <param name="city">"St-Prex</param>
		/// <param name="country">Switzerland</param>
		/// <param name="language">"fr"</param>
		/// <param name="type">"metric"</param>
		/// <returns>Weather object</returns>
		public async static Task<Weather> GetCurrentWeather(string city, string country, string language, string type)
		{
			ClientSettings.ApiUrl = "http://api.openweathermap.org/data/2.5";
			ClientSettings.ApiKey = "c0fef2f73a39d4f8bf97ebf8dfc02e88"; 

			var result = await CurrentWeather.GetByCityNameAsync(city, country, language, type);
			if (!result.Success)
				return null;
			Weather t = new Weather()
			{
				Day = result.Item.Date,
				TempMin = result.Item.TempMin,
				TempMax = result.Item.TempMax,
				Temp = result.Item.Temp,
				Description = result.Item.Description,
				Humidity = result.Item.Humidity,
				WindSpeed = result.Item.WindSpeed,
				Icon = result.Item.Icon
			};
			return t;
		}

		public async static Task<List<Weather>> GetFiveDayForecast(string city, string country, string language, string type)
		{
			ClientSettings.ApiUrl = "http://api.openweathermap.org/data/2.5";
			ClientSettings.ApiKey = "c0fef2f73a39d4f8bf97ebf8dfc02e88";

			var result = await FiveDaysForecast.GetByCityNameAsync(city, country, language, type);
			if (!result.Success)
				return null;

			List<Weather> t = new List<Weather>();
			foreach(var item in result.Items)
			{
				t.Add(new Weather()
				{
					Day = item.Date,
					TempMin = item.TempMin,
					TempMax = item.TempMax,
					Temp = item.Temp,
					Description = item.Description,
					Humidity = item.Humidity,
					WindSpeed = item.WindSpeed,
					Icon = item.Icon
				});
			}
			return t;
		}
	}
}
