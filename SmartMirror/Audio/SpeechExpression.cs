using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;

namespace SmartMirror.Audio
{
	class SpeechExpression
	{
		public VoiceGender voiceGender { get; set; }

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
				VoiceInformation voiceInfo =
				(
					from voice in SpeechSynthesizer.AllVoices
					where voice.Gender == VoiceGender.Female
					select voice
				).FirstOrDefault() ?? SpeechSynthesizer.DefaultVoice;

				synthesizer.Voice = voiceInfo;

				// Windows.Media.SpeechSynthesis.SpeechSynthesisStream
				stream = await synthesizer.SynthesizeTextToStreamAsync(TTS);
			}

			return (stream);
		}

		/// <summary>
		/// Fonction that transform a TTS input to a stream with grammar pronounciation
		/// </summary>
		/// <param name="ssmlFile"></param>
		/// <returns></returns>
		async Task<IRandomAccessStream> SynthesizeSsmlToSpeechAsync(StorageFile ssmlFile)
		{
			// Windows.Storage.Streams.IRandomAccessStream
			IRandomAccessStream stream = null;

			// Windows.Media.SpeechSynthesis.SpeechSynthesizer
			using (SpeechSynthesizer synthesizer = new SpeechSynthesizer())
			{
				// Windows.Media.SpeechSynthesis.SpeechSynthesisStream
				string text = await FileIO.ReadTextAsync(ssmlFile);

				stream = await synthesizer.SynthesizeSsmlToStreamAsync(text);
			}
			return (stream);
		}

		/// <summary>
		/// Speak literaly
		/// </summary>
		/// <param name="text">TTS</param>
		/// <param name="mediaElement">Where to speak</param>
		/// <returns></returns>
		async Task SpeakTextAsync(string text, MediaElement mediaElement)
		{
			IRandomAccessStream stream = await SynthesizeTextToSpeechAsync(text);
			await mediaElement.PlayStreamAsync(stream, true);
		}

		/// <summary>
		/// Speak literaly with grammar
		/// </summary>
		/// <param name="ssmlFile">SSML File location</param>
		/// <param name="mediaElement">Where to speak</param>
		/// <returns></returns>
		async Task SpeakSsmlFileAsync(StorageFile ssmlFile, MediaElement mediaElement)
		{
			IRandomAccessStream stream = await this.SynthesizeSsmlToSpeechAsync(ssmlFile);
			await mediaElement.PlayStreamAsync(stream, true);
		}
	}
}
