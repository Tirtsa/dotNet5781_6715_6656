﻿using System;
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
        //CRUD method implementations
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
            BusStation sta = DataSource.ListStations.Find(s => s.BusStationKey == id);
            if (sta != null)
                DataSource.ListStations.Remove(sta);
            else
                throw new ArgumentException("It's not exist Bus Station with this key : " + id);
        }

        public IEnumerable<BusStation> GetAllStations()
        {
            return from station in DataSource.ListStations
                   orderby station.BusStationKey
                   select station.Clone();
        }

        public IEnumerable<BusStation> GetAllStationsBy(Predicate<BusStation> predicate)
        {
            return from station in DataSource.ListStations
                   where predicate(station)
                   orderby station.BusStationKey
                   select station.Clone();
        }

        public BusStation GetStation(int id)
        {
            BusStation station = DataSource.ListStations.Find(s => s.BusStationKey == id);
                if (station != null)
                    return station.Clone();
                else
                    throw new ArgumentException("Bus Station doesn't exist");
            
        }
        public void UpdateStation(BusStation station)
        {
            BusStation sta = DataSource.ListStations.Find(s => s.BusStationKey == station.BusStationKey);
            if (sta != null)
            {
                DataSource.ListStations.Remove(sta);
                DataSource.ListStations.Add(station.Clone());
            }
            else
                throw new ArgumentException("station to update doesn't exist");
        }

        public void UpdateStation(int id, Action<BusStation> update)
        {
            BusStation sta = DataSource.ListStations.Find(s => s.BusStationKey == id);
            update(sta);
        }
        #endregion

        #region FollowingStations
        public void AddFollowingStations(BusStation station1, BusStation station2)
        {
            if (DataSource.ListFollowingStations.FirstOrDefault(f => (f.KeyStation1 == station1.BusStationKey
                && f.KeyStation2 == station2.BusStationKey) || (f.KeyStation1 == station2.BusStationKey
                && f.KeyStation2 == station1.BusStationKey)) != null)
                throw new ArgumentException("duplicate stations");
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
        public void DeleteFollowingStations(BusStation station1, BusStation station2)
        {
            var followingStationToDelete = DataSource.ListFollowingStations.Where(f => (f.KeyStation1 == station1.BusStationKey
                && f.KeyStation2 == station2.BusStationKey) || (f.KeyStation1 == station2.BusStationKey
                && f.KeyStation2 == station1.BusStationKey)).FirstOrDefault();
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
        public FollowingStations GetFollowingStations(BusStation station1, BusStation station2)
        {
            if (DataSource.ListFollowingStations.Find(f => (f.KeyStation1 == station1.BusStationKey
                 && f.KeyStation2 == station2.BusStationKey)) != null)
                return DataSource.ListFollowingStations.Find(f => (f.KeyStation1 == station1.BusStationKey
                    && f.KeyStation2 == station2.BusStationKey)).Clone();
            else
                return null;
        }
        public void UpdateFollowingStations(BusStation station1, BusStation station2)
        {
            FollowingStations folStat = DataSource.ListFollowingStations.Find(s => (s.KeyStation1 == station1.BusStationKey
            && s.KeyStation2 == station2.BusStationKey) || (s.KeyStation1 == station2.BusStationKey
            && s.KeyStation2 == station1.BusStationKey));
            if (folStat != null)
            {
                DataSource.ListFollowingStations.Remove(folStat);
                AddFollowingStations(station1, station2);
            }
            else
                throw new ArgumentException("Following stations between " + station1 + " and " + station2 + " doesn't exist");
        }

        public void UpdateFollowingStations(BusStation busStation1, BusStation busStation2, Action<FollowingStations> update)
        {
            FollowingStations following = DataSource.ListFollowingStations.Find(s => (s.KeyStation1 == busStation1.BusStationKey
            && s.KeyStation2 == busStation2.BusStationKey) || (s.KeyStation1 == busStation2.BusStationKey
            && s.KeyStation2 == busStation1.BusStationKey));
            update(following);
        }
        #endregion

        #region BusLine
        public void AddLine(BusLine line)
        {
            if (DataSource.ListLines.FirstOrDefault(l => l.BusLineNumber == line.BusLineNumber && l.Area == line.Area) != null)
                throw new ArgumentException("Duplicate BusLine");
            line.Id = DataSource.LineId++;
            DataSource.ListLines.Add(line.Clone());
        }
        public void DeleteLine(BusLine line)
        {
            var lineToDelete = DataSource.ListLines.Where(l => l.BusLineNumber == line.BusLineNumber && l.Area == line.Area).
                FirstOrDefault();
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
        public BusLine GetLine(int lineId, Areas area)
        {
            if (DataSource.ListLines.Find(l => l.BusLineNumber == lineId && l.Area == area) != null)
                return DataSource.ListLines.Find(l => l.BusLineNumber == lineId && l.Area == area).Clone();
            else 
                throw new ArgumentException("There is no line with this number and area" + lineId + area);
        }
        public BusLine GetLine (int Id)
        {
            return DataSource.ListLines.Find(l => l.Id == Id);
        }
        public void UpdateLine(BusLine line)
        {
            BusLine tempLine = DataSource.ListLines.Find(l => l.BusLineNumber == line.BusLineNumber && l.Area == line.Area);
            if (tempLine != null)
            {
                line.Id = tempLine.Id;
                DataSource.ListLines.Remove(tempLine);
                DataSource.ListLines.Add(line.Clone());
            }
            else
                throw new ArgumentException("line doesn't exist");
        }

        public void UpdateLine(BusLine line, Action<BusLine> update)
        {
            BusLine tempLine = DataSource.ListLines.Find(l => l.BusLineNumber == line.BusLineNumber && l.Area == line.Area);
            update(tempLine);
        }
        #endregion

        #region LineStation
        public void AddLineStation(LineStation lineStation)
        {
            if (DataSource.ListLineStations.FirstOrDefault(s => s.Id == lineStation.Id)!= null)
                throw new ArgumentException("Duplicate Stations");
            lineStation.Id = DataSource.LineStationId++;
            DataSource.ListLineStations.Add(lineStation);
        }

        public void DeleteLineStation(LineStation lineStation)
        {
            var lineStationToDelete = DataSource.ListLineStations.Where(t => t.LineId == lineStation.LineId && t.StationKey == 
                lineStation.StationKey).FirstOrDefault();
            DataSource.ListLineStations.Remove(lineStationToDelete);
        }

        public void DeleteLineStation(Predicate<LineStation> predicate)
        {
            var lineStationsToDelete = (from stat in DataSource.ListLineStations
                                      where predicate(stat)
                                      select stat).ToList();
            foreach (var item in lineStationsToDelete)
                DataSource.ListLineStations.Remove(item);
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

        public LineStation GetLineStation(int lineId, int stationKey)
        {
            return DataSource.ListLineStations.FirstOrDefault(s => s.Id == lineId && s.StationKey == stationKey).Clone();
        }


        public void UpdateLineStation(LineStation station)
        {
            LineStation tempStat = DataSource.ListLineStations.FirstOrDefault(s => s.LineId == station.LineId && 
            s.StationKey == station.StationKey);
            if (tempStat != null)
            {
                station.Id = tempStat.Id;
                DataSource.ListLineStations.Remove(tempStat);
                DataSource.ListLineStations.Add(station);
            }
            else
                throw new ArgumentException("line station doesn't exist");
        }

        public void UpdateLineStation(LineStation station, Action<LineStation> update)
        {
            LineStation tempStat = DataSource.ListLineStations.FirstOrDefault(s => s.LineId == station.LineId &&
            s.StationKey == station.StationKey);
            update(tempStat);
        }
        #endregion

        #region LineTrip
        public LineTrip GetLineTrip(int id)
        {
            LineTrip trip = DataSource.ListLineTrips.Find(s => s.Id == id);
            if (trip != null)
                return trip.Clone();
            else
                throw new ArgumentException("This trip doesn't exist");
        }
        public IEnumerable<LineTrip> GetAllLineTrips()
        {
            return from trip in DataSource.ListLineTrips
                   select trip;
        }
        public void AddLineTrip(LineTrip trip)
        {
            if(DataSource.ListLineTrips.FirstOrDefault(t => t.Id == trip.Id) != null)
                throw new ArgumentException("Duplicate trip");
            DataSource.ListLineTrips.Add(trip.Clone());
        }
        public void DeleteLineTrip(LineTrip trip)
        {
            var tripToDelete = DataSource.ListLineTrips.Where(t => t.Id == trip.Id).FirstOrDefault();
            DataSource.ListLineTrips.Remove(tripToDelete);
        }
        #endregion
    }
}
