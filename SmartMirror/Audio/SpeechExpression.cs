using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.Storage.Streams;

namespace SmartMirror.Audio
{
	class SpeechExpression
	{
		/// <summary>
		/// Fonction that transform a TTS input to a stream without any grammar pronounciation
		/// </summary>
		/// <param name="TTS">Text to Speech</param>
		/// <returns>Return a Stream to push to a MediaElement</returns>
		public static async Task<IRandomAccessStream> SynthesizeTextToSpeechAsync(string TTS)
		{
			// Windows.Storage.Streams.IRandomAccessStream
			IRandomAccessStream stream = null;

			// Windows.Media.SpeechSynthesis.SpeechSynthesizer
			using (SpeechSynthesizer synthesizer = new SpeechSynthesizer())
			{
				// Windows.Media.SpeechSynthesis.SpeechSynthesisStream
				stream = await synthesizer.SynthesizeTextToStreamAsync(text);
			}

			return (stream);
		}
	}
}
