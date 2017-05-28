using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror.Network
{
	public static class Internet
	{
		public static bool IsConnected()
		{
			return NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable;
		}

		public static async Task<bool> WaitForConnection()
		{
			while(!NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
				await Task.Delay(3000);
			return true;
		}
	}
}
