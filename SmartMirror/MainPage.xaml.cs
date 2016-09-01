using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization;
using Windows.Media.SpeechRecognition;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SmartMirror
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
		ObservableCollection<Weather> WeatherList;
		ObservableCollection<Calendar> CalendarList;
		ObservableCollection<News> NewsList;
		private bool isListening = false;
		// Speech events may come in on a thread other than the UI thread, keep track of the UI thread's
		// dispatcher, so we can update the UI in a thread-safe manner.
		private CoreDispatcher dispatcher;
		// The speech recognizer used throughout this sample.
		private SpeechRecognizer speechRecognizer;
		/// <summary>
		/// the HResult 0x8004503a typically represents the case where a recognizer for a particular language cannot
		/// be found. This may occur if the language is installed, but the speech pack for that language is not.
		/// See Settings -> Time & Language -> Region & Language -> *Language* -> Options -> Speech Language Options.
		/// </summary>
		private static uint HResultRecognizerNotFound = 0x8004503a;
		private ResourceContext speechContext;
		private ResourceMap speechResourceMap;

		public Task<bool> AudioCapturePermissions { get; private set; }

		// Keep track of whether the continuous recognizer is currently running, so it can be cleaned up appropriately.

		public MainPage()
        {
            this.InitializeComponent();
			isListening = false;
			Initialisation();
        }

		private void Initialisation()
		{
			GetWeather();
			DispatcherTimer time = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 20) };
			time.Tick += (tick, args) =>
			{
				//get time and update it
			};
			time.Start();
		}

		#region speech
		/// <summary>
		/// Upon entering the scenario, ensure that we have permissions to use the Microphone. This may entail popping up
		/// a dialog to the user on Desktop systems. Only enable functionality once we've gained that permission in order to 
		/// prevent errors from occurring when using the SpeechRecognizer. If speech is not a primary input mechanism, developers
		/// should consider disabling appropriate parts of the UI if the user does not have a recording device, or does not allow 
		/// audio input.
		/// </summary>
		/// <param name="e">Unused navigation parameters</param>
		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
			// Keep track of the UI thread dispatcher, as speech events will come in on a separate thread.
			dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
			// Prompt the user for permission to access the microphone. This request will only happen
			// once, it will not re-prompt if the user rejects the permission.
			bool permissionGained = await AudioCapturePermissions.RequestMicrophonePermission();
			if (permissionGained)
			{
				// Initialize resource map to retrieve localized speech strings.
				Language speechLanguage = SpeechRecognizer.SystemSpeechLanguage;
				string langTag = speechLanguage.LanguageTag;
				speechContext = ResourceContext.GetForCurrentView();
				speechContext.Languages = new string[] { langTag };
				speechResourceMap = ResourceManager.Current.MainResourceMap.GetSubtree("LocalizationSpeechResources");
				await InitializeRecognizer(SpeechRecognizer.SystemSpeechLanguage);
			}
			else
			{ 
			}
		}

		/// <summary>
		/// Upon leaving, clean up the speech recognizer. Ensure we aren't still listening, and disable the event
		/// handlers to prevent leaks
		/// </summary>
		/// <param name="e">Unused navigation parameters.</param>
		protected async override void OnNavigatedFrom(NavigationEventArgs e)
		{
			if (this.speechRecognizer != null)
			{
				if (isListening)
				{
					await this.speechRecognizer.ContinuousRecognitionSession.CancelAsync();
					isListening = false;
				}
				heardYouSayTextBlock.Visibility = resultTextBlock.Visibility = Visibility.Collapsed;
				speechRecognizer.ContinuousRecognitionSession.Completed -= ContinuousRecognitionSession_Completed;
				speechRecognizer.ContinuousRecognitionSession.ResultGenerated -= ContinuousRecognitionSession_ResultGenerated;
				speechRecognizer.StateChanged -= SpeechRecognizer_StateChanged;
				this.speechRecognizer.Dispose();
				this.speechRecognizer = null;
			}
		}
	
		/// <summary>
		/// Initialize Speech Recognizer and compile constraints.
		/// </summary>
		/// <param name="recognizerLanguage">Language to use for the speech recognizer</param>
		/// <returns>Awaitable task.</returns>
		private async Task InitializeRecognizer(Language recognizerLanguage)
		{
			if (speechRecognizer != null)
			{
				// cleanup prior to re-initializing this scenario.
				speechRecognizer.StateChanged -= SpeechRecognizer_StateChanged;
				speechRecognizer.ContinuousRecognitionSession.Completed -= ContinuousRecognitionSession_Completed;
				speechRecognizer.ContinuousRecognitionSession.ResultGenerated -= ContinuousRecognitionSession_ResultGenerated;

				this.speechRecognizer.Dispose();
				this.speechRecognizer = null;
			}
			try
			{
				this.speechRecognizer = new SpeechRecognizer(recognizerLanguage);
				// Provide feedback to the user about the state of the recognizer. This can be used to provide visual feedback in the form
				// of an audio indicator to help the user understand whether they're being heard.
				speechRecognizer.StateChanged += SpeechRecognizer_StateChanged;

				// Build a command-list grammar. Commands should ideally be drawn from a resource file for localization, and 
				// be grouped into tags for alternate forms of the same command.
				speechRecognizer.Constraints.Add
				(
					new SpeechRecognitionListConstraint
					(
						new List<string>()
						{
							speechResourceMap.GetValue("ListGrammarGoHome", speechContext).ValueAsString
						}, 
						"Home"
					)
				);
				speechRecognizer.Constraints.Add
				(
					new SpeechRecognitionListConstraint
					(
						new List<string>()
						{
							speechResourceMap.GetValue("ListGrammarGoToContosoStudio", speechContext).ValueAsString

						}, 
						"GoToContosoStudio"
					)	
				);
				speechRecognizer.Constraints.Add
				(
					new SpeechRecognitionListConstraint
					(
						new List<string>()
						{
							speechResourceMap.GetValue("ListGrammarShowMessage", speechContext).ValueAsString,
							speechResourceMap.GetValue("ListGrammarOpenMessage", speechContext).ValueAsString

						}, 
						"Message"
					)
				);
				speechRecognizer.Constraints.Add
				(
					new SpeechRecognitionListConstraint
					(
						new List<string>()
						{
							speechResourceMap.GetValue("ListGrammarSendEmail", speechContext).ValueAsString,
							speechResourceMap.GetValue("ListGrammarCreateEmail", speechContext).ValueAsString

						}, 
						"Email"
					)
				);

				// Update the help text in the UI to show localized examples
				string uiOptionsText = string.Format("Try saying '{0}', '{1}' or '{2}'",
					speechResourceMap.GetValue("ListGrammarGoHome", speechContext).ValueAsString,
					speechResourceMap.GetValue("ListGrammarGoToContosoStudio", speechContext).ValueAsString,
					speechResourceMap.GetValue("ListGrammarShowMessage", speechContext).ValueAsString);

				listGrammarHelpText.Text = string.Format("{0}\n{1}",
					speechResourceMap.GetValue("ListGrammarHelpText", speechContext).ValueAsString,
					uiOptionsText);

				SpeechRecognitionCompilationResult result = await speechRecognizer.CompileConstraintsAsync();
				if (result.Status != SpeechRecognitionResultStatus.Success)
				{ 
					// Let the user know that the grammar didn't compile properly.
					resultTextBlock.Visibility = Visibility.Visible;
					resultTextBlock.Text = "Unable to compile grammar.";
				}
				else
				{
					resultTextBlock.Visibility = Visibility.Collapsed;
					// Handle continuous recognition events. Completed fires when various error states occur. ResultGenerated fires when
					// some recognized phrases occur, or the garbage rule is hit.
					speechRecognizer.ContinuousRecognitionSession.Completed += ContinuousRecognitionSession_Completed;
					speechRecognizer.ContinuousRecognitionSession.ResultGenerated += ContinuousRecognitionSession_ResultGenerated;
				}
			}
			catch (Exception ex)
			{
				if ((uint)ex.HResult == HResultRecognizerNotFound)
				{
					resultTextBlock.Visibility = Visibility.Visible;
					resultTextBlock.Text = "Speech Language pack for selected language not installed.";
				}
				else
				{
					resultTextBlock.Visibility = Visibility.Visible;
					resultTextBlock.Text = "Unknown Error";
				}
			}
		}

		/// <summary>
		/// Handle events fired when error conditions occur, such as the microphone becoming unavailable, or if
		/// some transient issues occur.
		/// </summary>
		/// <param name="sender">The continuous recognition session</param>
		/// <param name="args">The state of the recognizer</param>
		private async void ContinuousRecognitionSession_Completed(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionCompletedEventArgs args)
		{
			if (args.Status != SpeechRecognitionResultStatus.Success)
			{
				await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
				{
					isListening = false;
				});
			}
		}

		/// <summary>

		/// Handle events fired when a result is generated. This may include a garbage rule that fires when general room noise

		/// or side-talk is captured (this will have a confidence of Rejected typically, but may occasionally match a rule with

		/// low confidence).

		/// </summary>

		/// <param name="sender">The Recognition session that generated this result</param>

		/// <param name="args">Details about the recognized speech</param>

		private async void ContinuousRecognitionSession_ResultGenerated(SpeechContinuousRecognitionSession sender, SpeechContinuousRecognitionResultGeneratedEventArgs args)

		{

			// The garbage rule will not have a tag associated with it, the other rules will return a string matching the tag provided

			// when generating the grammar.

			string tag = "unknown";

			if (args.Result.Constraint != null)

			{

				tag = args.Result.Constraint.Tag;

			}



			// Developers may decide to use per-phrase confidence levels in order to tune the behavior of their 

			// grammar based on testing.

			if (args.Result.Confidence == SpeechRecognitionConfidence.Medium ||

			args.Result.Confidence == SpeechRecognitionConfidence.High)

			{

				await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>

				{

					heardYouSayTextBlock.Visibility = Visibility.Visible;

					resultTextBlock.Visibility = Visibility.Visible;

					resultTextBlock.Text = string.Format("Heard: '{0}', (Tag: '{1}', Confidence: {2})", args.Result.Text, tag, args.Result.Confidence.ToString());

				});

			}

			else

			{

				// In some scenarios, a developer may choose to ignore giving the user feedback in this case, if speech

				// is not the primary input mechanism for the application.

				await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>

				{

					heardYouSayTextBlock.Visibility = Visibility.Collapsed;

					resultTextBlock.Visibility = Visibility.Visible;

					resultTextBlock.Text = string.Format("Sorry, I didn't catch that. (Heard: '{0}', Tag: {1}, Confidence: {2})", args.Result.Text, tag, args.Result.Confidence.ToString());

				});

			}

		}

		private void StartRecognition()
		{
			if (isListening == false)

			{

				// The recognizer can only start listening in a continuous fashion if the recognizer is currently idle.

				// This prevents an exception from occurring.

				if (speechRecognizer.State == SpeechRecognizerState.Idle)

				{

					try

					{

						await speechRecognizer.ContinuousRecognitionSession.StartAsync();

						ContinuousRecoButtonText.Text = " Stop Continuous Recognition";

						cbLanguageSelection.IsEnabled = false;

						isListening = true;

					}

					catch (Exception ex)

					{

						var messageDialog = new Windows.UI.Popups.MessageDialog(ex.Message, "Exception");

						await messageDialog.ShowAsync();

					}

				}

			}

			else

			{

				isListening = false;

				ContinuousRecoButtonText.Text = " Continuous Recognition";

				cbLanguageSelection.IsEnabled = true;



				heardYouSayTextBlock.Visibility = Visibility.Collapsed;

				resultTextBlock.Visibility = Visibility.Collapsed;

				if (speechRecognizer.State != SpeechRecognizerState.Idle)

				{

					try

					{

						// Cancelling recognition prevents any currently recognized speech from

						// generating a ResultGenerated event. StopAsync() will allow the final session to 

						// complete.

						await speechRecognizer.ContinuousRecognitionSession.CancelAsync();

					}

					catch (Exception ex)

					{

						var messageDialog = new Windows.UI.Popups.MessageDialog(ex.Message, "Exception");

						await messageDialog.ShowAsync();

					}

				}

			}
		}

		#endregion

		private void GetWeather()
		{
			
		}
	}
}
