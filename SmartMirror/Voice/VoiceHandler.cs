using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Resources.Core;
using Windows.Globalization;
using Windows.Media.SpeechRecognition;
using Windows.Storage;

namespace SmartMirror.Voice
{
	public class VoiceHandler
	{
		private bool setupDone = false;
		public VoiceHandler()
		{
			Setup();
		}

		private async void Setup()
		{
			bool permissionGained = await AudioCapturePermissions.RequestMicrophonePermission();
			await InitializeRecognizer(SpeechRecognizer.SystemSpeechLanguage);
			setupDone = true;
			Start();
		}

		public async void Start()
		{
			if (!setupDone)
				return;
			await speechRecognizer.ContinuousRecognitionSession.StartAsync();
		}

		public async void Stop()
		{
			if (!setupDone)
				return;
			await speechRecognizer.ContinuousRecognitionSession.StopAsync();
		}

		private async Task InitializeRecognizer(Language recognizerLanguage)
		{
			if (speechRecognizer != null)
			{
				speechRecognizer.ContinuousRecognitionSession.Completed -= ContinuousRecognitionSession_Completed;
				speechRecognizer.ContinuousRecognitionSession.ResultGenerated -= ContinuousRecognitionSession_ResultGenerated;

				this.speechRecognizer.Dispose();
				this.speechRecognizer = null;
			}

			try
			{
				// determine the language code being used.
				StorageFile grammarContentFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Voice/SRGS/Sonos.xml"));
				SpeechRecognitionGrammarFileConstraint grammarConstraint = new SpeechRecognitionGrammarFileConstraint(grammarContentFile);
				// Initialize the SpeechRecognizer and add the grammar.
				speechRecognizer = new SpeechRecognizer(new Language("fr-FR"));
				
				speechRecognizer.Constraints.Add(grammarConstraint);
				SpeechRecognitionCompilationResult compilationResult = await speechRecognizer.CompileConstraintsAsync();

				// Check to make sure that the constraints were in a proper format and the recognizer was able to compile them.
				if (compilationResult.Status != SpeechRecognitionResultStatus.Success)
				{
					Debug.WriteLine(compilationResult.Status.ToString());
				}
				else
				{
					// Set EndSilenceTimeout to give users more time to complete speaking a phrase.
					speechRecognizer.Timeouts.EndSilenceTimeout = TimeSpan.FromSeconds(1.2);
					// Handle continuous recognition events. Completed fires when various error states occur. ResultGenerated fires when
					// some recognized phrases occur, or the garbage rule is hit.
					speechRecognizer.ContinuousRecognitionSession.Completed += ContinuousRecognitionSession_Completed;
					speechRecognizer.ContinuousRecognitionSession.ResultGenerated += ContinuousRecognitionSession_ResultGenerated;
				}
			}
			catch (Exception ex)
			{
				if ((uint)ex.HResult == HResultRecognizerNotFound)
					Debug.WriteLine("Speech Language pack for selected language not installed.");
				else
					Debug.WriteLine(ex.Message);
			}
		}

		private void ContinuousRecognitionSession_ResultGenerated(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionResultGeneratedEventArgs args)
		{
			if (args.Result.Confidence == SpeechRecognitionConfidence.Medium ||	args.Result.Confidence == SpeechRecognitionConfidence.High)
			{
				args.Result.RulePath.ToString();
			}
		}

		private void ContinuousRecognitionSession_Completed(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionCompletedEventArgs args)
		{
			//BOF quoi faire avec ce truc
		}

		/// <summary>
		/// the HResult 0x8004503a typically represents the case where a recognizer for a particular language cannot
		/// be found
		/// </summary>
		private static uint HResultRecognizerNotFound = 0x8004503a;
		private SpeechRecognizer speechRecognizer;
		private ResourceContext speechContext;
		private ResourceMap speechResourceMap;
	}
}
