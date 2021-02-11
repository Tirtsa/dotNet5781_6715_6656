using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLApi;
using BO;

namespace PO
{
    class LineTiming
    {
        public int LineId { get; set; }
        public int LineNumber { get; set; }
        public string LastStation { get; set; }
        public TimeSpan TripStart { get; set; }
        public TimeSpan Timing { get; set; }
    }

    public class PL
    {
        static IBL bl = BlFactory.GetBL();

        internal IEnumerable<IGrouping<TimeSpan, LineTiming>> BoPoLineTimingAdapter
            (IEnumerable<IGrouping<TimeSpan, BO.LineTiming>> listTiming, TimeSpan hour)
        {
            List<LineTiming> timing = new List<LineTiming>();
            foreach (var element in listTiming)
            {
                foreach(var item in element)
                {
                    BusLine line = bl.GetBusLine(item.LineId);
                    timing.Add(new LineTiming {
                        LineId = item.LineId,
                        LineNumber = line.BusLineNumber,
                        LastStation = bl.GetBusStation(line.LastStationKey).StationName,
                        TripStart = item.TripStart,
                        Timing = item.ExpectedTimeTillArrive - hour
                    });
                } 
            }

            return from item in timing
                   group item by item.Timing;
        }
    }

}
