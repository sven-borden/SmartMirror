using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;

namespace SmartMirror.Content
{
	public class Message : INotifyPropertyChanged
	{
		public Message()
		{
			DispatcherTimer timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 5) };
			timer.Tick += (e, r) =>
			{
				if (Queue.Count == 0)
				{
					MessageToShow = string.Empty;
					NMessages = Queue.Count;
					return;
				}
				MessageToShow = Queue.First();
				NMessages = Queue.Count;
				Queue.RemoveAt(0);
			};
			timer.Start();

		}

		private List<string> Queue = new List<string>();

		public void ShowMessage(string message)
		{
			Queue.Add(message);
			NMessages = Queue.Count;
		}

		private string message = string.Empty;
		public string MessageToShow
		{
			get
			{
				return message;
			}
			set
			{
				message = value;
				OnPropertyChange("MessageToShow");
			}
		}

		private int nMessages = 0;
		public int NMessages
		{
			get { return nMessages; }
			set { nMessages = value; OnPropertyChange("NMessages"); }
		}

		private void OnPropertyChange(string v)
		{
			if (this.PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(v));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
