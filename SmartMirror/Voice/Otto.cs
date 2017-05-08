using SmartMirror.Content;
using SmartMirror.Hue;
using SmartMirror.Sonos;
using SmartMirror.WeatherAPI;
using Windows.Media.SpeechRecognition;

namespace SmartMirror.Voice
{
	/// <summary>
	/// Otto is the master, he dispatch what is needed from the recognition
	/// </summary>
	public class Otto
	{

		public HueHandler Hue { get; private set; }
		public Music Sonos { get; private set; }
		public WeatherHandler Weather { get; private set; }
		public Message Message;

		public Otto(HueHandler _hue, Music _sonos, WeatherHandler _weather, Message _m)
		{
			Hue = _hue;
			Sonos = _sonos;
			Weather = _weather;
			Message = _m;
		}

		public async void Request(SpeechRecognitionResult Rule)
		{
			switch(Rule.Constraint.Tag)
			{
				case "TurnOnSonos":
					Sonos.Play();
					break;
				case "TurnOffSonos":
					Sonos.Pause();
					break;
				case "NextSong":
					Sonos.Next();
					break;
				case "PreviousSong":
					Sonos.Previous();
					break;
				case "SoundUp":
					int volume = await Sonos.GetVolume() + 10;
					if (volume > 100)
						volume = 100;
					Sonos.SetVolume(volume);
					break;
				case "SoundDown":
					int volum = await Sonos.GetVolume() - 10;
					if (volum < 0)
						volum = 0;
					Sonos.SetVolume(volum);
					break;
				case "TurnOffLight":
					Hue.TurnOffLights();
					break;
				case "TurnOnLight":
					Hue.TurnOnLights();
					break;
				case "Fort":
					Sonos.SetVolume(100);
					break;
			}
		}
	}
}