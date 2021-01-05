using System;
using BLApi;
using APIDL;
using DS;
using BO;
using DO;
using System.Linq;

namespace BL
{
    public class BLIMP : IBL
    {
		//static Random rnd = new Random(DateTime.Now.Millisecond);

		readonly IBL bl = BlFactory.GetBL();
		public void AddBusLine(BO.BusLine newLine)
		{
			if (DataSource.ListLines.FirstOrDefault(line => line.Id == newLine.Id) != null)
				throw new ArgumentException("This bus line already exists");
			//DataSource.ListLines.Add(newLine);
		}

		public void AddStation(BO.BusStation station)
		{
			throw new NotImplementedException();
		}

		public void DeleteBusLine(int id)
		{
			throw new NotImplementedException();
		}

		public void DeleteStation(int id)
		{
			throw new NotImplementedException();
		}

		public void EditBusLine(BO.BusLine line, BO.BusStation station)
		{
			throw new NotImplementedException();
		}

		public void EditStation(BO.BusStation station)
		{
			throw new NotImplementedException();
		}

		public BO.BusLine GetBusLine(int id)
		{
			throw new NotImplementedException();
			//return from temp in BusLine.
			//where predicate(temp)
			//select temp.Clone();
		}

		public BO.BusStation GetBusStation(int key)
		{
			throw new NotImplementedException();
		}
	}
}