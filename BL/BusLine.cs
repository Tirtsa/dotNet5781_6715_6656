using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace BO
{
	public class BusLine
	{
		BusStation Station;
        ObservableCollection<BusStation> busStations { get; set; }
		DateTime ArrivalTime;
		//tostring
	}
}
