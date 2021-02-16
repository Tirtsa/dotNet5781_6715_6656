using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BLApi
{
    public interface IBL
    {

        #region BusLine
        DO.Areas AreasAdapter(BO.Areas areaBO);
        IEnumerable<BO.BusLine> GetAllBusLines();
        IEnumerable<BO.BusLine> GetAllBusLinesBy(Predicate<BO.BusLine> predicate);
        BO.BusLine GetBusLine(int lineNumber, DO.Areas area);
        BO.BusLine GetBusLine(int id);
        void AddBusLine(BO.BusLine newLine);
        void UpdateBusLine(BO.BusLine line);
        void UpdateBusLine(int id, Action<DO.BusLine> action);
        void DeleteBusLine(BO.BusLine line);
        #endregion

        #region BusStation
        BO.BusStation BusStationDoBoAdapter(DO.BusStation stationDo);
        IEnumerable<BO.BusStation> GetAllBusStations();
        IEnumerable<BO.BusStation> GetAllBusStationsBy(Predicate<BO.BusStation> predicate);
        BO.BusStation GetBusStation(int key);
        void AddStation(BO.BusStation station);
        void UpdateBusStation(BO.BusStation station);
        void UpdateBusStation(int key, Action<BO.BusStation> action);
        void DeleteStation(int key);
        #endregion

        #region LineStation
        IEnumerable<BO.LineStation> GetAllLineStations();
        IEnumerable<BO.LineStation> GetAllLineStationsBy(Predicate<BO.LineStation> predicate);
        BO.LineStation GetLineStation(int id);
        BO.LineStation GetLineStation(int lineId, int stationId);
        void AddLineStation(BO.LineStation lineStation);
        void UpdateLineStation(BO.LineStation lineStation);
        void DeleteLineStation(BO.LineStation lineStation);
        void DeleteLineStation(int id);

        #endregion

        #region LineTrip
        IEnumerable<LineTrip> GetAllLineTrips();
        IEnumerable<LineTrip> GetAllLineTripsBy(Predicate<BO.LineTrip> predicate);
        LineTrip GetLineTrip(int lineId, TimeSpan startTime);
        void AddLineTrip(BO.LineTrip lineTrip);
        void UpdateLineTrip(BO.LineTrip lineTrip);
        void DeleteLineTrip(BO.LineTrip lineTrip);
        #endregion

        #region User
        User GetUser(string id, string pwd);
        #endregion

        IEnumerable<LineTiming> ListArrivalOfLine(int lineId, TimeSpan hour, int stationKey);
        IEnumerable<IGrouping<TimeSpan, LineTiming>> StationTiming(BO.BusStation station, TimeSpan hour);
    }
}