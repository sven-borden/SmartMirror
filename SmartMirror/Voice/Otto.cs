using SmartMirror.Content;
using SmartMirror.Hue;
using SmartMirror.Sonos;
using SmartMirror.WeatherAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
		private Message Message;

		public Otto(HueHandler _hue, Music _sonos, WeatherHandler _weather, Message _m)
		{
			Hue = _hue;
			Sonos = _sonos;
			Weather = _weather;
			Message = _m;
		}

		public void Request(SpeechRecognitionResult Rule)
		{
			Debug.WriteLine("Request");
			if (Rule.Confidence == SpeechRecognitionConfidence.Low || Rule.Confidence == SpeechRecognitionConfidence.Rejected)
				return;
			foreach (var r in Rule.RulePath)
			{
				switch (r)
				{
					case "Sonos":
						SonosRequest(Rule.SemanticInterpretation.Properties);
						break;
				}
			}

		}

		private void SonosRequest(IReadOnlyDictionary<string, IReadOnlyList<string>> properties)
		{
			
		}
	}
}