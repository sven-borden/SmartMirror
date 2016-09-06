using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace SmartMirror.Pages
{
	class SetupItem
	{
		public SolidColorBrush Color { get; set; }
		public string Title { get; }
		public string Description { get; }
		public SolidColorBrush SelectColor { get; }
		public SolidColorBrush UnselectColor { get; }
		public Visibility isSelected { get; set; }

		public SetupItem(string _title, string _description)
		{
			SelectColor = new SolidColorBrush(Colors.White);
			UnselectColor = new SolidColorBrush(Colors.LightGray);
			Color = UnselectColor;
			Title = _title;
			Description = _description;
			isSelected = Visibility.Collapsed;
		}

		public void Unselect()
		{
			this.Color = UnselectColor;
			this.isSelected = Visibility.Collapsed;
		}

		public void Select()
		{
			this.Color = SelectColor;
			this.isSelected = Visibility.Visible;
		}
	}
}