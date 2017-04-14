
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartMirror.CFF;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {

		[TestMethod]
		public async Task TestStationCFFAsync()
		{
			string city = "basel";
			Station expectedStation = new Station()
			{
				Id = "000000022",
				Name = "Basel",
				Score = "null",
				Coordinate = new Coordinate()
				{
					Type = "WGS84",
					X = 47.547408,
					Y = 7.589547
				}
			};

			Handler CFF = new Handler();
			Station toComp = await CFF.GetStation(city);
			Assert.AreEqual(expectedStation.Id, toComp.Id, null, "Station is not get correctly");
		}
	}
}
