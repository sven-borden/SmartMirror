namespace SmartMirror.Class.News
{
	class NewsData
	{
		/// <summary>
		/// News Title
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// News Description
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// News AAuthor
		/// </summary>
		public string Author { get; set; }

		/// <summary>
		/// News link
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// Image url of news
		/// </summary>
		public string ImageUrl { get; set; }

		/// <summary>
		/// Date of publication
		/// </summary>
		public string Date { get; set; }

		public NewsData()
		{
		}
	}
}
