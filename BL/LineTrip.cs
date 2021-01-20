using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class LineTrip
    {
        public int Id { get; set; }
        public int LineNumber { get; set; }
        public int LineId { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public string Destination { get; set; }
        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}
