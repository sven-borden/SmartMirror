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
		/// <summary>
		/// Day concerning the Weather
		/// </summary>
		public DateTime Day { get; set; }

		/// <summary>
		/// Actual Temperature
		/// </summary>
		public double Temp { get; set; }

		/// <summary>
		/// Minimum Temperature of the Day
		/// </summary>
		public double TempMin { get; set; }

		/// <summary>
		/// Maximum Temperature of the Day
		/// </summary>
		public double TempMax { get; set; }

		/// <summary>
		/// Actual Humidity
		/// </summary>
		public double Humidity { get; set; }

		/// <summary>
		/// Description of the Actual weather
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Actual Windspeed
		/// </summary>
		public double WindSpeed { get; set; }

		/// <summary>
		/// Icon
		/// </summary>
		public string Icon { get; set; }


		/// <summary>
		/// General constructor
		/// </summary>
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
			if (result.Item.TempMin.ToString().Contains("."))
				result.Item.TempMin = Convert.ToDouble(result.Item.TempMin.ToString().Split('.')[0]);
			if (result.Item.TempMax.ToString().Contains("."))
				result.Item.TempMax = Convert.ToDouble(result.Item.TempMax.ToString().Split('.')[0]);
			if (result.Item.Temp.ToString().Contains("."))
				result.Item.Temp = Convert.ToDouble(result.Item.Temp.ToString().Split('.')[0]);
			Weather t = new Weather()
			{
				Day = result.Item.Date,
				TempMin = result.Item.TempMin,
				TempMax = result.Item.TempMax,
				Temp = result.Item.Temp,
				Description = result.Item.Description,
				Humidity = result.Item.Humidity,
				WindSpeed = result.Item.WindSpeed,
				Icon = "ms-appx:///WeatherAssets/sun.png"
			};
			return t;
		}

		/// <summary>
		/// Get five day forecast
		/// </summary>
		/// <param name="city">St-Prex</param>
		/// <param name="country">Switzerland</param>
		/// <param name="language">"fr"</param>
		/// <param name="type">metric</param>
		/// <returns></returns>
		public async static Task<List<Weather>> GetFiveDayForecast(string city, string country, string language, string type)
		{
			ClientSettings.ApiUrl = "http://api.openweathermap.org/data/2.5";
			ClientSettings.ApiKey = "c0fef2f73a39d4f8bf97ebf8dfc02e88";

			var result = await FiveDaysForecast.GetByCityNameAsync(city, country, language, type);
			if (!result.Success)
				return null;

			List<Weather> t = new List<Weather>();
			foreach (var item in result.Items)
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
