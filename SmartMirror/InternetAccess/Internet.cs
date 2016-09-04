using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace SmartMirror.InternetAccess
{
	class Internet
	{
		/// <summary>
		/// Check if there is an active internet connection
		/// </summary>
		/// <param name="DNS">Using special IP address, otherwise using 8.8.8.8</param>
		/// <returns>If device is connected</returns>
		public static bool isConnected(string DNS = "8.8.8.8")
		{
			ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
			bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
			return internet;
		}
	}
}
