using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DO;
using APIDL;


namespace DS
{
    public static class DataSource
    {
        
		public static List<BusStation> ListStations;
		public static List<BusLine> ListLines;
		public static List<LineStation> ListLineStations;
		public static List<FollowingStations> ListFollowingStations;
		public static int LineId = 0;
		public static int LineStationId = 0;
		public static int FollowingStationsId = 0;

		static DataSource()
		{
			InitAllLists();
		}

		static void InitAllLists()
		{

			//while (Stations.Count() != 50)           //randomly initializing bus list of 21 buses
			//{
			//	BusStation station = new BusStation();
			//}

			//while (Buses.Count() != 25)           //randomly initializing bus list of 21 buses
			//{
			//	BusLine bus = new BusLine();
			//	int rand = new Random().Next(1000000, 99999999);

			//	Thread.Sleep(100);
			//	//add at least 10 bus stops for 10 buses
			//}

			ListStations = new List<BusStation>
			{
				new BusStation
				{
					BusStationKey = 38831, 
					StationName =" בי''ס בר לב / בן יהודה", 
					Address =" רחוב:בן יהודה 76 עיר: כפר סבא רציף: קומה", 
					Latitude = 32.183921,    
					Longitude = 34.917806,
				},
				new BusStation
				{
					BusStationKey = 38894,
					StationName ="פיינברג/שכביץ",
					Address =" רחוב:פיינברג 4 עיר: גדרה רציף:   קומה",
					Latitude = 31.813285,
					Longitude = 34.775928,
				},
				new BusStation
				{
					BusStationKey = 38903,
					StationName ="קרוננברג/ארגמן",
					Address ="רחוב:יוסף קרוננברג  עיר: רחובות רציף:   קומה",
					Latitude = 31.878667,
					Longitude = 34.81138,
				},
				new BusStation
				{
					BusStationKey = 38912,
					StationName ="השומר/האבות",
					Address ="רחוב:השומר 22 עיר: ראשון לציון רציף:   קומה",
					Latitude = 31.959821,
					Longitude = 34.814747,
				},
				new BusStation
				{
					BusStationKey = 38916,
					StationName ="יוסף בורג/משואות יצחק",
					Address ="רחוב:ד''ר יוסף בורג 9 עיר: ראשון לציון רציף:   קומה",
					Latitude = 31.968049,
					Longitude = 34.818099,
				},
				new BusStation
				{
					BusStationKey = 38922,
					StationName ="השר חיים שפירא/הרב שלום ג'רופי",
					Address ="רחוב:השר חיים משה שפירא 16 עיר: ראשון לציון רציף:   קומה",
					Latitude = 31.990757,
					Longitude = 34.755683,
				},
				new BusStation
				{
					BusStationKey = 39001,
					StationName ="שדרות יעקב/יוסף הנשיא",
					Address ="רחוב:שדרות יעקב 65 עיר: ראשון לציון רציף:   קומה",
					Latitude = 31.950254,
					Longitude = 34.819244 ,
				},
				new BusStation
				{
					BusStationKey = 39002,
					StationName ="שדרות יעקב/עזרא",
					Address ="רחוב:שדרות יעקב 59 עיר: ראשון לציון רציף:   קומה",
					Latitude = 31.95111,
					Longitude = 34.819766 ,
				},
				new BusStation
				{
					BusStationKey = 39004,
					StationName ="לייב יוספזון/יעקב ברמן",
					Address ="רחוב:יהודה לייב יוספזון  עיר: רחובות רציף:   קומה",
					Latitude = 31.905052,
					Longitude = 34.818909,
				},
				new BusStation
				{
					BusStationKey = 39005,
					StationName ="הרב יעקב ברמן/הרב יהודה צבי מלצר",
					Address =" רחוב:הרב יעקב ברמן 4 עיר: רחובות רציף:   קומה",
					Latitude = 31.901879,
					Longitude = 34.819443,
				},
				new BusStation
				{
					BusStationKey = 39006,
					StationName ="ברמן/מלצר",
					Address ="רחוב:הרב יעקב ברמן  עיר: רחובות רציף:   קומה",
					Latitude = 31.90281,
					Longitude = 34.818922,
				},
				new BusStation
				{
					BusStationKey = 39007,
					StationName ="הנשיא הראשון/מכון ויצמן",
					Address ="רחוב:הנשיא הראשון 55 עיר: רחובות רציף:   קומה",
					Latitude = 31.904567,
					Longitude = 34.815296,
				},
				new BusStation
				{
					BusStationKey = 39008,
					StationName ="הנשיא הראשון/קיפניס",
					Address ="רחוב:הנשיא הראשון 56 עיר: רחובות רציף:   קומה",
					Latitude = 31.904755,
					Longitude = 34.816661,
				},
				new BusStation
				{
					BusStationKey = 39012,
					StationName ="הירדן/הערבה",
					Address ="רחוב:הירדן 23 עיר: באר יעקב רציף:   קומה",
					Latitude = 31.937387,
					Longitude = 34.838609,
				},
				new BusStation
				{
					BusStationKey = 39013,
					StationName ="הירדן/חרוד",
					Address ="רחוב:הירדן 22 עיר: באר יעקב רציף:   קומה",
					Latitude = 31.936925,
					Longitude = 34.838341,
				},
				new BusStation
				{
					BusStationKey = 39014,
					StationName ="האלונים/הדקל",
					Address ="רחוב:שדרות האלונים  עיר: באר יעקב רציף:   קומה",
					Latitude = 31.939037,
					Longitude = 34.831964,
				},
				new BusStation
				{
					BusStationKey = 39017,
					StationName ="האלונים א/הדקל",
					Address ="רחוב:שדרות האלונים  עיר: באר יעקב רציף:   קומה",
					Latitude = 31.939656,
					Longitude = 34.832104,
				},
				new BusStation
				{
					BusStationKey = 39018,
					StationName ="פארק תעשיות שילת",
					Address ="רחוב:דרך הזית  עיר: שילת רציף:   קומה",
					Latitude = 31.914324,
					Longitude = 35.023589,
				},
				new BusStation
				{
					BusStationKey = 39019,
					StationName ="פארק תעשיות שילת",
					Address ="רחוב:דרך הזית  עיר: שילת רציף:   קומה",
					Latitude = 31.914816,
					Longitude = 35.023028,

				},
				new BusStation
				{
					BusStationKey = 39024,
					StationName ="עיריית מודיעין מכבים רעות",
					Address ="רחוב:  עיר: מודיעין מכבים רעות רציף:   קומה",
					Latitude = 31.908499,
					Longitude = 35.007955,
				},
				new BusStation
				{
					BusStationKey = 39028,
					StationName ="חיים ברלב/מרדכי מקלף",
					Address ="רחוב:חיים ברלב 30 עיר: מודיעין מכבים רעות רציף:   קומה",
					Latitude = 31.907828,
					Longitude = 35.000614,
				}
			};

			ListLines = new List<BusLine>
			{
				new BusLine
				{
					Id = LineId++,
					BusLineNumber = 12,
					Area = Areas.Center,
					FirstStationKey = 38831,
					LastStationKey = 39007,
					TotalTime = 20
				},
				new BusLine
				{
					Id = LineId++,
					BusLineNumber = 30,
					Area = Areas.Jerusalem,
					FirstStationKey = 38894,
					LastStationKey = 39024,
					TotalTime = 15
				},
				new BusLine
				{
					Id = LineId++,
					BusLineNumber = 50,
					Area = Areas.North,
					FirstStationKey = 38903,
					LastStationKey = 39024,
					TotalTime = 30
				},
				new BusLine
				{
					Id = LineId++,
					BusLineNumber = 113,
					Area = Areas.Center,
					FirstStationKey = 38831,
					LastStationKey = 39019,
					TotalTime = 40
				}
			};

			ListLineStations = new List<LineStation>
			{
				new LineStation
				{
					Id = LineStationId++,
					LineId = 1,
					StationKey = 38831,
					RankInLine = 1
                }, 
				new LineStation
				{
					Id = LineStationId++,
					LineId = 1,
					StationKey = 38894,
					RankInLine = 2
				}, 
				new LineStation
				{
					Id = LineStationId++,
					LineId = 1,
					StationKey = 39002,
					RankInLine = 3
				}, 
				new LineStation
				{
					Id = LineStationId++,
					LineId = 1,
					StationKey = 39006,
					RankInLine = 4
				}, 
				new LineStation
				{
					Id = LineStationId++,
					LineId = 1,
					StationKey = 39007,
					RankInLine = 5
				},

				new LineStation
				{
					Id = LineStationId++,
					LineId = 2,
					StationKey = 38894,
					RankInLine = 1
				},
				new LineStation
				{
					Id = LineStationId++,
					LineId = 2,
					StationKey = 39002,
					RankInLine = 2
				},
				new LineStation
				{
					Id = LineStationId++,
					LineId = 2,
					StationKey = 39006,
					RankInLine = 3
				},
				new LineStation
				{
					Id = LineStationId++,
					LineId = 2,
					StationKey = 39024,
					RankInLine = 4
				},

				new LineStation
				{
					Id = LineStationId++,
					LineId = 3,
					StationKey = 38903,
					RankInLine = 1
				},
				new LineStation
				{
					Id = LineStationId++,
					LineId = 3,
					StationKey = 39002,
					RankInLine = 2
				},
				new LineStation
				{
					Id = LineStationId++,
					LineId = 3,
					StationKey = 39024,
					RankInLine = 3
				},

				new LineStation
				{
					Id = LineStationId++,
					LineId = 4,
					StationKey = 38831,
					RankInLine = 1
				},
				new LineStation
				{
					Id = LineStationId++,
					LineId = 4,
					StationKey = 39004,
					RankInLine = 2
				},
				new LineStation
				{
					Id = LineStationId++,
					LineId = 4,
					StationKey = 39019,
					RankInLine = 4
				},
			};

			ListFollowingStations = new List<FollowingStations>
			{
				new FollowingStations
                {
					Id = FollowingStationsId++,
					KeyStation1 = 38831,
					KeyStation2 = 38894,
                }
			};
		} 
	}
}