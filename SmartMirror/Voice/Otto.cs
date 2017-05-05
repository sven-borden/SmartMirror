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
					Message.ShowMessage("Play Sonos");
					Sonos.Play();
					break;
				case "TurnOffSonos":
					Sonos.Pause();
					Message.ShowMessage("Pause Sonos");
					break;
				case "NextSong":
					Sonos.Next();
					Message.ShowMessage("Next song");
					break;
				case "PreviousSong":
					Sonos.Previous();
					Message.ShowMessage("Previous song");
					break;
				case "SoundUp":
					int volume = await Sonos.GetVolume() + 10;
					if (volume > 100)
						volume = 100;
					Sonos.SetVolume(volume);
					Message.ShowMessage($"Volume at {volume}");
					break;
				case "SoundDown":
					int volum = await Sonos.GetVolume() - 10;
					if (volum < 0)
						volum = 0;
					Sonos.SetVolume(volum);
					Message.ShowMessage($"Volume at {volum}");
					break;
			}
		}
	}
}