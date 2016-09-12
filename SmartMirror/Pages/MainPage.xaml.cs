using SmartMirror.Audio;
using SmartMirror.Class.News;
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
using Windows.Globalization.DateTimeFormatting;
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
		ObservableCollection<Weather> TodayWeather;
		ObservableCollection<Weather> TomorrowWeather;
		ObservableCollection<Calendar> CalendarList;
		ObservableCollection<NewsData> NewsList;

		public MainPage()
        {
            this.InitializeComponent();
			Time();
			SetNews();
			DispatcherTimer internetTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0) };
			internetTimer.Tick += (tick, args) =>
			{
				internetTimer.Interval = new TimeSpan(0, 0, 30);
				if (InternetAccess.Internet.isConnected())
				{
					internetTimer.Stop();
					MainBox.Text = "Hi, There !";
					Initialisation();
				}
				else
					MainBox.Text = "No internet access !";
			};
			internetTimer.Start();
        }

		private async void SetNews()
		{
			if (NewsList == null)
				NewsList = new ObservableCollection<NewsData>();
			List<NewsData> news = new List<NewsData>();
			news = await News.GetNews();
			if (news == null)
			return;
			foreach(var item in news)
				NewsList.Add(item);
		}

		private void Time()
		{
			DispatcherTimer timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0) };
			timer.Tick += (tick, args) =>
			{
				timer.Interval = new TimeSpan(0, 0, 0, 1);
				//get time and update it
				DateTime date = DateTime.Now;
				var time = new DateTimeFormatter("shorttime");
				string t = date.Hour.ToString() + ":";
				t += date.Minute + ":";
				t += date.Second;

				TimeBox.Text = time.Format(date);
			};
			timer.Start();
		}

		private void Initialisation()
		{
			GetWeather();
			
			SpeechRecognition speech = new SpeechRecognition();
			DispatcherTimer speechTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };
			speechTimer.Tick += (tick, args) =>
			{
				if (speech.isListening && speech.newWord)
					MainBox.Text = speech.lastPhrase;
			};
			speechTimer.Start();
		}

		private async void GetWeather()
		{
			if (TodayWeather == null)
				TodayWeather = new ObservableCollection<Weather>();
			TodayWeather.Add(await Weather.GetCurrentWeather("St-Prex", "Switzerland", "fr", "metric"));
			this.Bindings.Update();
		}
	}
}
