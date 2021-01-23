using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_6656
{
	enum Month { January = 1, February, March, April, May, June, July, August, September, October, November, December }
	class Bus
	{
		readonly Month m_startMonth;
		int m_license, m_startDay, m_startYear;

		Bus(int license, int day, Month month, int year)
		{
			m_license = license;
			m_startDay = day;
			m_startMonth = month;
			m_startYear = year;			
		}

		//void updateMaintenance(); //changes date when maintenance is performed
		//void addMilage(); //counts total, fuel and tune up km over time
		//void resetTuneUp(); //when bus goes for maintenance it puts the tune up km counter back to 0
		//void resetRefuel();//when bus is refueled, it resets the km counter
		//bool needsMaintenance(); //determines if the bus will reach 20,000 km on next trip or it has been a year since last maintenance check
		//bool needsRefuel(); //determines if the bus will reach 1,200 (since last refuel) on next trip

	}
	class Program
	{
		static void AddBus()
		{
			int test = 0, day = 0, year = 0, license;
			string monthStr;
			Month month = 0;
			bool invalid = true;

			Console.WriteLine("Enter the start of use date for the bus");
			Console.WriteLine("month? (written out fully with capital first letter)");
			while (invalid)
			{
				monthStr = Console.ReadLine();
				invalid = !Enum.IsDefined(typeof(Month), monthStr);
				if (invalid)
					Console.WriteLine("the entered month is invalid, enter a correct month");
				else
					Enum.TryParse(monthStr, out month);
			}
			
			invalid = true;
			Console.WriteLine("year?");
			while (invalid)
			{
				test = int.Parse(Console.ReadLine());
				if (test < 1900 || test > DateTime.Now.Year)
					Console.WriteLine("the entered year is invalid, enter a correct year from 1900 to today");
				else
				{
					year = test;
					invalid = false;
				}
			}
			
			invalid = true;
			Console.WriteLine("day?");
			while (invalid)
			{
				test = int.Parse(Console.ReadLine());
				if (test < 1 || test > 31)
				{
					Console.WriteLine("the day of the month you entered in invalid, enter a number from 1-31");
					continue;
				}

				switch (test)
				{
					case 29:
						if (month == Month.February)
							if (year % 400 == 0 || (year % 4 == 0 && year % 100 != 0))
								invalid = false;
							else
							{
								Console.WriteLine("the day of the month you entered in invalid, enter a number from 1-28");
								test = 0;
							}
						else
							invalid = false;
						break;

					case 30:
						if (month == Month.February)
						{
							Console.WriteLine("the day of the month you entered in invalid, enter a number from 1-29");
							test = 0;
						}
						else
							invalid = false;
						break;

					case 31:
						if (month == Month.February || month == Month.September || month == Month.April || month == Month.June || month == Month.November)
						{
							Console.WriteLine("the day of the month you entered in invalid, enter a number from 1-30");
							test = 0;
						}
						else
							invalid = false;
						break;

					default:
						invalid = false;
						break;
				}
			}
			day = test;

			invalid = true;
			string licenseStr = "";
			while (invalid)
			{
				Console.WriteLine("Enter the license number of the bus you would like to add only in numbers, without dashes");
				licenseStr = Console.ReadLine();

				if ((year < 2018 && licenseStr.Length != 7) || (year > 2018 && licenseStr.Length != 8))///////////////// 2018
					Console.WriteLine("The license number is not a valid length");
				else
					invalid = false;
			}
			license = int.Parse(licenseStr);

			//Bus bus(license, day, month, year);
		}
		static void Main(string[] args)
		{
			List<Bus> buses = new List<Bus>();

			int answer = -1;
			while (answer != 4)
			{
				Console.WriteLine("Please select an option from the menu:\n0 - Add a bus\n1 - Choose a bus for travel\n2 - Refuel or tune up a bus\n3 - Display the distance since last maintenance for all buses\n4 - Exit");
				answer = int.Parse(Console.ReadLine());

				switch (answer)
				{
					case 0:
						AddBus();
						break;
					case 1:
						//travel();
						break;
					case 2:
						//refuel
						//tune up
						break;
					case 3:
						//list
						break;
					case 4:
						break;
					default:
						Console.WriteLine("You have entered an invalid number, please try again.");
						break;
				}
			}
			Console.WriteLine("Goodbye!\nPress any key to close the console...");
			Console.ReadKey();
		}
	}
}
