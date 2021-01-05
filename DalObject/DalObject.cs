using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIDL;
using DO;
using DS;

namespace DL
{
    public class DalObject : IDAL
    {
        //CRUD method implimentations
        #region singelton
        static readonly DalObject instance = new DalObject();
        static DalObject() { }
        DalObject() { }
        public static DalObject Instance => instance;
        #endregion

        #region Station
        public void AddStation(BusStation station)
        {
            if (DataSource.ListStations.FirstOrDefault(s => s.BusStationKey == station.BusStationKey) != null)
                throw new ArgumentException("Duplicate Stations");
            DataSource.ListStations.Add(station.Clone());
        }

        public void DeleteStation(int id)
        {
            var stationToDelete = DataSource.ListStations.Where(s => s.BusStationKey == id).FirstOrDefault();
            DataSource.ListStations.Remove(stationToDelete);
        }

        public IEnumerable<BusStation> GetAllStations()
        {
            return from station in DataSource.ListStations
                   select station.Clone();
        }

        public IEnumerable<BusStation> GetAllStationsBy(Predicate<BusStation> predicate)
        {
            return from station in DataSource.ListStations
                   where predicate(station)
                   select station.Clone();
        }

        public BusStation GetStation(int id)
        {
            return DataSource.ListStations.Find(s => s.BusStationKey == id).Clone();
        }
        public void UpdateStation(BusStation station)
        {
            BusStation sta = DataSource.ListStations.Find(s => s.BusStationKey == station.BusStationKey);
            if (sta != null)
            {
                DataSource.ListStations.Remove(sta);
                DataSource.ListStations.Add(station.Clone());
            }
            //else
                //throw new exception that station to update doesn't exist 
        }

        public void UpdateStation(int id, Action<BusStation> update)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region FollowingStations
        public void AddFollowingStations(BusStation station1, BusStation station2)
        {
            if (DataSource.ListFollowingStations.FirstOrDefault(f => f.KeyStation1 == station1.BusStationKey 
                && f.KeyStation2 == station2.BusStationKey) != null)
                //throw new exception to alert on duplicate stations
            DataSource.ListFollowingStations.Add( 
                new FollowingStations
                {
                    Id = DS.DataSource.FollowingStationsId++,
                    KeyStation1 = station1.BusStationKey,
                    KeyStation2 = station2.BusStationKey,
                    Distance = new GeoCoordinate(station1.Latitude, station1.Longitude).GetDistanceTo
                        (new GeoCoordinate(station2.Latitude, station2.Longitude)),
                    AverageJourneyTime = new GeoCoordinate(station1.Latitude, station1.Longitude).GetDistanceTo
                    (new GeoCoordinate(station2.Latitude, station2.Longitude))*0.0012*0.5
                });
        }
        public void DeleteFollowingStations(int id)
        {
            var followingStationToDelete = DataSource.ListFollowingStations.Where(f => f.Id == id).FirstOrDefault();
            DataSource.ListFollowingStations.Remove(followingStationToDelete);
        }
        public IEnumerable<FollowingStations> GetAllFollowingStations()
        {
            return from fs in DataSource.ListFollowingStations
                   select fs.Clone();
        }

        public IEnumerable<FollowingStations> GetAllFollowingStationsBy(Predicate<FollowingStations> predicate)
        {
            return from fs in DataSource.ListFollowingStations
                   where predicate(fs)
                   select fs.Clone();
        }
        public FollowingStations GetFollowingStations(int id)
        {
            return DataSource.ListFollowingStations.Find(f => f.Id == id).Clone();
        }
        public void UpdateFollowingStations(BusStation station1, BusStation station2)
        {
            FollowingStations folStat = DataSource.ListFollowingStations.Find(s => s.KeyStation1 == station1.BusStationKey
            && s.KeyStation2 == station2.BusStationKey);
            if (folStat != null)
            {
                DataSource.ListFollowingStations.Remove(folStat);
                AddFollowingStations(station1, station2);
            }
            //else throx exception
        }

        public void UpdateFollowingStations(int id, Action<FollowingStations> update)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Line
        public void AddLine(BusLine line)
        {
            if (DataSource.ListLines.FirstOrDefault(l => l.BusLineNumber == line.BusLineNumber && l.Area == line.Area) != null)
                //throw new DO.BadPersonIdException(person.ID, "Duplicate person ID");
            DataSource.ListLines.Add(line.Clone());
        }
        public void DeleteLine(int id)
        {
            var lineToDelete = DataSource.ListLines.Where(t => t.Id == id).FirstOrDefault();
            DataSource.ListLines.Remove(lineToDelete);
        }
        public IEnumerable<BusLine> GetAllLines()
        {
            return from line in DataSource.ListLines
                   select line.Clone();
        }

        public IEnumerable<BusLine> GetAllLinesBy(Predicate<BusLine> predicate)
        {
            return from line in DataSource.ListLines
                   where predicate(line)
                   select line.Clone();
        }
        public BusLine GetLine(int id)
        {
            return DataSource.ListLines.Find(l => l.Id == id).Clone();
        }
        public void UpdateLine(BusLine line)
        {
            BusLine tempLine = DataSource.ListLines.Find(l => l.BusLineNumber == line.BusLineNumber && l.Area == line.Area);
            if (tempLine != null)
            {
                DataSource.ListLines.Remove(tempLine);
                DataSource.ListLines.Add(line.Clone());
            }
            //else throw exception that line doesn't exist
        }

        public void UpdateLine(int id, Action<BusLine> update)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region LineStation
        public void AddLineStation(LineStation lineStation)
        {
            if (DataSource.ListLineStations.FirstOrDefault(s => s.Id == lineStation.Id)!= null)
                throw new ArgumentException("Duplicate Stations");
            DataSource.ListLineStations.Add(lineStation);

            //DataSource.ListLineStations.Add( 
            //    new LineStation
            //    {
            //        Id = DS.DataSource.LineStationId++,
            //        LineId = lineStation.LineId,
            //        StationKey = lineStation.StationKey,
            //        RankInLine = lineStation.RankInLine
            //    });
        }

        public void DeleteLineStation(int id)
        {
            var lineStationToDelete = DataSource.ListLineStations.Where(t => t.Id == id).FirstOrDefault();
            DataSource.ListLineStations.Remove(lineStationToDelete);
        }

        public IEnumerable<LineStation> GetAllLineStations()
        {
            return from stat in DataSource.ListLineStations
                   select stat.Clone();
        }

        public IEnumerable<LineStation> GetAllLineStationsBy(Predicate<LineStation> predicate)
        {
            return from stat in DataSource.ListLineStations
                   where predicate(stat)
                   select stat.Clone();
        }

        public LineStation GetLineStation(int id)
        {
            return DataSource.ListLineStations.FirstOrDefault(s => s.Id == id).Clone();
        }


        public void UpdateLineStation(LineStation station)
        {
            LineStation tempStat = DataSource.ListLineStations.FirstOrDefault(s => s.Id == station.Id);
            if (tempStat != null)
            {
                DataSource.ListLineStations.Remove(tempStat);
                DataSource.ListLineStations.Add(station);
            }
            //else throw that line station doesn't exist
        }

        public void UpdateLineStation(int id, Action<LineStation> update)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
