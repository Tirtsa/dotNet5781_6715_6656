using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO

{

    // The class represents a bus (of certain line) arriving soon to the bus station

    public class LineTiming

    {

        private static int counter = 0;

        public int ID;

        public LineTiming() => ID = ++counter; //unique

        public TimeSpan TripStart { get; set; } //time of Ine start the trip, taken from StartAt of LineTrip

        public int LineId { get; set; } //Line ID from Line

        public TimeSpan ExpectedTimeTillArrive { get; set; }//Expected time of arrival

        public override string ToString()
        {
            return this.ToStringProperty();
        }

    }

}
