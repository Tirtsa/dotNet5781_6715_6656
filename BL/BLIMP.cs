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
		readonly IDAL dl = DalFactory.GetDal();
		static Random rndLat = new Random(DateTime.Now.Millisecond);
		static Random rndLong = new Random(DateTime.Now.Millisecond);

        #region BusStation
		DO.BusStation BusStationBoDoAdapter (BO.BusStation stationBo)
        {
			//new DO.BusStation
			//{
			//	BusStationKey = stationBo.BusStationKey,
			//	Address = stationBo.Address,
			//	StationName = stationBo.StationName,
			//	Latitude = rndLat.NextDouble() + 31,
			//	Longitude = rndLong.NextDouble() + 34
			//};
			DO.BusStation stationDo = new DO.BusStation();
			stationBo.CopyPropertiesTo(stationDo);
			if (dl.GetStation(stationDo.BusStationKey).Latitude == 0)
				stationDo.Latitude = rndLat.NextDouble() + 31;
			if (dl.GetStation(stationDo.BusStationKey).Longitude == 0)
				stationDo.Longitude = rndLong.NextDouble() + 34;
			return stationDo;
		}
		BO.BusStation BusStationDoBoAdapter (DO.BusStation stationDo)
        {
			BO.BusStation stationBo = new BO.BusStation();
			stationDo.CopyPropertiesTo(stationBo);
			
			stationBo.LinesThatPass = (from line in dl.GetAllLineStationsBy(s => s.StationKey == stationBo.BusStationKey)
									   select line.LineId).ToList();
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
			try
			{
				dl.DeleteStation(key);
            }
            catch
            {
				throw new ArgumentException("It's not exist Bus Station with this key : " + key);
			}
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
				return from bs in dl.GetAllStations()
					   let stat1 = bs
					   select BusStationDoBoAdapter(stat1);
            }
            catch
            {
				throw new ArgumentException("There is an error with recperation of all stations' list");
            }
		}
		#endregion

		#region BusLine
		BO.BusLine BusLineDoBoAdapter(DO.BusLine lineDo)
        {
			BO.BusLine lineBo = new BO.BusLine();
			lineDo.CopyPropertiesTo(lineBo);

			lineBo.AllStationsOfLine = (from station in dl.GetAllLineStationsBy(s => s.LineId == lineBo.BusLineNumber)
										   //let stationKey = station.StationKey
									   orderby station.RankInLine
									   select BusStationDoBoAdapter(dl.GetStation(station.StationKey))).ToList();
			
			for(int i=0; i<lineBo.AllStationsOfLine.Count()-1; i++)
            {
				lineBo.TotalDistance += dl.GetFollowingStations(BusStationBoDoAdapter(lineBo.AllStationsOfLine.ElementAt(i)),
					BusStationBoDoAdapter(lineBo.AllStationsOfLine.ElementAt(i + 1))).Distance;
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
			//int id = dl.AddLine(BusLineConverter(newLine));
			foreach (DO.BusLine line in dl.GetAllLines())
			{
				new BO.BusLine { };
			}
		}

		public void DeleteBusLine(int id)
		{
			//var busLine = DataSource.ListLines.Where(line => line.Id == id).FirstOrDefault();
			//dl.DeleteLine(id);
		}

		public void EditBusLine(BO.BusLine line)
		{
			//dl.UpdateLine(BusLineConverter(line));
		}

		public void EditBusLine(int id, Action<DO.BusLine> action)
		{
			//dl.UpdateLine(id, action);
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
		#endregion
	}
}