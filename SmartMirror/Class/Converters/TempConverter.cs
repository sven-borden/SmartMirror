using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace SmartMirror.Class.Converters
{
	public class TempConverter : IValueConverter
	{
		public TempConverter()
		{

		}

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			double temp = (double)value;
			return temp.ToString() + "°";
		}

		public object ConvertBack(object value, Type targetType,
		object parameter, string language)
		{
			string t = (string)value;
			if (t.Contains("°"))
				return t.Split('°')[0];
			else
				return 0.00;
		}

	}
}
