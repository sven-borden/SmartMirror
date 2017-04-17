using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror.CFF
{
	[DataContract]
	public class Location
	{
		[DataMember(Name = "stations")]
		public List<Station> Stations { get; set; }
	}

	[DataContract]
	public class Station
	{
		[DataMember(Name = "id")]
		public string Id { get; set; }

		[DataMember(Name = "name")]
		public string Name { get; set; }

		[DataMember(Name = "score")]
		public string Score { get; set; }

		[DataMember(Name ="coordinate")]
		public Coordinate Coordinate { get; set; }
	}

	[DataContract]
	public class Coordinate
	{
		[DataMember(Name = "type")]
		public string Type { get; set; }

		[DataMember(Name = "x")]
		public double X { get; set; }

		[DataMember(Name = "y")]
		public double Y { get; set; }
	}

	[DataContract]
	public class From
	{
		[DataMember(Name = "arrival")]
		public string Arrival { get; set; }

		[DataMember(Name = "arrivalTimestamp")]
		public string ArrivalTimeStamp { get; set; }

		[DataMember(Name = "departure")]
		public string Departure { get; set; }

		[DataMember(Name = "departureTimestamp")]
		public string DepartureTimeStamp { get; set; }

		[DataMember(Name = "platform")]
		public string Platform { get; set; }

		[DataMember(Name = "prognosis")]
		public Prognosis Prognosis { get; set; }

		[DataMember(Name = "station")]
		public Station Station { get; set; }
	}

	[DataContract]
	public class To
	{
		[DataMember(Name = "arrival")]
		public string Arrival { get; set; }

		[DataMember(Name = "arrivalTimestamp")]
		public string ArrivalTimeStamp { get; set; }

		[DataMember(Name = "departure")]
		public string Departure { get; set; }

		[DataMember(Name = "departureTimestamp")]
		public string DepartureTimeStamp { get; set; }

		[DataMember(Name = "platform")]
		public string Platform { get; set; }

		[DataMember(Name = "prognosis")]
		public Prognosis Prognosis { get; set; }

		[DataMember(Name = "station")]
		public Station Station { get; set; }
	}

	[DataContract]
	public class Connections
	{
		[DataMember(Name = "connections")]
		public ObservableCollection<Connection> Connection { get; set; }
	}

	[DataContract]
	public class Connection
	{
		[DataMember(Name = "from")]
		public From From { get; set; }

		[DataMember(Name = "to")]
		public To To { get; set; }
	}

	[DataContract]
	public class Service
	{
		[DataMember(Name = "regular")]
		public string Regular { get; set; }

		[DataMember(Name = "irregular")]
		public string Irregular { get; set; }
	}

	[DataContract]
	public class Prognosis
	{
		[DataMember(Name = "platform")]
		public string Platform { get; set; }

		[DataMember(Name = "departure")]
		public string Departure { get; set; }

		[DataMember(Name = "arrival")]
		public string Arrival { get; set; }

		[DataMember(Name = "capacity1st")]
		public string Capacity1st { get; set; }

		[DataMember(Name = "capacity2nd")]
		public string Capacity2nd { get; set; }
	}

	[DataContract]
	public class StationBoard
	{
		[DataMember(Name = "station")]
		public Station Station { get; set; }

		[DataMember(Name = "stationboard")]
		public ObservableCollection<StationBoards> StationBoards { get; set; }
	}

	[DataContract]
	public class StationBoards
	{
		[DataMember(Name = "stop")]
		public Stop Stop { get; set; }

		[DataMember(Name = "name")]
		public string Name { get; set; }

		[DataMember(Name = "category")]
		public string Category { get; set; }

		[DataMember(Name = "number")]
		public string Number { get; set; }

		[DataMember(Name = "operator")]
		public string Operator { get; set; }

		[DataMember(Name = "to")]
		public string To { get; set; }
	}

	[DataContract]
	public class Stop
	{
		[DataMember(Name = "station")]
		public Station Station { get; set; }

		[DataMember(Name = "arrival")]
		public string Arrival { get; set; }

		[DataMember(Name = "arrivalTimestamp")]
		public string ArrivalTimestamp { get; set; }

		[DataMember(Name = "departure")]
		public string Departure { get; set; }

		[DataMember(Name = "departureTimestamp")]
		public string DepartureTimestamp { get; set; }

		[DataMember(Name = "platform")]
		public string Platform { get; set; }

		[DataMember(Name = "prognosis")]
		public Prognosis Prognosis { get; set; }
	}

	[DataContract]
	public class Section
	{
		[DataMember(Name = "journey")]
		public Journey Journey { get; set; }

		[DataMember(Name = "walk")]
		public string Walk { get; set; }

		[DataMember(Name = "departure")]
		public string Departure { get; set; }

		[DataMember(Name = "arrival")]
		public string Arrival { get; set; }
	}

	[DataContract]
	public class Journey
	{
		[DataMember(Name = "name")]
		public string Name { get; set; }

		[DataMember(Name = "category")]
		public string Category { get; set; }

		[DataMember(Name = "categoryCode")]
		public string CategoryCode { get; set; }

		[DataMember(Name = "number")]
		public string Number { get; set; }

		[DataMember(Name = "operator")]
		public string Operator { get; set; }

		[DataMember(Name = "to")]
		public string To { get; set; }

		[DataMember(Name = "passList")]
		public string PassList { get; set; }

		[DataMember(Name = "capacity1st")]
		public string Capacity1st { get; set; }

		[DataMember(Name = "capacity2nd")]
		public string Capacity2nd { get; set; }
	}
}
