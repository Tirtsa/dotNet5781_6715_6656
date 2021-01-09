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
            
            foreach(DO.LineStation item in dl.GetAllLineStationsBy(s => s.StationKey == 38831))
                Console.WriteLine(item);

            BusStation stationTest = bl.GetBusStation(38831);
            Console.WriteLine(stationTest);
            Console.ReadKey();
            foreach (BusStation item in bl.GetAllBusStations())
                Console.WriteLine(item);
            try
            {
                bl.AddStation(new BusStation { Address = "testAdresse", BusStationKey = 12345, StationName = "testNom", });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            foreach (BusStation item in bl.GetAllBusStations())
                Console.WriteLine(item);
            //bl.DeleteStation(12345);
            //bl.UpdateBusStation(new BusStation { Address = "MonNewtestAdresse", BusStationKey = 12345, StationName = "2etestNom" });
            //bl.UpdateBusStation(12345, testVoid); Need to define function's body
            //void testVoid (BusStation myStation)
            //{
            //    myStation.BusStationKey = 45678;
            //}

            Console.ReadKey();
        }
    }
}