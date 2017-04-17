using SmartMirror.CFF;
using SmartMirror.Hue;
using SmartMirror.Sonos;
using SmartMirror.Voice;
using SmartMirror.WeatherAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SmartMirror.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		public WeatherHandler weather = new WeatherHandler();
		Music Sonos = null;
		VoiceHandler Voice = null;
		HueHandler Hue = null;
		Handler CFF = null;

		public MainPage()
		{
			SetupWeather();
			this.InitializeComponent();
			this.DataContext = this;
			SetupClock();
			SetupSonos();
			SetupHue();
			SetupCff();
			SetupVoice();
		}

		private async void SetupCff()
		{
			CFF = new Handler();
			await CFF.GetStationBoard("St-Prex");
		}

		private void SetupHue()
		{
			Hue = new HueHandler();	
		}

		private void SetupVoice()
		{
			Voice = new VoiceHandler(new Otto(Hue,Sonos,weather));
		}

		private void SetupSonos()
		{
			if (Sonos == null)
				Sonos = new Music();
			Sonos.Prepare();			
		}

		private async void SetupWeather()
		{
			if (weather == null)
				weather = new WeatherHandler();
			weather.CurrentWeather.weather.id = 800;
			await weather.GetWeather();
			

			DispatcherTimer WeatherTimer = new DispatcherTimer();
			WeatherTimer.Interval = new TimeSpan(0,1, 10);
			WeatherTimer.Tick +=  async (e, r) =>
			{
				await weather.GetWeather();
			};
		}

		private void SetupClock()
		{
			DispatcherTimer Timer = new DispatcherTimer();
			Timer.Tick += (e, r) => 
			{
				Clock.Text = DateTime.Now.ToString("HH : mm");
				Day.Text = ConvertDay(DateTime.Now.DayOfWeek.ToString());
			};
			Timer.Interval = new TimeSpan(0, 0, 1);
			Timer.Start();
		}

		private string ConvertDay(string Day)
		{
			switch(Day)
			{
				case "Monday":
					return "LUNDI";
				case "Tuesday":
					return "MARDI";
				case "Wednesday":
					return "MERCREDI";
				case "Thursday":
					return "JEUDI";
				case "Friday":
					return "VENDREDI";
				case "Saturday":
					return "SAMEDI";
				case "Sunday":
					return "DIMANCHE";
				default:
					return "UNKNOWN DAY";
			}
		}
	}
}
