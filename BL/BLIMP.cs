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
			catch (DO.DuplicateStationException ex)
            {
                throw new BO.DuplicateStationException("You can't add this station because it already exists", ex);
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
			catch (DO.InexistantStationException ex)
			{
                throw new BO.InexistantStationException("This Bus Station not exist", ex);
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
			catch (DO.InexistantStationException ex)
            {
                throw new BO.InexistantStationException("This Bus Station not exist", ex);
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
                lineBo.TotalTime += dl.GetFollowingStations(station1, station2).AverageJourneyTime;
            }
            lineBo.TotalDistance = Math.Round(lineBo.TotalDistance /= 1000);

            lineBo.AllLineTripsOfLine = (from lt in GetAllLineTripsBy(t => t.LineId == busLine.Id)
                                         orderby lt.StartTimeRange
                                         select lt).ToList();
            
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
                try
                {
                    dl.GetStation(item);
                }
                catch (DO.InexistantStationException ex)
                {
                    throw new BO.InexistantStationException("Please create station and then add it to line", ex);
                }
            }

            for (int i = 0; i < (newLine.AllStationsOfLine.Count() - 1); i++)
            {
				//2. Add FollowingStations if it's necessary
				DO.BusStation station1 = dl.GetStation(newLine.AllStationsOfLine.ElementAt(i));
				DO.BusStation station2 = dl.GetStation(newLine.AllStationsOfLine.ElementAt(i + 1));
				if (dl.GetFollowingStations(station1, station2) == null)
					dl.AddFollowingStations(station1, station2);
                newLine.TotalTime += dl.GetFollowingStations(station1, station2).AverageJourneyTime;
			}

            dl.AddLine(BusLineBoDoAdapter(newLine));//add line to can then add lisStations with LineId (attributed in dl.AddLine)

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

            //4. Create corresponding trip line
            foreach (BO.LineTrip item in newLine.AllLineTripsOfLine)
            {
                item.LineId = lineId;
                AddLineTrip(item);
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
                dl.DeleteLineTrip(t => t.LineId == lineBo.Id);
            }
            catch(DO.InexistantLineException ex)
            {
                throw new BO.InexistantLineException("This Line not exist", ex);
            }
		}

		public void UpdateBusLine(BO.BusLine line)
		{
			try
			{
                DeleteBusLine(line);
                AddBusLine(line);
            }
			catch (DO.InexistantLineException ex)
            {
				throw new BO.InexistantLineException("This Line not exist", ex);
            }
		}

		public void UpdateBusLine(int id, Action<DO.BusLine> action)
		{
			try
			{
				BO.BusLine line = GetBusLine(id);
				dl.UpdateLine(BusLineBoDoAdapter(line), action);
			}
			catch (DO.InexistantLineException ex)
            {
				throw new BO.InexistantLineException("This Line not exist", ex);
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
        BO.LineTrip LineTripDoBoAdapter(DO.LineTrip lineTripDo)
        {
            BO.LineTrip lineTripBo = new BO.LineTrip();
            lineTripDo.CopyPropertiesTo(lineTripBo);
            return lineTripBo;
        }
        DO.LineTrip LineTripBoDoAdapter(BO.LineTrip lineTripBo)
        {
            DO.LineTrip lineTripDo = new DO.LineTrip();
            lineTripBo.CopyPropertiesTo(lineTripDo);
            return lineTripDo;
        }
        public IEnumerable<BO.LineTrip> GetAllLineTrips()
        {
            return from item in dl.GetAllLineTrips()
                   select LineTripDoBoAdapter(item);
        }
        public IEnumerable<BO.LineTrip> GetAllLineTripsBy(Predicate<BO.LineTrip> predicate)
        {
            return from item in GetAllLineTrips()
                   where predicate(item)
                   select item;
        }
        public BO.LineTrip GetLineTrip(int lineId, TimeSpan startTime)
        {
            DO.LineTrip ls = dl.GetLineTrip(lineId, startTime);
            if (ls != null)
                return LineTripDoBoAdapter(dl.GetLineTrip(lineId, startTime));
            else
                return null;
        }
        public void AddLineTrip(BO.LineTrip lineTrip)
        {
            dl.AddLineTrip(LineTripBoDoAdapter(lineTrip));
        }
        public void UpdateLineTrip(BO.LineTrip lineTrip)
        {
            dl.UpdateLineTrip(LineTripBoDoAdapter(lineTrip));
        }
        public void DeleteLineTrip(BO.LineTrip lineTrip)
        {
            dl.DeleteLineTrip(LineTripBoDoAdapter(lineTrip));
        }
        public void DeleteLineTrip(Predicate<BO.LineTrip> predicate)
        {
            //dl.DeleteLineTrip(predicate);
        }
        public void DeleteLineTrip(int id)
        {

        }
        #endregion

        public IEnumerable<LineTiming> ListArrivalOfLine (int lineId, TimeSpan hour, int stationKey)
        {
            //Calcul of TravelTime between first station of line and our station
            BO.BusLine line = GetBusLine(lineId);
            TimeSpan durationOfTravel = DurationOfTravel(line, stationKey);

            DO.LineTrip myLineTrip = dl.GetLineTrip(lineId, hour);
            

            List<LineTiming> listTiming = new List<LineTiming>(); //initialize list of all timing for the specified line
            while (myLineTrip.StartTimeRange + durationOfTravel < hour)
                myLineTrip.StartTimeRange += myLineTrip.Frequency; //we can change value of StartTimeRange thanks to Clone() 
            for (TimeSpan i = myLineTrip.StartTimeRange; i<= hour;)
            {
                listTiming.Add(new LineTiming {
                    TripStart = i,
                    LineId = myLineTrip.LineId,
                    ExpectedTimeTillArrive = CalculateTimeOfArrival(i, durationOfTravel)
                });
                i += myLineTrip.Frequency;
            }
            //if station is the first we want to show 2 nexts departures
            if(stationKey == line.FirstStationKey)
            {
                listTiming.Add(new LineTiming {
                    TripStart = myLineTrip.StartTimeRange,
                    LineId = myLineTrip.LineId,
                    ExpectedTimeTillArrive = myLineTrip.StartTimeRange
                });
                myLineTrip.StartTimeRange += myLineTrip.Frequency;
                listTiming.Add(new LineTiming {
                    TripStart = myLineTrip.StartTimeRange,
                    LineId = myLineTrip.LineId,
                    ExpectedTimeTillArrive = myLineTrip.StartTimeRange
                });
            }
            return listTiming;
            
        }

        internal TimeSpan CalculateTimeOfArrival (TimeSpan startOfTRavel, TimeSpan durationOfTravel)
        {
            return startOfTRavel + durationOfTravel;
        }

        internal TimeSpan DurationOfTravel(BO.BusLine line, int stationKey)
        {
           int rankOfStation = dl.GetLineStation(line.Id, stationKey).RankInLine;
           IEnumerable<DO.LineStation> stations =  (from lineStat in dl.GetAllLineStationsBy(l => l.LineId == line.Id).ToList()
                                                    where lineStat.RankInLine <= rankOfStation
                                                    select lineStat).ToList();

            TimeSpan travelDuration = new TimeSpan();
            for(int i = 0; i < stations.Count() - 1; i++)
            {
                DO.BusStation station1 = BusStationBoDoAdapter(GetBusStation(stations.ElementAt(i).StationKey));
                DO.BusStation station2 = BusStationBoDoAdapter(GetBusStation(stations.ElementAt(i + 1).StationKey));

                travelDuration += dl.GetFollowingStations(station1, station2).AverageJourneyTime;
            }

            return travelDuration;
        }

        public IEnumerable<IGrouping< TimeSpan, LineTiming>> StationTiming(BO.BusStation station, TimeSpan hour)
        {
            //if (station.LinesThatPass == null)
            //    throw new BO.InexistantLineTripException("לא נמצעו נסיעות בשעות אלו עבור הקו המבוקש");
            try
            {
                List<LineTiming> timing = new List<LineTiming>();
                foreach (int lineId in station.LinesThatPass)
                {
                    foreach (var item in ListArrivalOfLine(lineId, hour, station.BusStationKey))
                        timing.Add(item);
                }

                return from item in timing
                       group item by item.ExpectedTimeTillArrive;
            }
            catch (DO.InexistantLineTripException ex)
            {
                throw new BO.InexistantLineTripException("לא נמצעו נסיעות בשעות אלו עבור הקו המבוקש", ex);
            }
           
        }
    }
}