using SmartMirror.Audio.Recognition.Setup;
using SmartMirror.Class.Setup;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, voir la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace SmartMirror.Pages
{
	/// <summary>
	/// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
	/// </summary>
	public sealed partial class Startup : Page
	{
		ObservableCollection<SetupItem> SetupProgression;
		public string Title = "";
		public string Description = "";
		int selectedProg = 0;

		public Startup()
		{
			//ApplicationLanguages.PrimaryLanguageOverride = "fr-CH";
			this.InitializeComponent();

			DispatcherTimer checkConnectivity = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 20) };
			checkConnectivity.Tick += (tick, args) =>
			{
				checkConnectivity.Stop();
				if (InternetAccess.Internet.isConnected())
					SetProgression();
				else
				{
					Title = "No Internet Access";
					Description = "Please ensure that an active internet connection is available, SmartMirror will proceed to setup once connection come back again";
					this.Bindings.Update();
					checkConnectivity.Interval = new TimeSpan(0, 0, 8);
					checkConnectivity.Start();
				}
			};
			checkConnectivity.Start();
		}

		private void isConnected()
		{
			throw new NotImplementedException();
		}

		private void SetProgression()
		{
			if (SetupProgression == null)
				SetupProgression = new ObservableCollection<SetupItem>();
			else
				SetupProgression.Clear();

			ResourceLoader l = new Windows.ApplicationModel.Resources.ResourceLoader();

			SetupProgression.Add
			(
				new SetupItem
				(
					l.GetString("ProgIntroT"),
					l.GetString("ProgIntroD"),
					l.GetString("SetupHi"),
					l.GetString("SetupIntroText"),
					new List<SetupQuestion>()
					{
						new SetupQuestion()
						{
							Question = l.GetString("SetupIntroStart"),
							Suggestion = l.GetString("SetupIntroSuggestion"),
							Setting = null
						}
					}
				)
			);
			SetupProgression.Add
			(
				new SetupItem
				(
					l.GetString("ProgLangT"),
					l.GetString("ProgLangD"),
					l.GetString("LangTitle"),
					l.GetString("LangDescription"),
					new List<SetupQuestion>()
					{
						new SetupQuestion()
						{
							Question = l.GetString("LangChoose"),
							Suggestion = l.GetString("LangSuggestion"),
							Setting = "UserLanguage"
						}
					}
				)
			);
			SetupProgression.Add
			(
				new SetupItem
				(
					l.GetString("ProgVoiceT"),
					l.GetString("ProgVoiceD"),
					l.GetString("VoiceTitle"),
					l.GetString("VoiceDescription"),
					new List<SetupQuestion>()
					{
						new SetupQuestion()
						{
							Question = l.GetString("VoiceChoose"),
							Suggestion = l.GetString("VoiceSuggestion"),
							Setting = "UserVoice"
						}
					} 
				)
			);

			foreach (var item in SetupProgression)
				item.Unselect();
			SetupProgression[selectedProg].Select();
			Display(selectedProg);
		}

		private void Display(int selectedProg)
		{
			ResourceLoader l = new Windows.ApplicationModel.Resources.ResourceLoader();

			SetupItem current = SetupProgression[selectedProg];
			Title = current.LongTitle.ToString();
			Description = current.LongDescription;

			QuestionListPanel.Children.Clear();
			foreach (SetupQuestion question in current.Questions)
			{
				QuestionListPanel.Children.Add(new TextBlock()
				{
					Text = l.GetString(question.Question),
					Foreground = new SolidColorBrush(Colors.White),
					Margin = new Thickness(10)
				});
				QuestionListPanel.Children.Add(new TextBlock()
				{
					Text = "Try " + l.GetString(question.Suggestion),
					Foreground = new SolidColorBrush(Colors.LightGray),
					Margin = new Thickness(10)
				});

				SetupSpeechRecognition recognition = new SetupSpeechRecognition
				(
					new Windows.Media.SpeechRecognition.SpeechRecognitionListConstraint[] 
					{
						new Windows.Media.SpeechRecognition.SpeechRecognitionListConstraint(l.GetString(question.Suggestion).Split(','))
					},
					Windows.Media.SpeechRecognition.SpeechRecognizer.SystemSpeechLanguage
				);
			}
		}

		private void goNextProgression()
		{
			SetupProgression[selectedProg++].Unselect();
			SetupProgression[selectedProg].Select();
		}
	}
}
