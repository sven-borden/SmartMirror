using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization;
using Windows.Media.SpeechRecognition;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SmartMirror
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
		ObservableCollection<Weather> WeatherList;
		ObservableCollection<Calendar> CalendarList;
		ObservableCollection<News> NewsList;

		public MainPage()
        {
            this.InitializeComponent();
			Initialisation();
        }

		private void Initialisation()
		{
			GetWeather();
			DispatcherTimer time = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 20) };
			time.Tick += (tick, args) =>
			{
				//get time and update it
			};
			time.Start();
		}

		private void GetWeather()
		{
			
		}
	}
}
