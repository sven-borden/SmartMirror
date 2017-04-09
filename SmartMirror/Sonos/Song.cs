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
		public string Title { get; set; }
		public string Creator { get; set; }
		public string Album { get; set; }
		public int Duration { get; set; }
		public int Remaining { get; set; }
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
