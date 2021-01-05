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
        BusLine GetBusLine(int id);
        void AddBusLine(BusLine newLine);
        void EditBusLine(BusLine line, BusStation station);
        void DeleteBusLine(int id);

        BusStation GetBusStation(int key);
        void AddStation(BusStation station);
        void EditStation(BusStation station);
        void DeleteStation(int id);
    }
}