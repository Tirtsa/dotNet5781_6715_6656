using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using System.Collections;

namespace dotNet5781_02_6715_5227
{


    /*************************************
     * Class with list of all Bus Stations
     * 
     *************************************/
    public class ListBusStation
    {
        /// <summary>
        /// creat list which contains all bus station.
        /// </summary>

        public static List<BusStation> ListOfAllStations = new List<BusStation>();
        
        public void addStation(BusStation myStation)
        {
            ListOfAllStations.Add(new BusStation(myStation));
        }

        public void deleteStation(int stationBusToDelete)
        {
            foreach(BusStation item in ListOfAllStations)
            {
                if (item.BusStationKey == stationBusToDelete)
                {
                    ListOfAllStations.Remove(item);
                    Console.WriteLine("Bus station removed with success");
                }
            }
        }

        public BusStation SearchStation(int stationKey)
        {
             foreach(BusStation item in ListOfAllStations)
             {
                if (item.BusStationKey == stationKey)
                {
                    return item;
                }
             }
            throw new ArgumentException($"Station {stationKey} doesn't exist");
        }
    }


    /*******************************************************
     * Class BusStation - all details concern bus station
     * 
     *******************************************************/
    public class BusStation : ListBusStation
    {
        //random numbers for latitude and longitude
        static Random  rLatitude = new Random(DateTime.Now.Millisecond);
        static Random rLongitude = new Random(DateTime.Now.Millisecond);

        public static int codeStation = 1;
        
        //fields of bus station
        public int BusStationKey {get; set;}
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string StationAdress { get; set; }


        public BusStation() //constuctor with no parameter
        {
            BusStationKey = codeStation ++;
            Latitude = rLatitude.NextDouble() * (33.3 - 31) + 31;
            Longitude = rLongitude.NextDouble() *(34.3 - 35.5)+ 34.3;
            StationAdress = " ";
            addStation(this); //add the bus station to the list
        }

        
        public BusStation(string name) //constructor with parameter : adress of station
        {
            BusStationKey = codeStation++;
            Latitude = rLatitude.NextDouble() + 31;
            Longitude = rLongitude.NextDouble() + 34;
            StationAdress = name;
            addStation(this);//add the bus station to the list
        }

        public BusStation(BusStation myStation)//copy constructor
        {
            BusStationKey = myStation.BusStationKey;
            Latitude = myStation.Latitude;
            Longitude = myStation.Longitude;
            StationAdress = myStation.StationAdress;
        }

        public BusStation (int numStation)
        {
            BusStationKey = numStation;
            Latitude = rLatitude.NextDouble() + 31;
            Longitude = rLongitude.NextDouble() + 34;
            StationAdress = "";
            addStation(this);
        }
        
        //override of ToString method
        public override string ToString()
        {
            return "Bus Station Code:" + BusStationKey + ", " + Latitude + "°N " + Longitude + "°E " + StationAdress;
        }
    }

    /**************************************************
     * Class LineBus - a bbus line with many stations
     * 
     **************************************************/
    public class BusLine : ListBusStation, IComparable
    {
        /**************************************************
        * Class BusLineStation - an interior class for station of a line bus route
        * 
        **************************************************/
        public class BusLineStation 
        {
            public int BusStationKey { get; set; }
            public double Distance { get; set; }
            public double TravelTime { get; set; }
            public BusLineStation(int codeStation, BusLineStation prevStation)
            {
                BusStationKey = codeStation;
                if (prevStation == null)
                    prevStation = this;
                Distance = DistanceFromPrevStation(ListOfAllStations[(this.BusStationKey) - 1],
                    ListOfAllStations[(prevStation.BusStationKey) - 1]);
                TravelTime = Distance * 0.02;
            }

            //functions
            public double DistanceFromPrevStation(BusStation station1, BusStation station2)
            {
                var eCord = new GeoCoordinate(station1.Latitude, station1.Longitude);
                var sCord = new GeoCoordinate(station2.Latitude, station2.Longitude);
                return eCord.GetDistanceTo(sCord);
            }
        }

        //constructors
        public BusLine() { }
        public BusLine(int lineNumber, List<int> listStations) 
        { 
            BusLineNumber = lineNumber;
            foreach (int item in listStations)
                AddStation(3, item);
            TotalTime = 0;
            foreach (BusLineStation item in Stations)
                TotalTime += item.TravelTime;
        }
        public BusLine(BusLine myLine)
        {
            BusLineNumber = myLine.BusLineNumber;
            for (int i = 0; i < myLine.Stations.Count(); i++)
                AddStation(3, myLine.Stations[i].BusStationKey);
            TotalTime = 0;
            foreach (BusLineStation item in Stations)
                TotalTime += item.TravelTime;
        }


        //static & enum fields
        enum areas {General, North, South, Center, Jerusalem};

        //properties
        public int BusLineNumber {get; set;}
        public BusLineStation FirstStation { get {return Stations[0];}}
        public BusLineStation LastStation 
        {
            get
            {
                return Stations[Stations.Count - 1];
            }
        }
        public double TotalTime { get; set; }
        areas Areas
        { get;set;
            //get
            //{
            //    return Areas;
            //}
            //set
            //{
            //    if (BusLineNumber < 30)
            //        Areas = 0;
            //    if (BusLineNumber >= 30 && BusLineNumber < 60)
            //        Areas = (areas)1;
            //    if (BusLineNumber >= 60 && BusLineNumber < 90)
            //        Areas = (areas)2;
            //    if (BusLineNumber >= 90 && BusLineNumber < 120)
            //        Areas = (areas)3;
            //    if (BusLineNumber >= 120 && BusLineNumber <= 150)
            //        Areas = (areas)4;
            //}
        }
        List<BusLineStation> Stations = new List<BusLineStation> ();
        BusLineStation this[int index]
        {
            get
            {
                return Stations[index];
            }
            set
            {
                Stations[index] = value;
            }
        }

        //functions

        /// <summary>
        /// search index of station and return station in previous index
        /// </summary>
        /// <returns>the previous station</returns>
        public BusLineStation previousStation(BusLineStation myStation)
        {
            if(searchStationIndex(myStation.BusStationKey)==0)
            {
                return myStation;
            }
            return Stations[searchStationIndex(myStation.BusStationKey)-1];
        }

        /// <summary>
        /// search index of a buslinestation in Stations' list with FindeIndex and bool function
        /// </summary>
        /// <param name="myStation">BusLineStation we want to find its index</param>
        /// <returns>int : index of the station in Stations' list</returns>
        public int searchStationIndex(int codeStation)
        {
          
            bool same(BusLineStation stat)
            {
                return stat.BusStationKey == codeStation;
            }
            return Stations.FindIndex(same);
        }

        public void PrintStations ()
        {
            foreach (BusLineStation item in Stations)
                Console.Write(item.BusStationKey + " ");
            Console.WriteLine();
        }

        //methods
        public override string ToString()
        {
            Console.Write("Bus line: " + BusLineNumber + "  First Station: " + FirstStation.BusStationKey + "   Last Station: " +
                LastStation.BusStationKey + "   Areas: " + Areas + "\nStations: ");
            PrintStations();
            return " ";
        }
        public void AddStation(int num, int codeStation)
        {
            switch (num)
	        {
                case 1:
                    Stations.Insert(0, new BusLineStation(codeStation, null));
                    break;
                case 2:
                    Console.WriteLine("Please enter previous station's code : ");
                    int prevcode = int.Parse(Console.ReadLine());
                    int index = searchStationIndex(prevcode);
                    if (index == -1)
                        Console.WriteLine("ERROR, this station not exist in this bus line");
                    else
                        Stations.Insert(index + 1, new BusLineStation(codeStation, Stations[index]));
                    break;
                case 3:
                    if (Stations.Count > 0)
                        Stations.Add(new BusLineStation(codeStation, Stations[Stations.Count - 1]));
                    else
                        Stations.Add(new BusLineStation(codeStation, null));
                    break;
		        default:
                    break;
	        }
        }

        public void DeleteStation(int codeStation)
        {
            int index = searchStationIndex(codeStation);
            if (index != -1)
            {
                BusLineStation stationToDelete = Stations[index];
                Stations.Remove(stationToDelete);
            }
            throw new ArgumentException($"Line bus doesn't contains station {codeStation}.");
        }

        public bool CheckBusStation(BusStation station1)
        {
            foreach(BusLineStation item in Stations)
                if (station1.BusStationKey == item.BusStationKey)
                    return true;
            return false;
        }

        public double getDistance(int stationKey1, int stationKey2)
        {
            var sCoord = new GeoCoordinate(ListOfAllStations[stationKey1].Latitude, ListOfAllStations[stationKey1].Longitude);
            var eCoord = new GeoCoordinate(ListOfAllStations[stationKey2].Latitude, ListOfAllStations[stationKey2].Longitude);

            return sCoord.GetDistanceTo(eCoord);
        }

        public double TravelTimeBetweenStations(int codeStation1, int codeStation2)
        {
            return 0.5 * getDistance(codeStation1, codeStation2);
        }

        public BusLine PartOfLine(int begin, int end)
        {
            List<int> PartialStations = new List<int>();
            for (int i = begin; i <= end; i++)
            {
                PartialStations.Add(this.Stations[i].BusStationKey);
            }

            return new BusLine(this.BusLineNumber, PartialStations);
        }

        public int CompareTo(object obj)
        {
            BusLine b = (BusLine)obj;
            return TravelTimeBetweenStations(FirstStation.BusStationKey, LastStation.BusStationKey).CompareTo
                (b.TravelTimeBetweenStations(b.FirstStation.BusStationKey, b.LastStation.BusStationKey));
        }
    }

    class BusesCollection : ListBusStation, IEnumerable
    {
        //fields ans properties
        List<BusLine> BusesLines = new List<BusLine>();


        public new IEnumerator GetEnumerator()
        {
            for (int i = 0; i < BusesLines.Count; i++)
                yield return BusesLines[i];
        }


        /// <summary>
        /// Function Add - to add a new bus line to collection
        /// </summary>
        /// <param name="line"> number of line we want to add</param>
        /// <param name="Stations">list of int - numbers of stations the bus line have</param>
        public void Add(int line, List<int> Stations)
        {
            foreach (BusLine item in BusesLines)
            {
                if (item.BusLineNumber == line)
                    throw new ArgumentException(String.Format($"Bus line's number must be unique, line {line} is " +
                        "already exists."));
            }
            
            
            foreach (int item in Stations)
            {
                bool exist = false;
                foreach (BusStation item2 in ListOfAllStations)
                    if (exist == false)
                        if (item2.BusStationKey == item)
                            exist = true;
                if (exist == false)
                    throw new ArgumentException($"Station number {item} doesn't exist");
            }
            BusesLines.Add(new BusLine(line, Stations));
        }

        public void Add(BusLine myLine)
        {
            BusesLines.Add(new BusLine(myLine));
        }


        /// <summary>
        /// function Remove - to remove a bus line from collection
        /// </summary>
        /// <param name="myLine">int - num of line to delete</param>
        public void Remove(BusLine myLine)
        {
            BusesLines.Remove(myLine);
        }

        /// <summary>
        /// Function LisOfLine - list of all bus' lines in a station
        /// </summary>
        /// <param name="codeStation">int - code of station</param>
        /// <returns></returns>
        public List<BusLine> ListOfLines(int codeStation)
        {
            List<BusLine> ListLines = new List<BusLine>();
            BusStation myStation = SearchStation(codeStation);
            foreach (BusLine item in BusesLines)
            {
                if (item.searchStationIndex(codeStation) != -1)
                    ListLines.Add(item);
            }
            if (!ListLines.Any())
                throw new ArgumentException($"No line goes through station {codeStation}");
            return ListLines;
        } 
        
        /// <summary>
        /// Sorted List of all Bus' lines
        /// </summary>
        public void SortedBusLineList()
        {
            BusesLines.Sort(delegate (BusLine x, BusLine y)
            {
                return x.TotalTime.CompareTo(y.TotalTime);
            });
        }

        /// <summary>
        /// Indexer - return BusLine by line number
        /// </summary>
        /// <param name="lineNumber"></param>
        /// <returns></returns>
        public BusLine this[int lineNumber]
        {
            get
            {
                foreach (BusLine item in BusesLines)
                    if (item.BusLineNumber == lineNumber)
                        return item;
                throw new ArgumentException($"There is no Bus Line with number {lineNumber}.");
            }

        }

        public bool Empty()
        {
            if (BusesLines.Count == 0)
                return true;
            return false;
        }
    }



    /// <summary>
    /// Main of the program
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            //Initialization of buses collection
            BusesCollection allBuses = new BusesCollection();

            //creation of 40 stations
            for(int i=0; i<=40; i++)
                new BusStation();

            List<int> stations1 = new List<int> { 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22 };
            List<int> stations2 = new List<int> { 1, 3, 5, 7, 9, 11, 13, 15, 17, 18, 20, 21, 22, 26, 30 };
            List<int> stations3 = new List<int> { 3, 6, 9, 12, 15, 18, 21, 24, 30, 34, 38 };
            List<int> stations4 = new List<int> { 1, 5, 10, 15, 20, 25, 30, 35, 40, 41};
            List<int> stations5 = new List<int> { 4, 8, 12, 15, 14, 16, 21, 27, 28, 37, 39 };
            List<int> stations6 = new List<int> { 2, 8, 9, 15, 12, 13, 23, 29, 32, 36, 39 };
            List<int> stations7 = new List<int> { 5, 7, 14, 23, 31, 33, 35, 40 };
            List<int> stations8 = new List<int> { 8, 12, 17, 19, 29, 31, 37, 39 };
            List<int> stations9 = new List<int> { 1, 6, 10, 11, 18, 23, 24, 30, 40 };
            List<int> stations10 = new List<int> { 3, 4, 11, 15, 16, 21, 28, 29, 33, 37, 41 };

            //creation of 10 lines
            allBuses.Add(3, stations1);
            allBuses.Add(41, stations2);
            allBuses.Add(15, stations3);
            allBuses.Add(6, stations4);
            allBuses.Add(13, stations5);
            allBuses.Add(32, stations6);
            allBuses.Add(74, stations7);
            allBuses.Add(50, stations8);
            allBuses.Add(33, stations9);
            allBuses.Add(1, stations10);




            string choice;
            do
            {

                //menu
                Console.WriteLine("Please choose action you want do :\nAdd an element\n    a1: add a new Bus line\n" +
                    "    a2: Add a new station in a bus line\nRemove an element\n    b1: Remove a bus line\n    b2:Remove a " +
                    "station in a bus line\nSearch\n    c1: Search bus lines in station\n    c2: Search the best travel\n" +
                    "Print\n    d1: Print all bus' lines\n    d2: Print all stations with lines that pass through them\n" +
                    "e: Exit");
                choice = Console.ReadLine();
                
                switch (choice)
                {

                    case "a1":
                        Console.WriteLine("Please enter number of the new line you want add : ");
                        int line = int.Parse(Console.ReadLine());
                        Console.WriteLine("Please enter list of bus stations in this new line (0 at end) : ");
                        List<int> stations = new List<int>();

                        int oneStation = int.Parse(Console.ReadLine());
                        while (oneStation != 0)
                        {

                            stations.Add(oneStation);
                            oneStation = int.Parse(Console.ReadLine());
                        }

                        try
                        {
                            allBuses.Add(line, stations);
                        }
                        catch (ArgumentException e)
                        {
                            Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);

                        }
                        Console.WriteLine();
                        break;
                    case "a2":
                        BusLine LineToAdd = new BusLine();
                        try
                        {
                            LineToAdd = allBuses[int.Parse(Console.ReadLine())];
                            Console.WriteLine("Please enter station's code to add");
                            int codeStation = int.Parse(Console.ReadLine());
                            Console.WriteLine("Choose where add the new station (1 for start, 2 for middle, 3 for end) : ");
                            LineToAdd.AddStation(int.Parse(Console.ReadLine()), codeStation);
                        }
                        catch (ArgumentException e)
                        {
                            Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
                        }
                        break;
                    case "b1":
                        BusLine lineToDelete = new BusLine();
                        Console.WriteLine("Please enter number of line to remove : ");
                        try
                        {
                            lineToDelete = allBuses[int.Parse(Console.ReadLine())];
                        }
                        catch (ArgumentException e)
                        {
                            Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
                        }
                        allBuses.Remove(lineToDelete);
                        break;
                    case "b2":
                        BusLine myLine = new BusLine();
                        Console.WriteLine("Please enter line's number : ");
                        line = int.Parse(Console.ReadLine());
                        Console.WriteLine("Please enter station's code to delete : ");
                        int code = int.Parse(Console.ReadLine());
                        try
                        {
                            myLine = allBuses[line];
                        }
                        catch (ArgumentException e)
                        {
                            Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
                        }

                        try
                        {
                            myLine.DeleteStation(code);
                        }
                        catch (ArgumentException e)
                        {
                            Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
                        }
                        break;
                    case "c1":
                        Console.WriteLine("Please enter line's number : ");
                        List<BusLine> allLinesInStation = allBuses.ListOfLines(int.Parse(Console.ReadLine()));
                        foreach (BusLine item in allLinesInStation)
                            Console.WriteLine(item.BusLineNumber + " ");
                        break;
                    case "c2":
                        Console.WriteLine("Please enter source's station : ");
                        int statSource = int.Parse(Console.ReadLine());
                        Console.WriteLine("Please enter destination's station : ");
                        int statDestination = int.Parse(Console.ReadLine());
                        BusesCollection routes = new BusesCollection();

                        List<BusLine> allLines = allBuses.ListOfLines(statSource);
                        foreach (BusLine item in allLines)
                        {
                            int indexSource = item.searchStationIndex(statSource);
                            int indexDestination = item.searchStationIndex(statDestination);
                            if (indexDestination >= indexSource)
                                routes.Add(item.PartOfLine(indexSource, indexDestination));
                        }
                        if (routes.Empty() == true)
                            throw new ArgumentException("No line pass trough these 2 stations.");
                        routes.SortedBusLineList();
                        foreach (BusLine item in routes)
                            Console.WriteLine(item.ToString());
                        break;
                    case "d1":
                        foreach (BusLine item in allBuses)
                            Console.WriteLine(item.ToString());
                        break;
                    case "d2":
                        List<BusLine> lines = new List<BusLine>();
                        foreach (BusStation item in ListBusStation.ListOfAllStations)
                        {
                            try
                            {
                                lines = allBuses.ListOfLines(item.BusStationKey);
                                Console.Write(item.BusStationKey + ": ");
                                foreach (BusLine item2 in lines)
                                    Console.Write(item2.BusLineNumber + " ");
                            }
                            catch (ArgumentException e)
                            {
                                Console.WriteLine("{0}: {1}", e.GetType().Name, e.Message);
                            }

                            Console.WriteLine();
                        }
                        break;
                    case "e":
                        break;
                    default:
                        break;
                }
            } while (choice != "e");
        }
    }
}
