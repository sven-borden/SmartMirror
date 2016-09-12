using Newtonsoft.Json.Linq;
using SmartMirror.Class.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror
{
	class News
	{
		static private string API = "af9149f9279646a2bae7a80d09a2d075";
		static private string link = "https://newsapi.org/v1/articles?";
		static private string source = "the-verge";
		static private string type = "latest";
		public static string Error = "";

		public static async Task<List<NewsData>> GetNews()
		{
			JObject o = await GetJson(BuildUri());
			return ParseList(o);
		}

		private static List<NewsData> ParseList(JObject o)
		{
			if (o["status"].ToString() != "ok")
				return null;
			List<NewsData> list = new List<NewsData>();
			foreach(var item in o["articles"])
			{
				try
				{
					list.Add
					(
						new NewsData()
						{
							Author = item["author"].ToString(),
							Title = item["title"].ToString(),
							Description = item["description"].ToString(),
							Url = item["url"].ToString(),
							ImageUrl = item["urlToImage"].ToString(),
							Date = item["publishedAt"].ToString()
						}
					);
				}
				catch(Exception e)
				{
					Error.ToString();
				}
			}

			return list;
		}

		private static string BuildUri()
		{
			return link + "source=" + source + "&apiKey=" + API + "&sortBy=" + type;
		}

		static async Task<JObject> GetJson(string URI)
		{
			HttpClient client = new HttpClient();
			var content = await client.GetStringAsync(new Uri(URI));
			return await Task.Run(() => JObject.Parse(content));
		}
	}
}
