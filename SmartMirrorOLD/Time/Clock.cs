using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace SmartMirror.Time
{
	public class Clock : INotifyPropertyChanged
	{
		private DateTime localTime;
		public DateTime LocalTime
		{
			get
			{
				return localTime;
			}
			set
			{
				localTime = value;
				OnPropertyChanged("LocalTime");
			}
		}

		private void OnPropertyChanged(string propertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;


		public Clock()
		{
			localTime = DateTime.Now;
		}
	}

	public class DateToTimeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			DateTime thisdate = (DateTime)value;

			string tmp;
			
			return tmp;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
