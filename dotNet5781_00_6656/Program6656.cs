using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_00_6656
{
	partial class Program
	{
		static void Main()
		{
			Welcome6656();
			Welcome1234();
			Console.ReadKey();
		}

		static partial void Welcome1234();

		private static void Welcome6656()
		{
			Console.WriteLine("Enter your name: ");
			string name = Console.ReadLine();
			Console.WriteLine($"{name}, welcome to my first console application");
		}
	}
}
