using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using DO;

namespace BLApi
{
    public interface IBL
    {
        #region BusLine
        IEnumerable<BO.BusLine> GetBusLines();
        BO.BusLine GetBusLine(int lineNumber, Areas area);
        void AddBusLine(BO.BusLine newLine);
        void EditBusLine(BO.BusLine line);
        void EditBusLine(int id, Action<DO.BusLine> action);
        void DeleteBusLine(int id);
        #endregion

        #region BusStation
        IEnumerable<BO.BusStation> GetAllBusStations();
        BO.BusStation GetBusStation(int key);
        void AddStation(BO.BusStation station);
        void UpdateBusStation(BO.BusStation station);
        void UpdateBusStation(int key, Action<BO.BusStation> action);
        void DeleteStation(int key);
        #endregion
    }
}