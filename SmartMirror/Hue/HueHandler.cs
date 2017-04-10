using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using static SmartMirror.Hue.Bridge;

namespace SmartMirror.Hue
{
	public class HueHandler
	{
		Bridge bridge = null;
		IEnumerable<Light> lights = null;
		public HueHandler()
		{
			Setup();
		}

		private async void Setup()
		{
			await FindBridgeAsync();
			await FindLightsAsync();
			SaveBridgeToCache();
		}

		private async Task FindLightsAsync()
		{
			try
			{
				lights = new ObservableCollection<Light>(await bridge.GetLightAsync());
				if (!lights.Any())
				{
					Debug.WriteLine("We couldn't find any lights. Make sure they're in range and connected to a power source.");
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine("We encountered an unexpected problem trying to find your lights: " + e);
			}
		}

		private void SaveBridgeToCache()
		{
			var localStorage = ApplicationData.Current.LocalSettings.Values;
			localStorage["bridgeIp"] = bridge.Ip;
			localStorage["userId"] = bridge.UserId;

		}

		private async Task FindBridgeAsync()
		{
			try
			{
				// First attempt: local storage cache.
				var localStorage = ApplicationData.Current.LocalSettings.Values;
				if (localStorage.ContainsKey("bridgeIp") && localStorage.ContainsKey("userId"))
				{
					bridge = new Bridge(
						localStorage["bridgeIp"].ToString(),
						localStorage["userId"].ToString());
					if (await PrepareBridgeAsync())
						return;
				}
				// Second attempt: Hue N-UPnP service.
				bridge = await Bridge.FindBridgeAsync();
				if (await PrepareBridgeAsync())
					return;
			}
			catch (Exception e)
			{
				Debug.WriteLine("We encountered an unexpected problem trying to find your bridge: " + e);
			}
		}

		private async Task<bool> PrepareBridgeAsync(int attempts = 0)
		{
			if (bridge == null || attempts > 2)
			{
				return false;
			}
			switch (await bridge.PingAsync())
			{
				case BridgeConnectionStatus.Success:
					return true;
				case BridgeConnectionStatus.Fail:
					return false;
				case BridgeConnectionStatus.Unauthorized:
					await bridge.RegisterAsync();
					return await PrepareBridgeAsync(++attempts);
				default:
					throw new InvalidOperationException();
			}
		}
	}
}
