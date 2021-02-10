using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class LineTrip
    {
        public int LineId{ get; set; }
        public TimeSpan StartTimeRange { get; set; }
        public TimeSpan EndTimeRange { get; set; }
        public TimeSpan Frequency { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}
