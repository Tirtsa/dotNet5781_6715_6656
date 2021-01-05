using System;
using BLApi;
using APIDL;
using DS;
using BO;
using DO;
using System.Linq;
using System.Collections.Generic;

namespace BL
{
	public class BLIMP : IBL
    {
		//static Random rnd = new Random(DateTime.Now.Millisecond);

		readonly IDAL dl = DalFactory.GetDal();

		private DO.BusLine BusLineConverter(BO.BusLine line)
		{
			foreach(DO.BusLine busLine in DataSource.ListLines)
			{
				if (busLine.BusLineNumber == line.BusLineNumber && busLine.Area == line.Area)
					return busLine;
			}
			//throw line doesnt exist
			return null;
		}

		private DO.BusStation BusStationConverter(BO.BusStation station)
		{
			foreach (DO.BusStation busStation in DataSource.ListStations)
			{
				if (busStation.BusStationKey == station.BusStationKey)
					return busStation;
			}
			//throw station doesnt exist
			return null;
		}
		
		public void AddBusLine(BO.BusLine newLine)
		{
			//int id = dl.AddLine(BusLineConverter(newLine));
			foreach (DO.BusLine line in dl.GetAllLines())
			{
				new BO.BusLine { };
			}
		}

		public void AddStation(BO.BusStation newStation)
		{
			//int lineId = dl.AddStation(BusStationConverter(newStation));
			foreach (DO.BusStation station in dl.GetAllStations())
			{
				if (newStation.BusStationKey == station.BusStationKey)
				{
					LineStation temp = new LineStation
					{ 
						Id = station.BusStationKey,
						//LineId = lineId,
						StationKey = station.BusStationKey,
						RankInLine = 1
					};
				}
			}
		}

		public void DeleteBusLine(int id)
		{
			//var busLine = DataSource.ListLines.Where(line => line.Id == id).FirstOrDefault();
			dl.DeleteLine(id);
		}

		public void DeleteStation(int key)
		{
			//var busStation = DataSource.ListStations.Where(station => station.BusStationKey == key).FirstOrDefault();
			dl.DeleteStation(key);
		}

		public void EditBusLine(BO.BusLine line)
		{
			dl.UpdateLine(BusLineConverter(line));
		}

		public void EditBusLine(int id, Action<DO.BusLine> action)
		{
			dl.UpdateLine(id, action);
		}

		public void EditBusStation(BO.BusStation station)
		{
			dl.UpdateStation(BusStationConverter(station));
		}

		public void EditBusStation(int id, Action<DO.BusStation> action)
		{
			dl.UpdateStation(id, action);
		}

		public BO.BusLine GetBusLine(int lineNumber, Areas area)
		{
			foreach (BO.BusLine line in GetBusLines())
			{
				if (line.BusLineNumber == lineNumber && line.Area == area)
					return line;
			}
			return null;
		}

		public BO.BusStation GetBusStation(int key)
		{
			foreach (BO.BusStation station in GetBusStations())
			{
				if (station.BusStationKey == key)
					return station;
			}
			return null;
		}

		public IEnumerable<BO.BusLine> GetBusLines()
		{
			IEnumerable<BO.BusLine> tempLine = null;
			foreach (DO.BusLine busLine in DataSource.ListLines)
			{
				BO.BusLine line = new BO.BusLine
				{
					BusLineNumber = busLine.BusLineNumber,
					Area = busLine.Area,
					FirstStationKey = busLine.FirstStationKey,
					LastStationKey = busLine.LastStationKey,
					TotalTime = busLine.TotalTime
				};
				tempLine.Append(line);
			}
			return tempLine;
		}

		public IEnumerable<BO.BusStation> GetBusStations()
		{
			IEnumerable<BO.BusStation> tempStation = null;
			foreach (DO.BusStation busStation in DataSource.ListStations)
			{
				BO.BusStation station = new BO.BusStation
				{
					BusStationKey = busStation.BusStationKey,
					StationName = busStation.StationName,
					Address = busStation.Address
				};
				tempStation.Append(station);
			}
			return tempStation;
		}
	}
}