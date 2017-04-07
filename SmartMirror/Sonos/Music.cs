using Sonos.Client;
using Sonos.Client.Models;
using Sonos.Client.Models.Spotify;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace SmartMirror.Sonos
{
	public class Music : INotifyPropertyChanged
	{
		DispatcherTimer timer;
		public string SonosIP { get; set; }
		SonosClient Sonos = null;

		/// <summary>
		/// Int correcponding to the volume
		/// </summary>
		public int Volume { get; set; }

		private bool _IsPlaying = false;
		public bool IsPlaying
		{
			get
			{ 
				return _IsPlaying;
			}
			set
			{
				_IsPlaying = value;
				OnPropertyChanged("IsPlaying");
			}
		}

		private Song currentSong = new Song();
		public Song CurrentSong
		{
			get
			{
				return currentSong;
			}
			set
			{
				currentSong = value;
				OnPropertyChanged("CurrentSong");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		void OnPropertyChanged(string name)
		{
			if (this.PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(name));
			}

		}


		public Music(string _sonosIp = "192.168.1.109")
		{
			SonosIP = _sonosIp;
			timer = new DispatcherTimer();
			timer.Interval = new TimeSpan(0, 0, 5);
			timer.Tick += (e, o) =>
			{
				RefreshSong();
			};
		}

		/// <summary>
		/// update Data that concern song, playing status etc
		/// </summary>
		private async void RefreshSong()
		{
			await isPlaying();
			PositionInfoResponse positionInfo = await Sonos.GetPositionInfo();

			SetTrackDataFromMeta(positionInfo.TrackMetaData, positionInfo.TrackDuration, positionInfo.RelTime);
		}

		/// <summary>
		/// Parse the metaData of the track to get name, album and singer
		/// </summary>
		/// <param name="trackMetaData"></param>
		private void SetTrackDataFromMeta(string trackMetaData, string _duration, string _reltime)
		{
			//Contribution
			trackMetaData = trackMetaData.Replace("&lt;", "<");
			trackMetaData = trackMetaData.Replace("&gt;", ">");
			trackMetaData = trackMetaData.Replace("&quot;", "\"");
			trackMetaData = trackMetaData.Replace("&amp;", "&");
			string _title = trackMetaData.Split(new string[] { "<dc:title>", "</dc:title>" }, StringSplitOptions.RemoveEmptyEntries)[1];
			string _album = trackMetaData.Split(new string[] { "<album>", "</album>" }, StringSplitOptions.RemoveEmptyEntries)[1];
			string _creator = trackMetaData.Split(new string[] { "<dc:creator>", "</dc:creator>" }, StringSplitOptions.RemoveEmptyEntries)[1];

			CurrentSong = new Song() { Title = _title,Album = _album,Creator = _creator,Duration = ConvertToSecond(_duration), RealTime = ConvertToSecond(_reltime)};
		}

		private int ConvertToSecond(string duration)
		{
			string[] tmp = duration.Split(':');
			int[] tmpi = new int[] { Convert.ToInt32(tmp[0]), Convert.ToInt32(tmp[1]), Convert.ToInt32(tmp[2]) };
			return tmpi[2] + 60 * tmpi[1] + 3600 * tmpi[0];
		}

		/// <summary>
		/// Return if it is playing
		/// </summary>
		/// <returns></returns>
		public async Task<bool> isPlaying()
		{
			try
			{
				IsPlaying = await Sonos.IsPlaying();
			}
			catch(NullReferenceException e)
			{
				Debug.WriteLine(e.Message);
			}
			return IsPlaying;
		}

		/// <summary>
		/// Setup the device
		/// </summary>
		public void Prepare()
		{
			Sonos = new SonosClient(SonosIP);
			timer.Start();
		}

		/// <summary>
		/// Start to play
		/// </summary>
		public async void Play()
		{
			try
			{
				await Sonos.Play();
			}
			catch(NullReferenceException e)
			{
				Debug.WriteLine("Play " + e.Message);
			}
		}

		/// <summary>
		/// Pause the music
		/// </summary>
		public async void Pause()
		{
			await Sonos.Pause();
		}

		/// <summary>
		/// Play Next song
		/// </summary>
		public async void Next()
		{
			await Sonos.Next();
		}

		/// <summary>
		/// Play Previous Song
		/// </summary>
		public async void Previous()
		{
			await Sonos.Previous();
		}

		/// <summary>
		/// Set the volume to an certain amount
		/// </summary>
		/// <param name="volume"></param>
		public async void SetVolume(int volume)
		{
			await Sonos.SetVolume(volume);
		}

		/// <summary>
		/// Return the Volume
		/// </summary>
		public async Task<int> GetVolume()
		{
			return await Sonos.GetVolume();
		}
	}


	public class PlayingConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			bool t = (bool)value;
			if (t)
				return "\uE768";
			return "\uE769";
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}

	public class TitleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
