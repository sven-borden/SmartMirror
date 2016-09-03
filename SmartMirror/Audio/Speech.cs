using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.Media.SpeechRecognition;
using Windows.UI.Core;

namespace SmartMirror.Audio
{
	public class Speech
	{
		private SpeechRecognizer _speechRecognizer;
		private CoreDispatcher _dispatcher;
		public bool isListening = false;
		public bool newWord { get; set; }
		public string lastPhrase { get; set; }

		public Speech()
		{
			isListening = false;
			newWord = false;
			Setup();
		}

		private async void Setup()
		{
			_dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
			var permissionGained = await AudioCapturePermissions.RequestMicrophonePermission();
			if (!permissionGained)
				return;

			await InitializeRecognizer(SpeechRecognizer.SystemSpeechLanguage);

			await _speechRecognizer.ContinuousRecognitionSession.StartAsync();
		}

		private async Task InitializeRecognizer(Language systemSpeechLanguage)
		{
			if (_speechRecognizer != null)
			{
				_speechRecognizer.ContinuousRecognitionSession.Completed -= ContinuousRecognitionSession_Completed;
				_speechRecognizer.ContinuousRecognitionSession.ResultGenerated -= ContinuousRecognitionSession_ResultGenerated;
				_speechRecognizer.Dispose();
				_speechRecognizer = null;
			}
			_speechRecognizer = new SpeechRecognizer(systemSpeechLanguage);
			var expectedResponses = GetPossibilities();
			var listConstraint = new SpeechRecognitionListConstraint(expectedResponses, "Animals");

			_speechRecognizer.Constraints.Add(listConstraint);
			await _speechRecognizer.CompileConstraintsAsync();
			_speechRecognizer.ContinuousRecognitionSession.Completed += ContinuousRecognitionSession_Completed;
			_speechRecognizer.ContinuousRecognitionSession.ResultGenerated += ContinuousRecognitionSession_ResultGenerated;
			isListening = true;
		}

		private async void ContinuousRecognitionSession_Completed(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionCompletedEventArgs args)
		{
			if (_speechRecognizer.State == SpeechRecognizerState.Idle)
				await _speechRecognizer.ContinuousRecognitionSession.StartAsync();
		}

		private void ContinuousRecognitionSession_ResultGenerated(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionResultGeneratedEventArgs args)
		{
			if (args.Result.Confidence == SpeechRecognitionConfidence.Medium || args.Result.Confidence == SpeechRecognitionConfidence.High)
			{
				newWord = true;
				lastPhrase = args.Result.Text;
				//await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
				//{

				//	await ShowResult(args.Result.Text);

				//});
			}
		}

		private static string[] GetPossibilities()
		{
			return new string[] { "chocolat", "lumière" };
		}
	}
}
