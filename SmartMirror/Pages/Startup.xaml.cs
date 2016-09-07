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
			SetProgression();
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
			SetupItem current = SetupProgression[selectedProg];
			Title = current.LongTitle.ToString();
			Description = current.LongDescription;
		}

		private void goNextProgression()
		{
			SetupProgression[selectedProg++].Unselect();
			SetupProgression[selectedProg].Select();
		}
	}
}
