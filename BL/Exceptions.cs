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

    public class DuplicateLineTripException : Exception
    {
        public int Id;
        public TimeSpan startHour;
        public DuplicateLineTripException(string message, Exception innerException) : 
            base(message, innerException) 
        {
            Id = ((DO.DuplicateLineTripException)innerException).Id;
            startHour = ((DO.DuplicateLineTripException)innerException).startHour;
        }
        
        public override string ToString() => base.ToString() + $", כבר רשומים יציאות לקו  {Id} עבור שעה {startHour}";

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

    public class InexistantLineTripException : Exception
    {
        public int Id;
        public TimeSpan startHour;
        public InexistantLineTripException(string message, Exception innerException) : 
            base(message, innerException) 
        { 
            Id = ((DO.InexistantLineTripException)innerException).Id; 
            startHour = ((DO.InexistantLineTripException)innerException).startHour; 
        }
        public override string ToString() => base.ToString() + $", לא נמצא מידע על יציאות קו {Id} בשעות אלו : {startHour}";
    }

    public class InexistantStationException : Exception
    {
        public int Id;
        public InexistantStationException(string message, Exception innerException) :
            base(message, innerException) => Id = ((DO.DuplicateStationException)innerException).Id;

        public override string ToString() => base.ToString() + $", There is no station {Id}";
    }

    public class InexistantUserException : Exception
    {
        public string Id;
        public string Password;
        public InexistantUserException(string message, Exception innerException) :
            base(message, innerException)
        {
            Id = ((DO.InexistantUserException)innerException).Id;
            Password = ((DO.InexistantUserException)innerException).Password;
        }

        public override string ToString() => base.ToString() + $", לא קיים משתמש עם הסיסמא והשם משתמש שהוזנו";

    }
}
