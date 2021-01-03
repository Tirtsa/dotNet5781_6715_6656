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
        IEnumerable<DO.BusStation> GetAllLines();
        IEnumerable<DO.BusStation> GetAllLinesBy(Predicate<DO.BusStation> predicate);
        DO.BusStation GetLine(int id);
        void AddLine(DO.BusLine line);
        void UpdateLine(DO.BusStation station);
        void UpdateLine(int id, Action<DO.BusStation> update);
        void DeleteLine(int id);
        #endregion

        #region LineStation
        IEnumerable<DO.BusStation> GetAllLineStations();
        IEnumerable<DO.BusStation> GetAllLineStationsBy(Predicate<DO.BusStation> predicate);
        DO.BusStation GetLineStation(int id);
        void AddLineStation(DO.LineStation lineStation);
        void UpdateLineStation(DO.BusStation station);
        void UpdateLineStation(int id, Action<DO.BusStation> update);
        void DeleteLineStation(int id);
        #endregion

        #region FollowingStations
        IEnumerable<DO.FollowingStations> GetAllFollowingStations();
        IEnumerable<DO.FollowingStations> GetAllFollowingStationsBy(Predicate<DO.BusStation> predicate);
        DO.FollowingStations GetFollowingStations(int id);
        void AddFollowingStations(DO.BusStation station1, DO.BusStation station2);
        void UpdateFollowingStations(DO.BusStation station);
        void UpdateFollowingStations(int id, Action<DO.BusStation> update);
        void DeleteFollowingStations(int id);
        #endregion
    }
}
