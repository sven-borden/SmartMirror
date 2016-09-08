using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.Media.SpeechRecognition;
using Windows.UI.Core;

namespace SmartMirror.Audio.Recognition.Setup
{
	class SetupSpeechRecognition
	{
		private SpeechRecognizer _speechRecognizer;
		private CoreDispatcher _dispatcher;

		/// <summary>
		/// Boolean that represent the state of Listening of the System
		/// </summary>
		public bool isListening = false;

		/// <summary>
		/// Boolean set to true if a new Phrase has been set
		/// </summary>
		public bool newWord { get; set; }

		/// <summary>
		/// Keep in mind the last user Phrase
		/// </summary>
		public string lastPhrase { get; set; }

		private SpeechRecognitionListConstraint[] Constraints { get; set;}
		private Language Lang { get; set; }
		/// <summary>
		/// Class used to Speech Recognition, that's all
		/// </summary>
		/// <remarks>You stupid? Why are you still reading this?</remarks>
		public SetupSpeechRecognition(SpeechRecognitionListConstraint[] _constraints, Language _lang)
		{
			isListening = false;
			newWord = false;
			Constraints = _constraints;
			Lang = _lang;
			Setup();
		}

		/// <summary>
		/// Get microphone permissions and dispatch recognition
		/// </summary>
		private async void Setup()
		{
			_dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
			var permissionGained = await AudioCapturePermissions.RequestMicrophonePermission();
			if (!permissionGained)
				return;

			await InitializeRecognizer(Lang);

			await _speechRecognizer.ContinuousRecognitionSession.StartAsync();
		}

		/// <summary>
		/// Initialise a recognition system with a specified language
		/// </summary>
		/// <param name="systemSpeechLanguage"></param>
		/// <returns></returns>
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

			foreach(var item in Constraints)
				_speechRecognizer.Constraints.Add(item);

			await _speechRecognizer.CompileConstraintsAsync();
			_speechRecognizer.ContinuousRecognitionSession.Completed += ContinuousRecognitionSession_Completed;
			_speechRecognizer.ContinuousRecognitionSession.ResultGenerated += ContinuousRecognitionSession_ResultGenerated;
			isListening = true;
		}

		/// <summary>
		/// Restart continuous recognition session by itself when completed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private async void ContinuousRecognitionSession_Completed(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionCompletedEventArgs args)
		{
			if (_speechRecognizer.State == SpeechRecognizerState.Idle)
				await _speechRecognizer.ContinuousRecognitionSession.StartAsync();
		}

		/// <summary>
		/// Result generated when recognize a constraint
		/// </summary>
		/// <param name="sender">Session</param>
		/// <param name="args">Result</param>
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
	}
}
