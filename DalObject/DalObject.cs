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

        public void AddFollowingStations(BusStation station1, BusStation station2)
        {
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

        public void AddLine(BusLine line)
        {
            DataSource.ListLines.Add(
                new BusLine
                {
                    Id = DS.DataSource.LineId++,
                    BusLineNumber = line.BusLineNumber,
                    Area = line.Area,
                    FirstStationKey = line.FirstStationKey,
                    LastStationKey = line.LastStationKey,
                    TotalTime = line.TotalTime
                }
             );
        }

        public void AddLineStation(LineStation lineStation)
        {
            DataSource.ListLineStations.Add( 
                new LineStation
                {
                    Id = DS.DataSource.LineStationId++,
                    LineId = lineStation.LineId,
                    StationKey = lineStation.StationKey,
                    RankInLine = lineStation.RankInLine
                });
        }

        public void AddStation(BusStation station)
        {
            DataSource.ListStations.Add( 
                new BusStation
                {
                    BusStationKey = station.BusStationKey,
                    Address = station.Address,
                    StationName = station.StationName,
                    Latitude = station.Latitude,
                    Longitude = station.Longitude
                });
        }

        public void DeleteFollowingStations(int id)
        {
            var followingStationToDelete = DataSource.ListFollowingStations.Where(t => t.Id == id).FirstOrDefault();
            DataSource.ListFollowingStations.Remove(followingStationToDelete);
        }

        public void DeleteLine(int id)
        {
            var lineToDelete = DataSource.ListLines.Where(t => t.Id == id).FirstOrDefault();
            DataSource.ListLines.Remove(lineToDelete);
        }

        public void DeleteLineStation(int id)
        {
            var lineStationToDelete = DataSource.ListLineStations.Where(t => t.Id == id).FirstOrDefault();
            DataSource.ListLineStations.Remove(lineStationToDelete);
        }

        public void DeleteStation(int id)
        {
            var stationToDelete = DataSource.ListStations.Where(t => t.BusStationKey == id).FirstOrDefault();
            DataSource.ListStations.Remove(stationToDelete);
        }

        public IEnumerable<FollowingStations> GetAllFollowingStations()
        {
            return DataSource.ListFollowingStations;
        }

        public IEnumerable<FollowingStations> GetAllFollowingStationsBy(Predicate<FollowingStations> predicate)
        {
            return from fs in DataSource.ListFollowingStations
                   where predicate(fs)
                   select fs.Clone();
        }

        public IEnumerable<BusStation> GetAllLines()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusStation> GetAllLinesBy(Predicate<BusStation> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusStation> GetAllLineStations()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusStation> GetAllLineStationsBy(Predicate<BusStation> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusStation> GetAllStations()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusStation> GetAllStationsBy(Predicate<BusStation> predicate)
        {
            throw new NotImplementedException();
        }

        public FollowingStations GetFollowingStations(int id)
        {
            throw new NotImplementedException();
        }

        public BusStation GetLine(int id)
        {
            throw new NotImplementedException();
        }

        public BusStation GetLineStation(int id)
        {
            throw new NotImplementedException();
        }

        public BusStation GetStation(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateFollowingStations(BusStation station)
        {
            throw new NotImplementedException();
        }

        public void UpdateFollowingStations(int id, Action<BusStation> update)
        {
            throw new NotImplementedException();
        }

        public void UpdateLine(BusStation station)
        {
            throw new NotImplementedException();
        }

        public void UpdateLine(int id, Action<BusStation> update)
        {
            throw new NotImplementedException();
        }

        public void UpdateLineStation(BusStation station)
        {
            throw new NotImplementedException();
        }

        public void UpdateLineStation(int id, Action<BusStation> update)
        {
            throw new NotImplementedException();
        }

        public void UpdateStation(BusStation station)
        {
            throw new NotImplementedException();
        }

        public void UpdateStation(int id, Action<BusStation> update)
        {
            throw new NotImplementedException();
        }
       


    }
}
