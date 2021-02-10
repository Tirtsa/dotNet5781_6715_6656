using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class DuplicateLineException : Exception
    {
        public int lineNumber;
        public Areas area;
        public DuplicateLineException(string message, Exception innerException) :
            base(message, innerException)
        { 
            lineNumber = ((DO.DuplicateLineException)innerException).lineNumber;
            area = (Areas)((DO.DuplicateLineException)innerException).area;
        }

        public override string ToString() => base.ToString() + $", Duplicate line {lineNumber} in {area}";

    }

    public class DuplicateStationException : Exception
    {
        public int Id;
        public DuplicateStationException(string message, Exception innerException) :
            base(message, innerException) => Id = ((DO.DuplicateStationException)innerException).Id;

        public override string ToString() => base.ToString() + $", Duplicate Station {Id}";
    }

    public class InexistantLineException : Exception
    {
        public int lineNumber;
        public Areas area;
        public InexistantLineException(string message, Exception innerException) :
            base(message, innerException)
        {
            lineNumber = ((DO.DuplicateLineException)innerException).lineNumber;
            area = (Areas)((DO.DuplicateLineException)innerException).area;
        }

        public override string ToString() => base.ToString() + $", There is no line {lineNumber} in {area}";

    }

    public class InexistantStationException : Exception
    {
        public int Id;
        public InexistantStationException(string message, Exception innerException) :
            base(message, innerException) => Id = ((DO.DuplicateStationException)innerException).Id;

        public override string ToString() => base.ToString() + $", There is no station {Id}";
    }
}
