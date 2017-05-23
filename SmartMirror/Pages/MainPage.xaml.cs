using SmartMirror.CFF;
using SmartMirror.Content;
using SmartMirror.Hue;
using SmartMirror.Sonos;
using SmartMirror.Voice;
using SmartMirror.WeatherAPI;
using System;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SmartMirror.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		public WeatherHandler weather;
		Music Sonos;
		VoiceHandler Voice;
		HueHandler Hue;
		Handler CFF;
		Message Message = new Message();

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
			ApplicationViewTitleBar formattableTitleBar = ApplicationView.GetForCurrentView().TitleBar;
			formattableTitleBar.ButtonBackgroundColor = Colors.Transparent;
			CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
			coreTitleBar.ExtendViewIntoTitleBar = true;
			CFF = new Handler(Message);
			await CFF.GetStationBoard("St-Prex");
			Message.ShowMessage("Train Updated");
		}

		private void SetupHue()
		{
			Hue = new HueHandler(Message);
		}

		private void SetupVoice()
		{
			Voice = new VoiceHandler(new Otto(Hue,Sonos,weather, Message));
		}

		private void SetupSonos()
		{
			if (Sonos == null)
				Sonos = new Music(Message);
			Sonos.Prepare();
            Sonos.Play();
		}

		private async void SetupWeather()
		{
			Message.ShowMessage("Start Setup Weather");
			if (weather == null)
				weather = new WeatherHandler(Message);
			weather.CurrentWeather.weather.id = 800;
			await weather.GetWeather();
			

			DispatcherTimer WeatherTimer = new DispatcherTimer();
			WeatherTimer.Interval = new TimeSpan(0,1, 10);
			WeatherTimer.Tick +=  async (e, r) =>
			{
				await weather.GetWeather();
			};
			WeatherTimer.Start();
			Message.ShowMessage("Weather setup completed");
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
