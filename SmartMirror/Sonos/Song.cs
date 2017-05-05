using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror.Sonos
{
	public class Song : INotifyPropertyChanged
	{
		private string title = string.Empty;
		public string Title { get { return title; } set { title = value; OnPropertyChanged(nameof(Title)); } }

		private string creator = string.Empty;
		public string Creator { get { return creator; } set { creator = value; OnPropertyChanged(nameof(creator)); } }

		private string album = string.Empty;
		public string Album { get { return album; } set { album = value; OnPropertyChanged(nameof(album)); } }

		public int Duration { get; set; }
		public int Remaining { get; set; }
		public bool IsRadio { get; set; }
		private int realTime;

		public event PropertyChangedEventHandler PropertyChanged;

		public int RealTime
		{
			get
			{
				return realTime;
			}
			set
			{
				realTime = value;
				OnPropertyChanged("RealTime");
			}
		}

		private void OnPropertyChanged(string name)
		{
			if (this.PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		public Song()
		{
			Title = "No Title";
			Album = "Unkown Album";
			Creator = "Unkown Creator";
			Duration = 1;
			RealTime = 0;
			Remaining = 0;
		}
	}
}
