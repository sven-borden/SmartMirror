using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace SmartMirror.WeatherAPI
{
	public class WeatherHandler : INotifyPropertyChanged
	{
		private Data currentWeather = new Data();

		public event PropertyChangedEventHandler PropertyChanged;

		public Data CurrentWeather
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

	public class TempToTextConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			double t = (double)value;

			return t + "°";
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}

	public class PressureToTextConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			int p = (int)value;

			return p + " hPa";
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}

	public class HumidityToTextConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			int h = (int)value;

			return h + " %";
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}

	public class WeatherToImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			int v = (int)value;
			Debug.WriteLine("Weather id : " + v);
			string source = "ms-appx:/WeatherIcon/NODATA.png";
			if (v < 200)
			{
				source = "ms-appx:/WeatherIcon/NODATA.png";
			}
			else
			{
				if (v < 500)
				{
					if (v < 210)
					{
						source = "ms-appx:/WeatherIcon/200.png";
					}
					else
					{
						if (v == 211)
							source = "ms-appx:/WeatherIcon/211.png";
						else
							source = "ms-appx:/WeatherIcon/212.png";
					}
				}
				else // >=500
				{
					if (v == 500)
						source = "ms-appx:/WeatherIcon/500.png";
					else
					{
						if (v < 600)
							source = "ms-appx:/WeatherIcon/501.png";
						else // >=600
						{
							if (v == 600)
								source = "ms-appx:/WeatherIcon/600.png";
							else
							{
								if(v < 700)
								{
									if (v == 601)
										source = "ms-appx:/WeatherIcon/601.png";
									else
									{
										if (v < 615)
											source = "ms-appx:/WeatherIcon/615.png";
										else
											source = "ms-appx:/WeatherIcon/616.png";
									}
								}
								else // >=700
								{
									if(v < 800)
									{
										if (v == 701)
											source = "ms-appx:/WeatherIcon/701.png";
										else
											source = "ms-appx:/WeatherIcon/711.png";
									}
									else //>= 800
									{
										if(v < 900)
										{
											if (v == 800)
												source = "ms-appx:/WeatherIcon/800D.png";
											if (v == 801)
												source = "ms-appx:/WeatherIcon/801D.png";
											if (v == 802)
												source = "ms-appx:/WeatherIcon/802D.png";
											if (v == 803)
												source = "ms-appx:/WeatherIcon/803D.png";
											if (v == 804)
												source = "ms-appx:/WeatherIcon/804D.png";
										}
										else
										{
											switch(v)
											{
												case 900: source = "ms-appx:/WeatherIcon/900.png"; break;
												case 901:
												case 902: source = "ms-appx:/WeatherIcon/902.png"; break;
												case 903: source = "ms-appx:/WeatherIcon/903.png"; break;
												case 904: source = "ms-appx:/WeatherIcon/904.png"; break;
												default: source = "ms-appx:/WeatherIcon/905.png"; break;
											}
										}
									}
								}
							}
						}
					}
						
				}
			}
			 
			BitmapImage imgSource = new BitmapImage(new Uri(source));
			Image i = new Image();
			i.Source = imgSource;
			//return source;
			return imgSource;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
