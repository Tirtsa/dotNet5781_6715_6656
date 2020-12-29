using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
	//public enum RunningNumbers { start, edit, delete, travel}
	public class RunningNumber : IClonable
	{
		//public int id;
		//public RunningNumber idNumber
		public int idNumber
		{
			get => idNumber;
			set
			{
				if (value < 1000000)
					while (value < 1000000)
						value *= 10;
				else if (value > 99999999)
					while (value > 99999999)
						value /= 10;
				idNumber = value;
			}
		}
	}
}
