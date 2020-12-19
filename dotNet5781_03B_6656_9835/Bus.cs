using System;
using System.Windows.Media;

namespace dotNet5781_03B_6656_9835
{
	class Bus
	{
		private DateTime m_startDate, m_maintenanceDate = DateTime.Today;
		private uint m_seats, m_totalDistance, m_distanceSinceMaintenance, m_distanceSinceRefuel;
		private int m_license, m_status;
		private string m_driverName;
		private Brush m_color;

		public Bus() { }
		public Bus(int license, int day, Month month, int year, uint totDist)
		{
			License = license;
			TotalDistance = totDist;
			m_startDate = new DateTime(year, (int)month, day);
			m_distanceSinceMaintenance = 0;
			m_distanceSinceRefuel = 0;
			m_status = 0;
			m_driverName = "Unknown";
			int rnd = new Random().Next(25, 50);
			m_seats = (uint)rnd;
		}
		public DateTime StartDate { get => m_startDate; }
		public string StartDateStr { get { return m_startDate.ToLongDateString(); } }
		public uint DistanceSinceMaintenance { get => m_distanceSinceMaintenance; set => m_distanceSinceMaintenance = value; } //checks how many km since last maintenance was performed
		public string LastServiceDistStr { get { return m_distanceSinceMaintenance.ToString(); } }
		public DateTime MaintenanceDate { get => m_maintenanceDate; set => m_maintenanceDate = value; } //checks or sets date when maintenance was last performed
		public string LastServiceDateStr { get { return m_maintenanceDate.ToLongDateString(); } }
		public uint DistanceSinceRefuel { get => m_distanceSinceRefuel; set => m_distanceSinceRefuel = value; }  //checks or sets last time bus was sent to refuel
		public string DistUntilRefuelStr { get { int temp = 1200 - (int)m_distanceSinceRefuel; return temp.ToString(); } }
		public int Status { get => m_status; set => m_status = value; }//0 == ready, 1 == traveling, 2 == refueling, 3 == being serviced
		public int License { get => m_license; private set => m_license = value; }
		public uint TotalDistance { get => m_totalDistance; set => m_totalDistance = value; }
		public string LicenseStr { get { return m_license.ToString(); } }
		public string DriverName { get => m_driverName; set => m_driverName = value; }
		public override string ToString()
		{
			return "License:  " + LicenseStr
				+ "\nDriver's name:  " + DriverName
				+ "\nAmount of seats on bus: " + m_seats
				+ "\nTotal distance driven:  " + TotalDistance
				+ "\nStart date:  " + StartDateStr
				+ "\nDate of last servicing:  " + LastServiceDateStr
				+ "\nDistance since last servicing:  " + LastServiceDistStr
				+ "\nDistance left until refueling needed:  " + DistUntilRefuelStr;
		}
		public Brush StatusColor
		{
			get => m_color;

			set
			{
				switch (m_status)
				{
					case 0:
						m_color = Brushes.YellowGreen;
						break;
					case 1:
						m_color = Brushes.DodgerBlue;
						break;
					case 2:
						m_color = Brushes.Orange;
						break;
					case 3:
						m_color = Brushes.Crimson;
						break;
				}
			}
		}
	}
}