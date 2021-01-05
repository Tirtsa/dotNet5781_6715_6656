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

		readonly IDAL dl = DalFactory.GetDal();
		public void AddBusLine(BO.BusLine newLine)
		{
			//int id = dl.AddLine();//call to converter BO.BusLine to DO.BusLine
			if (DataSource.ListLines.FirstOrDefault(line => line.Id == newLine.Id) != null)
				throw new ArgumentException("This bus line already exists");
			//DataSource.ListLines.Add(newLine);
			//"for" on list of stations and add linestationId using the id of newline(l.18) 
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