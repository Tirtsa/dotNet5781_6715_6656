using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
	public enum RunningNumbers { start, edit, delete, travel}
	public class RunningNumber : IClonable
	{
		public RunningNumber idNumber { get; set; }
	}
}
