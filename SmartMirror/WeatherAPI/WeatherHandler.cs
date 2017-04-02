using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace SmartMirror.WeatherAPI
{
	public class WeatherHandler : INotifyPropertyChanged
	{
		private WeatherData currentWeather;

		public event PropertyChangedEventHandler PropertyChanged;
		public string name { get { return "dkjfhs"; } set {; } }

		public WeatherData CurrentWeather
		{
			get
			{
				return currentWeather;
			}
			set
			{
				currentWeather = value;
				OnPropertyChanged("CurrentWeather");
			}
		}

		void OnPropertyChanged(string name)
		{
			if (this.PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(name));
			}

		}

		public WeatherHandler()
		{

		}

		public async Task<bool> GetWeather()
		{
			var position = await LocationManager.GetPosition();
			CurrentWeather = await OpenWeatherMapProxy.GetWeather(position.Coordinate.Point.Position.Latitude,	position.Coordinate.Point.Position.Longitude);
			return true;
		}
    }

	public class WeatherToImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			WeatherData d = (WeatherData)value;
			int iconId = d.weather[0].id;

			string source = "ms-appx:///WeatherIcon/NODATA.png";

			return source;

		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
