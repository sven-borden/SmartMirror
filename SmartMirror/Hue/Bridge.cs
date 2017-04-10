using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace SmartMirror.Hue
{
	public class Bridge
	{
		/// <summary>
		/// Ip if the Bridge
		/// </summary>
		public string Ip { get; set; }

		/// <summary>
		/// User id to communicate
		/// </summary>
		public string UserId { get; set; }

		public string UrlBase => $"{Ip}/api/{UserId}/";

		/// <summary>
		/// Basic constructor
		/// </summary>
		public Bridge()	{}

		/// <summary>
		/// Constructor with known Ip address
		/// </summary>
		/// <param name="ip"></param>
		public Bridge(string ip)
		{
			Ip = ip;
		}

		/// <summary>
		/// Full Constructor with known Ip address and user ID
		/// </summary>
		/// <param name="ip"></param>
		/// <param name="userId"></param>
		public Bridge(string ip, string userId) : this(ip)
		{
			UserId = userId;
		}

		/// <summary>
		/// Find a bridge by Ip
		/// </summary>
		/// <returns></returns>
		public static async Task<Bridge> FindBridgeAsync()
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					string response = await client.GetStringAsync(new Uri("https://www.meethue.com/api/nupnp"));
					if (response == "[]")
						return null;
					string ip = JArray.Parse(response).First["internalipaddress"].ToString();
					return new Bridge(ip);
				}
				catch(Exception)
				{
					return null;
				}
			}
		}

		/// <summary>
		/// Get all the lights known by the bridge
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<Light>> GetLightAsync()
		{
			List<Light> lights = new List<Light>();
			HttpResponseMessage response = await HttpGetAsync("lights");
			string json = await response.Content.ReadAsStringAsync();
			JObject jo = JObject.Parse(json);
			foreach(JProperty pro in jo.Properties())
			{
				Light light = JsonConvert.DeserializeObject<Light>(pro.Value.ToString());
				light.Id = pro.Name;
				light._bridge = this;
				light.State._light = light;
				if (light.State.Reachable)
					lights.Add(light);
			}
			return lights;
		}

		/// <summary>
		/// Gets a light with the give id knokwn to the bridge
		/// </summary>
		/// <param name="id"></param>
		/// <returns>light or null if unknown light</returns>
		public async Task<Light> GetLightAsync(string id)
		{
			HttpResponseMessage response = await HttpGetAsync($"lights/{id}");
			Light light = JsonConvert.DeserializeObject<Light>(await response.Content.ReadAsStringAsync());
			if(light != null)
			{
				light.Id = id;
				light._bridge = this;
				light.State._light = light;
			}
			return light;
		}

		/// <summary>
		/// Request the bridge to search new lights
		/// </summary>
		/// <returns></returns>
		public async Task FindNewLightsAsync() => await HttpPutAsync("lights", String.Empty);

		/// <summary>
		/// Register the app with the bridge 
		/// </summary>
		/// <returns></returns>
		public async Task<bool> RegisterAsync()
		{
			try
			{
				using (Windows.Web.Http.HttpClient client = new Windows.Web.Http.HttpClient())
				{
					string id = new Random().Next().ToString();
					var response = await client.PostAsync(new Uri($"http://{Ip}/api"), new HttpStringContent($"{{\"devicetype\":\"HueLightController#{id}\"}}"));
					string content = await response.Content.ReadAsStringAsync();
					JArray json = JArray.Parse(content);
					if(json.First["success"] != null)
					{
						UserId = json.First["success"]["username"].ToString();
						return true;
					}
				}
			}
			catch(Exception)
			{
				return false;
			}
			return false;
		}

		/// <summary>
		/// Send a basic command to the bridge and returns whether it receives the expected response
		/// </summary>
		/// <returns></returns>
		public async Task<BridgeConnectionStatus> PingAsync()
		{
			try
			{
				HttpResponseMessage response = await HttpGetAsync("config");
				string content = await response.Content.ReadAsStringAsync();
				if (content.Contains("zigbeechannel"))
					return BridgeConnectionStatus.Success;
				else
					return BridgeConnectionStatus.Fail;
			}
			catch(Exception)
			{
				return BridgeConnectionStatus.Fail;
			}
		}

		/// <summary>
		/// Sends a Get command via HTTP and returns the response
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		internal async Task<HttpResponseMessage> HttpGetAsync(string command)
		{
			using (HttpClient client = new HttpClient())
			{
				return await client.GetAsync(new Uri($"http://{UrlBase}{command}"), HttpCompletionOption.ResponseContentRead);
			}
		}

		/// <summary>
		/// Send a PUT command via HTTP and returns the response.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="body"></param>
		/// <returns></returns>
		internal async Task<string> HttpPutAsync(string command, string body)
		{
			using (HttpClient client = new HttpClient())
			{
				Uri uri = new Uri($"http://{UrlBase}{command}");
				HttpResponseMessage response = await client.PutAsync(uri, new HttpStringContent(body));
				return await response.Content.ReadAsStringAsync();
			}
		}

		public enum BridgeConnectionStatus
		{
			Success,
			Unauthorized,
			Fail
		}
	}
}
