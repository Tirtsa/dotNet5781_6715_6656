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
        IEnumerable<BO.BusLine> GetBusLines();
        BO.BusLine GetBusLine(int lineNumber, Areas area);
        void AddBusLine(BO.BusLine newLine);
        void EditBusLine(BO.BusLine line);
        void EditBusLine(int id, Action<DO.BusLine> action);
        void DeleteBusLine(int id);

        IEnumerable<BO.BusStation> GetBusStations();
        BO.BusStation GetBusStation(int key);
        void AddStation(BO.BusStation station);
        void EditBusStation(BO.BusStation station);
        void EditBusStation(int key, Action<DO.BusStation> action);
        void DeleteStation(int key);
    }
}