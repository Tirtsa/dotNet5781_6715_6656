using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace APIDL

{
    //CRUD Logic:
    // Create - add new instance
    // Request - ask for an instance or for a collection
    // Update - update properties of an instance
    // Delete - delete an instance
    public interface IDAL
    {

        #region Station
        IEnumerable<DO.BusStation> GetAllStations();
        IEnumerable<DO.BusStation> GetAllStationsBy(Predicate<DO.BusStation> predicate);
        DO.BusStation GetStation(int id);
        void AddStation(DO.BusStation station);
        void UpdateStation(DO.BusStation station);
        void UpdateStation(int id, Action<DO.BusStation> update);
        void DeleteStation(int id);
        #endregion

        #region BusLine
        IEnumerable<DO.BusLine> GetAllLines();
        IEnumerable<DO.BusLine> GetAllLinesBy(Predicate<DO.BusLine> predicate);
        DO.BusLine GetLine(int id);
        void AddLine(DO.BusLine line);
        void UpdateLine(DO.BusLine line);
        void UpdateLine(int id, Action<DO.BusLine> update);
        void DeleteLine(int id);
        #endregion

        #region LineStation
        IEnumerable<DO.LineStation> GetAllLineStations();
        IEnumerable<DO.LineStation> GetAllLineStationsBy(Predicate<DO.LineStation> predicate);
        DO.LineStation GetLineStation(int id);
        void AddLineStation(DO.LineStation lineStation);
        void UpdateLineStation(DO.LineStation station);
        void UpdateLineStation(int id, Action<DO.LineStation> update);
        void DeleteLineStation(int id);
        #endregion

        #region FollowingStations
        IEnumerable<DO.FollowingStations> GetAllFollowingStations();
        IEnumerable<DO.FollowingStations> GetAllFollowingStationsBy(Predicate<DO.FollowingStations> predicate);
        DO.FollowingStations GetFollowingStations(int id);
        void AddFollowingStations(DO.BusStation station1, DO.BusStation station2);
        void UpdateFollowingStations(DO.BusStation station1, DO.BusStation station2);
        void UpdateFollowingStations(int id, Action<DO.FollowingStations> update);
        void DeleteFollowingStations(int id);
        #endregion
    }
}
