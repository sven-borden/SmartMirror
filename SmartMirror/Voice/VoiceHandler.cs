using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.Media.SpeechRecognition;

namespace SmartMirror.Voice
{
	public class VoiceHandler
	{
		private bool setupDone = false;
		Otto Otto;
		public VoiceHandler(Otto _otto)
		{
			Otto = _otto;
			Setup();
		}

		private async void Setup()
		{
			bool permissionGained = await AudioCapturePermissions.RequestMicrophonePermission();
            setupDone = await InitializeRecognizer(SpeechRecognizer.SystemSpeechLanguage);
			if (!setupDone)
				Otto.Message.ShowMessage("Voice Setup Failed");
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

		private async Task<bool> InitializeRecognizer(Language recognizerLanguage)
		{

			if (speechRecognizer != null)
			{
				speechRecognizer.ContinuousRecognitionSession.Completed -= ContinuousRecognitionSession_Completed;
				speechRecognizer.ContinuousRecognitionSession.ResultGenerated -= ContinuousRecognitionSession_ResultGenerated;
                speechRecognizer.StateChanged -= SpeechRecognizer_StateChanged;
                this.speechRecognizer.Dispose();
				this.speechRecognizer = null;
			}

			try
			{
				// determine the language code being used.
				//StorageFile grammarContentFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Voice/SRGS/Sonos.xml"));
				//SpeechRecognitionGrammarFileConstraint grammarConstraint = new SpeechRecognitionGrammarFileConstraint(grammarContentFile);
				// Initialize the SpeechRecognizer and add the grammar.
				speechRecognizer = new SpeechRecognizer(new Language("en-US"));

				AddEnglishConstraint();
				speechRecognizer.StateChanged += SpeechRecognizer_StateChanged;
                SpeechRecognitionCompilationResult compilationResult = await speechRecognizer.CompileConstraintsAsync();

				// Check to make sure that the constraints were in a proper format and the recognizer was able to compile them.
				if (compilationResult.Status != SpeechRecognitionResultStatus.Success)
				{
					Debug.WriteLine(compilationResult.Status.ToString());
					return false;
				}
				else
				{
					// Set EndSilenceTimeout to give users more time to complete speaking a phrase.
					speechRecognizer.Timeouts.EndSilenceTimeout = TimeSpan.FromSeconds(0.8);
					// Handle continuous recognition events. Completed fires when various error states occur. ResultGenerated fires when
					// some recognized phrases occur, or the garbage rule is hit.
					speechRecognizer.ContinuousRecognitionSession.Completed += ContinuousRecognitionSession_Completed;
					speechRecognizer.ContinuousRecognitionSession.ResultGenerated += ContinuousRecognitionSession_ResultGenerated;
					return true;
				}
			}
			catch (Exception ex)
			{
				if ((uint)ex.HResult == HResultRecognizerNotFound)
					Debug.WriteLine("Speech Language pack for selected language not installed.");
				else
					Debug.WriteLine(ex.Message);
                return false;
			}
		}

		private void AddEnglishConstraint()
		{
			speechRecognizer.Constraints.Add
				(
					new SpeechRecognitionListConstraint
					(
						new List<string>()
						{
							"Turn on the music",
							"Turn on the sound",
							"music",
							"Play"
						}, "TurnOnSonos"
					)
				);
			speechRecognizer.Constraints.Add
			(
				new SpeechRecognitionListConstraint
				(
					new List<string>()
					{
							"Pause",
							"Stop",
							"Pause the music",
							"Stop the music",
							"Turn off the music",
							"Shut down the sound"
					}, "TurnOffSonos"
				)
			);
			speechRecognizer.Constraints.Add
			(
				new SpeechRecognitionListConstraint
				(
					new List<string>()
					{
							"Next",
							"Next song",
							"Next one",
							"Play the next song",
							"Play the next music"
					}, "NextSong"
				)
			);
			speechRecognizer.Constraints.Add
			(
				new SpeechRecognitionListConstraint
				(
					new List<string>()
					{
							"Previous",
							"Previous song",
							"Play the previous song",
							"Before"
					}, "PreviousSong"
				)
			);
			speechRecognizer.Constraints.Add
			(
				new SpeechRecognitionListConstraint
				(
					new List<string>()
					{
							"Set the volume up",
							"Volume up",
							"Louder"
					}, "SoundUp"
				)
			);
			speechRecognizer.Constraints.Add
			(
				new SpeechRecognitionListConstraint
				(
					new List<string>()
					{
							"Set the volume down",
							"Volume down",
					}, "SoundDown"
				)
			);
			speechRecognizer.Constraints.Add
			(
				new SpeechRecognitionListConstraint
				(
					new List<string>()
					{
							"Turn on the lights",
							"Lights on",
					}, "TurnOnLight"
				)
			);
			speechRecognizer.Constraints.Add
			(
				new SpeechRecognitionListConstraint
				(
					new List<string>()
					{
							"Turn off the lights",
							"Lights off",
					}, "TurnOffLight"
				)
			);
		}

		private void AddFrenchConstraint()
		{
			speechRecognizer.Constraints.Add
				(
					new SpeechRecognitionListConstraint
					(
						new List<string>()
						{
							"Allume",
							"Allume la musique",
							"Allume le son",
							"Allume le sonos",
							"Met de la musique",
							"Met la musique en marche",
						}, "TurnOnSonos"
					)
				);
			speechRecognizer.Constraints.Add
			(
				new SpeechRecognitionListConstraint
				(
					new List<string>()
					{
							"pause",
							"Stop",
							"Eteint la musique",
							"Stop la musique",
							"Arrête le son",
							"Stop le sonos",
					}, "TurnOffSonos"
				)
			);
			speechRecognizer.Constraints.Add
			(
				new SpeechRecognitionListConstraint
				(
					new List<string>()
					{
							"suivant",
							"suivante",
							"après"
					}, "NextSong"
				)
			);
			speechRecognizer.Constraints.Add
			(
				new SpeechRecognitionListConstraint
				(
					new List<string>()
					{
							"avant",
							"précédente",
							"arrière"
					}, "PreviousSong"
				)
			);
			speechRecognizer.Constraints.Add
			(
				new SpeechRecognitionListConstraint
				(
					new List<string>()
					{
							"Met plus fort",
							"Monte le son",
							"Met la musique plus fort"
					}, "SoundUp"
				)
			);
			speechRecognizer.Constraints.Add
			(
				new SpeechRecognitionListConstraint
				(
					new List<string>()
					{
							"Met moins fort",
							"Descend le son",
							"Met la musique moins fort",
							"Diminue la musique",
							"Plus doucement"
					}, "SoundDown"
				)
			);
			speechRecognizer.Constraints.Add
			(
				new SpeechRecognitionListConstraint
				(
					new List<string>()
					{
							"Allume la lumière",
							"Allume ma chambre",
							"Allume la chambre"
					}, "TurnOnLight"
				)
			);
			speechRecognizer.Constraints.Add
			(
				new SpeechRecognitionListConstraint
				(
					new List<string>()
					{
							"Eteint la lumière",
							"Eteint ma chambre",
							"Eteint la chambre"
					}, "TurnOffLight"
				)
			);
		}

		private void ContinuousRecognitionSession_ResultGenerated(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionResultGeneratedEventArgs args)
		{
			if (args.Result.Confidence == SpeechRecognitionConfidence.Medium ||	args.Result.Confidence == SpeechRecognitionConfidence.High)
			{
				Otto.Request(args.Result);
			}
        }

		private async void ContinuousRecognitionSession_Completed(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionCompletedEventArgs args)
		{
			if (speechRecognizer.State == SpeechRecognizerState.Idle)
			{
				await speechRecognizer.ContinuousRecognitionSession.StartAsync();
			}
		}

        private void SpeechRecognizer_StateChanged(SpeechRecognizer sender, SpeechRecognizerStateChangedEventArgs args)
        {
        }

        /// <summary>
        /// the HResult 0x8004503a typically represents the case where a recognizer for a particular language cannot
        /// be found
        /// </summary>
        private static uint HResultRecognizerNotFound = 0x8004503a;
		private SpeechRecognizer speechRecognizer;
	}
}
