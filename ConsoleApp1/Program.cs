using System;
using BLApi;
using BO;
using APIDL;
using DS;
using System.Linq;
using System.Collections.Generic;



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
            bl.DeleteStation(12345);
            foreach (BusStation item in bl.GetAllBusStations())
                Console.WriteLine(item);
            //bl.UpdateBusStation(new BusStation { Address = "MonNewtestAdresse", BusStationKey = 12345, StationName = "2etestNom" });
            //bl.UpdateBusStation(12345, testVoid); Need to define function's body
            //void testVoid (BusStation myStation)
            //{
            //    myStation.BusStationKey = 45678;
            //}
            Console.WriteLine(bl.GetBusLine(30, DO.Areas.Jerusalem));
            List<int> test = (from item in dl.GetAllStationsBy(s=>s.BusStationKey < 39020) 
                                           select bl.BusStationDoBoAdapter(item).BusStationKey).ToList();
            BO.BusLine lineToTest = new BusLine
            {
                Area = DO.Areas.Center,
                BusLineNumber = 52,
                FirstStationKey = 38831,
                LastStationKey = 39093,
                AllStationsOfLine = test
            };
            bl.AddBusLine(lineToTest) ;
            foreach(BusLine item in bl.GetAllBusLines())
                Console.WriteLine(item);
            bl.DeleteBusLine(lineToTest);
            foreach (BusLine item in bl.GetAllBusLines())
                Console.WriteLine(item);
            IEnumerable<LineStation> lineStatTest = bl.GetAllLineStationsBy(s => s.LineId == 52);
            foreach (LineStation item in bl.GetAllLineStationsBy(s => s.LineId == 52))
                Console.WriteLine(item); 
            if (lineStatTest == null) //line stations doesn't exist but lineStatTest != null and not write "bravo"...!?
                Console.WriteLine("bravo");
            foreach(BusLine item in bl.GetAllBusLinesBy(l => l.BusLineNumber % 2 == 0))
                Console.WriteLine(item);
            Console.ReadKey();
        }
    }
}