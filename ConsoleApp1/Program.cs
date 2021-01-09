using System;
using BLApi;
using BO;
using APIDL;
using DS;
using System.Linq;



namespace PlConsole
{
    class Program
    {
        static IBL bl;
        static IDAL dl;
        static void Main(string[] args)
        {
            bl = BlFactory.GetBL();
            dl = DalFactory.GetDal();

            foreach (DO.BusStation item in dl.GetAllStationsBy(s=>s.BusStationKey == 38831))
                Console.WriteLine(item);
            var request = from stat in DataSource.ListLineStations 
                          where predicate(stat)
                          select stat;
            bool predicate (DO.LineStation stat)
            {
                return stat.StationKey == 38831;
            }
            foreach (DO.LineStation item in request)
                Console.WriteLine(item);
            foreach(DO.LineStation item in dl.GetAllLineStationsBy(s => s.StationKey == 38831))
                Console.WriteLine(item);

            BusStation stationTest = bl.GetBusStation(38831);
            Console.WriteLine(stationTest);
            Console.ReadKey();
            foreach (BusStation item in bl.GetAllBusStations())
                Console.WriteLine(item);
            bl.AddStation(new BusStation { Address = "testAdresse", BusStationKey = 12345, StationName = "testNom", });
            foreach (BusStation item in bl.GetAllBusStations())
                Console.WriteLine(item);
            Console.ReadKey();
        }
    }
}