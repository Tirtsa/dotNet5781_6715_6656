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
        public static List<RunningNumber> idNumber;
		public static List<BusStation> Stations;
		public static List<BusLine> Buses;
        static DataSource()
        {
            idNumber = new List<RunningNumber>();
            idNumber.Add(new RunningNumber());

			while (Stations.Count() != 50)           //randomly initializing bus list of 21 buses
			{
				BusStation station = new BusStation();

			}

			while (Buses.Count() != 25)           //randomly initializing bus list of 21 buses
			{
				BusLine bus = new BusLine();
				int rand = new Random().Next(1000000, 99999999);
				
				if (!Buses.Contains(rand))
					Buses.Add(rand);
				Thread.Sleep(100);
				//add at least 10 bus stops for 10 buses
			}

		}
	}
}