using HueLibrary;
using SmartMirror.Content;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace SmartMirror.Hue
{
	public class HueHandler
	{
		Bridge bridge = null;
		IEnumerable<Light> lights = null;
		Message Message;

		public HueHandler(Message _m)
		{
			Message = _m;
			Message.ShowMessage("Start to setup Light");
			Setup();
		}

		public void TurnOnLights()
		{
			foreach (Light l in lights)
				l.State.On = true;
			Message.ShowMessage("Turned lights On");
		}

		public void TurnOffLights()
		{
			foreach (Light l in lights)
				l.State.On = false;
			Message.ShowMessage("Turned lights Off");
		}

		private async void Setup()
		{
			if (!await FindBridgeAsync())
			{
				Message.ShowMessage("Cannot find Hue");
				return;
			}
			await FindLightsAsync();
			SaveBridgeToCache();
			Message.ShowMessage("Connected to Hue");
		}

		private async Task FindLightsAsync()
		{
			try
			{
				lights = new ObservableCollection<Light>(await bridge.GetLightsAsync());
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

		private async Task<bool> FindBridgeAsync()
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
						return true;
				}
				// Second attempt: Hue N-UPnP service.
				bridge = await Bridge.FindAsync();
				if (await PrepareBridgeAsync())
					return true;
			}
			catch (Exception e)
			{
				Debug.WriteLine("We encountered an unexpected problem trying to find your bridge: " + e);
			}
			return false;
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
