﻿using System;
using BLApi;
using APIDL;
using DS;
using BO;
using DO;
using System.Linq;
using System.Collections.Generic;

namespace BL
{
	class BLIMP : IBL
    {
		readonly IDAL dl = DalFactory.GetDal();
		static Random rndLat = new Random(DateTime.Now.Millisecond);
		static Random rndLong = new Random(DateTime.Now.Millisecond);

        #region Singleton
        static readonly BLIMP instance = new BLIMP();
        static BLIMP() { }
        BLIMP() { }
        public static BLIMP Instance => instance;
        #endregion

        #region BusStation
        DO.BusStation BusStationBoDoAdapter (BO.BusStation stationBo)
        {
			DO.BusStation stationDo = new DO.BusStation();
			stationBo.CopyPropertiesTo(stationDo);
			if (stationDo.Latitude == 0)
				stationDo.Latitude = rndLat.NextDouble() + 31;
			if (stationDo.Longitude == 0)
				stationDo.Longitude = rndLong.NextDouble() + 34;
			return stationDo;
		}
		public BO.BusStation BusStationDoBoAdapter (DO.BusStation stationDo)
        {
			BO.BusStation stationBo = new BO.BusStation();
			stationDo.CopyPropertiesTo(stationBo);
			List<int> test = (from line in dl.GetAllLineStationsBy(s => s.StationKey == stationBo.BusStationKey)
										select line.LineId).ToList();
			stationBo.LinesThatPass = test;
			return stationBo;
        }

		public void AddStation(BO.BusStation newStation)
		{
			DO.BusStation stationToAdd = BusStationBoDoAdapter(newStation);
			try
			{
				dl.AddStation(stationToAdd);
			}
			catch
            {
				throw new ArgumentException("BL failed to add station because : Duplicate Stations");
            }
		}

        public void DeleteStation(int key)
        {
            foreach (DO.BusLine line in dl.GetAllLines())
            {
                if (GetLineStation(line.Id, key) != null)
                    throw new ArgumentException(" לא ניתן למחוק את התחנה כי קו " + line.BusLineNumber
                        + " עובר בה. אם ברצונכם למחוק את התחנה מחקו אותה קודם מהקו ");
            }
            dl.DeleteStation(key);

        }
        public void UpdateBusStation(BO.BusStation station)
		{
			try
			{
				dl.UpdateStation(BusStationBoDoAdapter(station));
			}
			catch
			{
				throw new ArgumentException("This Bus Station not exist");
			}
			
		}

		public void UpdateBusStation(int id, Action<BO.BusStation> action)
		{
			//Action<DO.BusStation> newAction = action(BusLineDoBoAdapter(DO.BusStation));
			//dl.UpdateStation(id, newAction);
		}


		public BO.BusStation GetBusStation(int key)
		{
			try
            {
				return BusStationDoBoAdapter(dl.GetStation(key));
			}
			catch
            {
				throw new ArgumentException("Bus station " + key + " doesn't exist");
            }
		}

		public IEnumerable<BO.BusStation> GetAllBusStations()
		{
            try
            {
                //var listTemp = dl.GetAllStations().ToList();
                //List<BO.BusStation> listStations = new List<BO.BusStation>();
                //foreach (var item in listTemp)
                //    listStations.Add(BusStationDoBoAdapter(item));
                //return listStations;
                return from bs in dl.GetAllStations()
                       let stat1 = bs
                       select BusStationDoBoAdapter(stat1);
            }
            catch
            {
                throw new ArgumentException("There is an error with recperation of all stations' list");
            }
        }

		public IEnumerable<BO.BusStation> GetAllBusStationsBy(Predicate<BO.BusStation> predicate)
        {
			return from item in GetAllBusStations()
				   where predicate(item)
				   select item;
        }
		#endregion

		#region BusLine
		
		public DO.Areas AreasAdapter(BO.Areas areaBO)
		{
			switch (areaBO)
			{
				case BO.Areas.General:
					return DO.Areas.General;

				case BO.Areas.North:
					return DO.Areas.North;

				case BO.Areas.South:
					return DO.Areas.South;
					
				case BO.Areas.Center:
					return DO.Areas.Center;

				case BO.Areas.Jerusalem:
					return DO.Areas.Jerusalem;
			}
			return DO.Areas.Jerusalem;
		}

		BO.BusLine BusLineDoBoAdapter(DO.BusLine lineDo)
        {
			BO.BusLine lineBo = new BO.BusLine();
			lineDo.CopyPropertiesTo(lineBo);

			DO.BusLine busLine = dl.GetLine(lineBo.BusLineNumber, AreasAdapter(lineBo.Area));
			List<int> request= (from station in dl.GetAllLineStationsBy(s => s.LineId == busLine.Id)
						orderby station.RankInLine
						select station.StationKey).ToList();
			lineBo.AllStationsOfLine = request;
            for (int i = 0; i < lineBo.AllStationsOfLine.Count() - 1; i++)
            {
				DO.BusStation station1 = BusStationBoDoAdapter(GetBusStation(lineBo.AllStationsOfLine.ElementAt(i)));
				DO.BusStation station2 = BusStationBoDoAdapter(GetBusStation(lineBo.AllStationsOfLine.ElementAt(i+1)));

				lineBo.TotalDistance += dl.GetFollowingStations(station1, station2).Distance;
            }
            lineBo.TotalTime = lineBo.TotalDistance * 0.0012 * 0.5;
			return lineBo;
        }

		DO.BusLine BusLineBoDoAdapter(BO.BusLine lineBo)
        {
			DO.BusLine lineDo = new DO.BusLine();
			lineBo.CopyPropertiesTo(lineDo);
			return lineDo;
        }

		public void AddBusLine(BO.BusLine newLine)
		{
            //1. Verify if all stations of list exists
            foreach (int item in newLine.AllStationsOfLine)
            {
                if (dl.GetStation(item) == null)
                    throw new ArgumentException("Bus Station " + item + " doesn't exist. Please create station and then" +
                        " add it to line");
            }

            for (int i = 0; i < (newLine.AllStationsOfLine.Count() - 1); i++)
            {
				//2. Add FollowingStations if it's necessary
				DO.BusStation station1 = dl.GetStation(newLine.AllStationsOfLine.ElementAt(i));
				DO.BusStation station2 = dl.GetStation(newLine.AllStationsOfLine.ElementAt(i + 1));
				if (dl.GetFollowingStations(station1, station2) == null)
					dl.AddFollowingStations(station1, station2);
			}

			dl.AddLine(BusLineBoDoAdapter(newLine));//add lint to can then add lisStations with LineId (attributed in dl.AddLine)

			//3. Create corresponding line stations
			int lineId = dl.GetLine(newLine.BusLineNumber, AreasAdapter(newLine.Area)).Id;
			for (int i = 0; i < newLine.AllStationsOfLine.Count(); i++)
			{
                int stationKey = GetBusStation(newLine.AllStationsOfLine.ElementAt(i)).BusStationKey;
                if (GetLineStation(lineId, stationKey) == null)
                {
                    AddLineStation(new BO.LineStation {
                        LineId = lineId,
                        StationKey = stationKey,
                        RankInLine = i + 1
                    });
                }
			}
		}

		public DO.Areas GetArea(int id)
		{
			return dl.GetLine(id).Area;
		}

		public void DeleteBusLine(BO.BusLine lineBo)
		{
            try
            {
				dl.DeleteLine(BusLineBoDoAdapter(lineBo));
				dl.DeleteLineStation(s => s.LineId == lineBo.Id);
            }
            catch
            {
				throw new ArgumentException("This Line not exist " + lineBo.BusLineNumber);
            }
		}

		public void UpdateBusLine(BO.BusLine line)
		{
			try
			{
                DeleteBusLine(line);
                AddBusLine(line);
                //dl.UpdateLine(BusLineBoDoAdapter(line));
            }
			catch
			{
				throw new ArgumentException("This Line not exist " + line.BusLineNumber);
			}
		}

		public void UpdateBusLine(int id, Action<DO.BusLine> action)
		{
			try
			{
				BO.BusLine line = GetBusLine(id);
				dl.UpdateLine(BusLineBoDoAdapter(line), action);
			}
			catch
			{
				throw new ArgumentException("This Line not exist " + id);
			}
			
		}

		public BO.BusLine GetBusLine(int id)
        {
			return BusLineDoBoAdapter(dl.GetLine(id));
        }

		public BO.BusLine GetBusLine(int lineNumber, DO.Areas area)
		{
			return BusLineDoBoAdapter(dl.GetLine(lineNumber, area));
		}

		public IEnumerable<BO.BusLine> GetAllBusLines()
		{
			return from line in dl.GetAllLines()
				   select BusLineDoBoAdapter(line);
		}

		public IEnumerable<BO.BusLine> GetAllBusLinesBy(Predicate<BO.BusLine> predicate)
        {
			return from item in GetAllBusLines()
				   where predicate(item)
				   select item;
        }

		#endregion

		#region LineStation
		BO.LineStation LineStationDoBoAdapter(DO.LineStation lineStationDo)
        {
			BO.LineStation lineStationBo = new BO.LineStation();
			lineStationDo.CopyPropertiesTo(lineStationBo);
			return lineStationBo;
        }
		DO.LineStation LineStationBoDoAdapter (BO.LineStation lineStationBo)
        {
			DO.LineStation lineStationDo = new DO.LineStation();
			lineStationBo.CopyPropertiesTo(lineStationDo);
			return lineStationDo;
		}
		public IEnumerable<BO.LineStation> GetAllLineStations()
        {
			return from item in dl.GetAllLineStations()
				   select LineStationDoBoAdapter(item);
        }
		public IEnumerable<BO.LineStation> GetAllLineStationsBy(Predicate<BO.LineStation> predicate)
        {
			return from item in GetAllLineStations()
				   where predicate(item)
				   select item;
        }
		public BO.LineStation GetLineStation(int id)
        {
			return null;
        }
		public BO.LineStation GetLineStation(int lineId, int stationId)
        {
            DO.LineStation ls = dl.GetLineStation(lineId, stationId);
            if (ls != null)
                return LineStationDoBoAdapter(dl.GetLineStation(lineId, stationId));
            else
                return null;
		}
		public void AddLineStation(BO.LineStation lineStation)
        {
			dl.AddLineStation(LineStationBoDoAdapter(lineStation));
        }
		public void UpdateLineStation(BO.LineStation lineStation)
        {
			dl.UpdateLineStation(LineStationBoDoAdapter(lineStation));
        }
		public void DeleteLineStation(BO.LineStation lineStation)
        {
			dl.DeleteLineStation(LineStationBoDoAdapter(lineStation));
        }
		public void DeleteLineStation(int id)
        {

        }
        #endregion

        #region LineTrip
        DO.LineTrip LineTripBoDoAdapter(BO.LineTrip lineTripBo)
        {
            DO.LineTrip lineTripDo = new DO.LineTrip();
            lineTripBo.CopyPropertiesTo(lineTripDo);
            return lineTripDo;
        }
        BO.LineTrip LineTripDoBoAdapter(DO.LineTrip lineTripDo)
        {
            BO.LineTrip lineTripBo = new BO.LineTrip();
            lineTripDo.CopyPropertiesTo(lineTripBo);
            return lineTripBo;
        }
        public BO.LineTrip GetLineTrip(int id)
        {
            return LineTripDoBoAdapter(dl.GetLineTrip(id));
        }
        public IEnumerable<BO.LineTrip> GetTripsForABus(BO.BusLine line)
        {
            return from trip in dl.GetAllLineTrips()
                   where trip.LineNumber == line.BusLineNumber
                   select LineTripDoBoAdapter(trip);
        }
        public IEnumerable<BO.LineTrip> GetAllLineTrips()
        {
            return from trip in dl.GetAllLineTrips()
                   select LineTripDoBoAdapter(trip);
        }
        public void AddLineTrip(BO.LineTrip trip)
        {
            dl.AddLineTrip(LineTripBoDoAdapter(trip));
        }
        public void DeleteLineTrip(BO.LineTrip trip)
        {
            dl.DeleteLineTrip(LineTripBoDoAdapter(trip));
        }
        public TimeSpan CalculateDistance(BO.LineStation station)
        {
            //from Departure to station
            //needs data source
            return TimeSpan.Zero;
        }
        #endregion
    }
}