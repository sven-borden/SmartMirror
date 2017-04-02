using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace SmartMirror.WeatherAPI
{
	public class LocationManager
    {
		public async static Task<Geoposition> GetPosition()
		{
			var accessStatus = await Geolocator.RequestAccessAsync();

			if (accessStatus != GeolocationAccessStatus.Allowed) throw new Exception();
			var geolocator = new Geolocator { DesiredAccuracyInMeters = 0 };

			var position = await geolocator.GetGeopositionAsync();
			return position;
		}
	}
}
