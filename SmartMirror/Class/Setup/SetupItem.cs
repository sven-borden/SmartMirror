using SmartMirror.Class.Setup;
using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace SmartMirror.Pages
{
	class SetupItem
	{
		/// <summary>
		/// Color of the item
		/// </summary>
		public SolidColorBrush Color { get; set; }

		/// <summary>
		/// Short title
		/// </summary>
		public string Title { get; }

		/// <summary>
		/// Full title
		/// </summary>
		public string LongTitle { get; }

		/// <summary>
		/// Short Description
		/// </summary>
		public string Description { get; }

		/// <summary>
		/// Long Description
		/// </summary>
		public string LongDescription { get; }

		public List<SetupQuestion> Questions { get; }

		/// <summary>
		/// Selected color
		/// </summary>
		public SolidColorBrush SelectColor { get; }

		/// <summary>
		/// Unselected color
		/// </summary>
		public SolidColorBrush UnselectColor { get; }

		/// <summary>
		/// Collapsed of Visible
		/// </summary>
		public Visibility isSelected { get; set; }

		/// <summary>
		/// Register a basic setup item
		/// </summary>
		/// <param name="_title">Small title for progressbar</param>
		/// <param name="_description">Short Description for progressbar</param>
		/// <param name="_longTitle">Full title</param>
		/// <param name="_longDescription">Full Description</param>
		public SetupItem(string _title, string _description, string _longTitle, string _longDescription)
		{
			SelectColor = new SolidColorBrush(Colors.White);
			UnselectColor = new SolidColorBrush(Colors.LightGray);
			Color = UnselectColor;
			Title = _title;
			Description = _description;
			LongDescription = _longDescription;
			LongTitle = _longTitle;
			isSelected = Visibility.Collapsed;
		}

		/// <summary>
		/// Register a full setup item
		/// </summary>
		/// <param name="_title">Short title</param>
		/// <param name="_description">Short description</param>
		/// <param name="_longTitle">Full title</param>
		/// <param name="_longDescription">Full description</param>
		/// <param name="_text">Triple dimension array, first line represent text of setting to ask, second line represent a suggestion regarding the setting and third the setting name</param>
		/// <param name="_settingsName">Relative setting name to register</param>
		public SetupItem(string _t, string _d, string _lt, string _ld, List<SetupQuestion> _list) : this(_t, _d, _lt, _ld)
		{
			Questions = _list;
		}

		/// <summary>
		/// Unselect this item
		/// </summary>
		public void Unselect()
		{
			this.Color = UnselectColor;
			this.isSelected = Visibility.Collapsed;
		}

		/// <summary>
		/// Select this item
		/// </summary>
		public void Select()
		{
			this.Color = SelectColor;
			this.isSelected = Visibility.Visible;
		}
	}
}