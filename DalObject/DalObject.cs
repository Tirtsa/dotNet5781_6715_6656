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
                throw new DuplicateStationException(station.BusStationKey, $"Duplicate station : {station.BusStationKey}");
            DataSource.ListStations.Add(station.Clone());
        }

        public void DeleteStation(int id)
        {
            BusStation sta = DataSource.ListStations.Find(s => s.BusStationKey == id);
            if (sta != null)
                DataSource.ListStations.Remove(sta);
            else
                throw new InexistantStationException(id, $"Station : {id} doesn't exist");
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
                throw new InexistantStationException(id, $"Station : {id} doesn't exist");
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
                throw new InexistantStationException(station.BusStationKey, $"Station : {station.BusStationKey} doesn't exist");
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
            if (DataSource.ListFollowingStations.FirstOrDefault(f => f.KeyStation1 == station1.BusStationKey
                && f.KeyStation2 == station2.BusStationKey) != null)
                throw new ArgumentException("duplicate stations");
            DataSource.ListFollowingStations.Add( 
                new FollowingStations
                {
                    KeyStation1 = station1.BusStationKey,
                    KeyStation2 = station2.BusStationKey,
                    Distance = new GeoCoordinate(station1.Latitude, station1.Longitude).GetDistanceTo
                        (new GeoCoordinate(station2.Latitude, station2.Longitude)),
                    AverageJourneyTime = TimeSpan.FromTicks(new TimeSpan(0, 0, 0, 0, 72).Ticks *
                        (long)new GeoCoordinate(station1.Latitude, station1.Longitude).GetDistanceTo
                        (new GeoCoordinate(station2.Latitude, station2.Longitude)))
                });
        }
        public void DeleteFollowingStations(BusStation station1, BusStation station2)
        {
            var followingStationToDelete = DataSource.ListFollowingStations.Where(f => f.KeyStation1 == station1.BusStationKey
                && f.KeyStation2 == station2.BusStationKey).FirstOrDefault();
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
            FollowingStations fsToReturn = DataSource.ListFollowingStations.Find(f => f.KeyStation1 == station1.BusStationKey
                 && f.KeyStation2 == station2.BusStationKey);
            if (fsToReturn != null)
                return fsToReturn.Clone();
            else
                return null;
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
            else
                throw new ArgumentException("Following stations between " + station1 + " and " + station2 + " doesn't exist");
        }

        public void UpdateFollowingStations(BusStation busStation1, BusStation busStation2, Action<FollowingStations> update)
        {
            FollowingStations following = DataSource.ListFollowingStations.Find(s => s.KeyStation1 == busStation1.BusStationKey
            && s.KeyStation2 == busStation2.BusStationKey);
            update(following);
        }
        #endregion

        #region BusLine
        public void AddLine(BusLine line)
        {
            if (DataSource.ListLines.FirstOrDefault(l => l.Id == line.Id) != null)
                throw new DuplicateLineException
                    (line.BusLineNumber, line.Area, $"Duplicate line {line.BusLineNumber} in {line.Area}");
            if (line.Id == 0)
                line.Id = DataSource.LineId++;
            DataSource.ListLines.Add(line.Clone());
        }
        public void DeleteLine(BusLine line)
        {
            var lineToDelete = DataSource.ListLines.Where(l => l.Id == line.Id).
                FirstOrDefault();
            DataSource.ListLines.Remove(lineToDelete);
        }
        public IEnumerable<BusLine> GetAllLines()
        {
            return from line in DataSource.ListLines
                   orderby line.BusLineNumber
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
            BusLine tempLine = DataSource.ListLines.Find(l => l.BusLineNumber == lineId && l.Area == area);
            if (tempLine != null)
                return tempLine.Clone();
            else 
                throw new InexistantLineException (lineId, area, $"There is no line {lineId} in {area}");
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
                throw new InexistantLineException
                    (line.BusLineNumber, line.Area, $"There is no line {line.BusLineNumber} in {line.Area}");
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
            if (DataSource.ListLineStations.FirstOrDefault(s => (s.LineId == lineStation.LineId 
                && s.StationKey == lineStation.StationKey)) != null)
                throw new ArgumentException("Duplicate Stations");
            DataSource.ListLineStations.Add(lineStation);
        }

        public void DeleteLineStation(LineStation lineStation)
        {
            var lineStationToDelete = DataSource.ListLineStations.Where(t => t.LineId == lineStation.LineId &&
            t.StationKey == lineStation.StationKey).FirstOrDefault();
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
            LineStation lsToReturn = DataSource.ListLineStations.FirstOrDefault(s => s.LineId == lineId && s.StationKey == stationKey);
            if (lsToReturn == null)
                return null;
            return lsToReturn.Clone();
        }


        public void UpdateLineStation(LineStation station)
        {
            LineStation tempStat = DataSource.ListLineStations.FirstOrDefault(s => s.LineId == station.LineId && 
            s.StationKey == station.StationKey);
            if (tempStat != null)
            {
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
        public LineTrip GetLineTrip(int lineId, TimeSpan now)
        {
            LineTrip trip = DataSource.ListLineTrips.Find(s => s.LineId == lineId && s.StartTimeRange <= now
            && s.EndTimeRange >= now);
            if (trip != null)
                return trip.Clone();
            else
            {
                int LineNumber = GetLine(lineId).BusLineNumber;
                throw new InexistantLineTripException(LineNumber, now);
            }
        }
        public IEnumerable<LineTrip> GetAllLineTrips()
        {
            return from trip in DataSource.ListLineTrips
                   select trip.Clone();
        }

        public IEnumerable<LineTrip> GetAllLineTripsBy(Predicate<LineTrip> predicate)
        {
            return from trip in DataSource.ListLineTrips
                   where predicate(trip)
                   select trip.Clone();
        }

        public void AddLineTrip(LineTrip trip)
        {
            if (DataSource.ListLineTrips.FirstOrDefault(t => t.LineId == trip.LineId 
            && t.StartTimeRange == trip.StartTimeRange) != null)
                throw new DuplicateLineTripException(GetLine(trip.LineId).BusLineNumber ,trip.StartTimeRange);
            DataSource.ListLineTrips.Add(trip.Clone());
        }

        public void DeleteLineTrip(LineTrip trip)
        {
            var tripToDelete = DataSource.ListLineTrips.Where(t => t.LineId == trip.LineId
            && t.StartTimeRange == trip.StartTimeRange).FirstOrDefault();
            
            DataSource.ListLineTrips.Remove(tripToDelete);
        }

        public void DeleteLineTrip(Predicate<LineTrip> predicate)
        {
            var tripToDelete = from LineTrip item in GetAllLineTripsBy(predicate)
                               select item;
            foreach (LineTrip item in tripToDelete)
                DataSource.ListLineTrips.Remove(item);
        }

        public void UpdateLineTrip (LineTrip trip)
        {
            DeleteLineTrip(trip);
            AddLineTrip(trip);
        }

        #endregion
    
        public User GetUser(string id, string pwd)
        {
            return null;
        }
    }
}
