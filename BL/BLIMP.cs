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
			List<int> request= (from station in dl.GetAllLineStationsBy(s => s.LineId == dl.GetLine(lineBo.BusLineNumber, lineBo.Area).Id)
							//let stationKey = station.StationKey
						orderby station.RankInLine
						select station.StationKey).ToList();
			lineBo.AllStationsOfLine = request;
			//for(int i=0; i<lineBo.AllStationsOfLine.Count()-1; i++)
   //         {
			//	lineBo.TotalDistance += dl.GetFollowingStations(BusStationBoDoAdapter(GetBusStation(lineBo.AllStationsOfLine.
			//		ElementAt(i))),BusStationBoDoAdapter(GetBusStation(lineBo.AllStationsOfLine.ElementAt(i + 1)))).Distance;
   //         }
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
			foreach (int item in newLine.AllStationsOfLine)
				if (dl.GetStation(item) != null)
					dl.AddLineStation();
			//foreach element in list of station we will verify if station with this id exist, create lineStation and add 
			//followingstation if necessary	
			for (int i = 0; i < newLine.AllStationsOfLine.Count() - 1; i++)
				if (dl.GetFollowingStations(dl.GetStation(newLine.AllStationsOfLine.ElementAt(i)), 
					dl.GetStation(newLine.AllStationsOfLine.ElementAt(i+1))) == null)

					dl.AddFollowingStations(dl.GetStation(newLine.AllStationsOfLine.ElementAt(i)), 
						dl.GetStation(newLine.AllStationsOfLine.ElementAt(i+1)));
			dl.AddLine(BusLineBoDoAdapter(newLine));
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

		public BO.BusLine GetBusLine(int id)
        {
			return BusLineDoBoAdapter(dl.GetLine(id));
        }
		public BO.BusLine GetBusLine(int lineNumber, Areas area)
		{
			return BusLineDoBoAdapter(dl.GetLine(lineNumber, area));
		}

		public IEnumerable<BO.BusLine> GetAllBusLines()
		{
			return from line in dl.GetAllLines()
				   select BusLineDoBoAdapter(line);
		}

		#endregion
	}
}