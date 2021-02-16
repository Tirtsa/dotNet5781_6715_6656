using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    #region Station
    public class DuplicateStationException : Exception
    {
        public int Id;
        public DuplicateStationException(int id) : base() => Id = id;
        public DuplicateStationException(int id, string message) :
            base(message) => Id = id;
        public DuplicateStationException(int id, string message, Exception innerException) :
            base(message, innerException) => Id = id;

        public override string ToString() => base.ToString() + $", Duplicate Station {Id}";
    }
    public class InexistantStationException : Exception
    {
        public int Id;
        public InexistantStationException(int id) : base() => Id = id;
        public InexistantStationException(int id, string message) :
            base(message) => Id = id;
        public InexistantStationException(int id, string message, Exception innerException) :
            base(message, innerException) => Id = id;

        public override string ToString() => base.ToString() + $", There is no station {Id}";
    }
    #endregion

    #region Line
    public class DuplicateLineException : Exception
    {
        public int lineNumber;
        public Areas area;
        public DuplicateLineException(int lineNum, Areas lineArea) : base() { lineNumber = lineNum; area = lineArea; }
        public DuplicateLineException(int lineNum, Areas lineArea, string message) :
            base(message)
        { lineNumber = lineNum; area = lineArea; }
        public DuplicateLineException(int lineNum, Areas lineArea, string message, Exception innerException) :
            base(message, innerException)
        { lineNumber = lineNum; area = lineArea; }

        public override string ToString() => base.ToString() + $", Duplicate line {lineNumber} in {area}";

    }
    public class InexistantLineException : Exception
    {
        public int lineNumber;
        public Areas area;
        public int Id;
        public InexistantLineException(int id) : base() => Id = id;
        public InexistantLineException(int lineNum, Areas lineArea) : base() { lineNumber = lineNum; area = lineArea; }
        public InexistantLineException(int lineNum, Areas lineArea, string message) :
            base(message)
        { lineNumber = lineNum; area = lineArea; }
        public InexistantLineException(int lineNum, Areas lineArea, string message, Exception innerException) :
            base(message, innerException)
        { lineNumber = lineNum; area = lineArea; }

        public override string ToString() 
        { 
            if (Id == 0)
                return base.ToString() + $", There is no line {lineNumber} in {area}";
            else
                return base.ToString() + $", There is no line with id {Id}";
        }

    }
    #endregion

    #region LineTrip
    public class DuplicateLineTripException : Exception
    {
        public int Id;
        public TimeSpan startHour;
        public DuplicateLineTripException(int id, TimeSpan start) : base() { Id = id; startHour = start; }
        public DuplicateLineTripException(int id, TimeSpan start, string message) :
            base(message)
        { Id = id; startHour = start; }
        public DuplicateLineTripException(int id, TimeSpan start, string message, Exception innerException) :
            base(message, innerException)
        { Id = id; startHour = start; }

        public override string ToString() => base.ToString() + $", כבר רשומים יציאות לקו  {Id} עבור שעה {startHour}";

    }
    public class InexistantLineTripException : Exception
    {
        public int Id;
        public TimeSpan startHour;
        public InexistantLineTripException(int id, TimeSpan start) : base() { Id = id; startHour = start; }
        public InexistantLineTripException(int id, TimeSpan start, string message) :
            base(message)
        { Id = id; startHour = start; }
        public InexistantLineTripException(int id, TimeSpan start, string message, Exception innerException) :
            base(message, innerException)
        { Id = id; startHour = start; }

        public override string ToString() => base.ToString() + $", לא נמצא מידע על יציאות קו {Id} בשעות אלו : {startHour}";
    }
    #endregion

    #region User
    public class InexistantUserException : Exception
    {
        public string Id;
        public string Password;

        public InexistantUserException(string id, string pwd) : base() { Id = id; Password = pwd; }
        public InexistantUserException(string id, string pwd, string message) :
            base(message)
        { Id = id; Password = pwd; }
        public InexistantUserException(string id, string pwd, string message, Exception innerException) :
            base(message, innerException)
        { Id = id; Password = pwd; }

        public override string ToString()
        {
            return base.ToString() + $", לא קיים משתמש עם הסיסמא והשם משתמש שהוזנו";
        }
    }
        #endregion

        #region XML
        public class XMLFileLoadCreateException : Exception
    {
        public string xmlFilePath;
        public XMLFileLoadCreateException(string xmlPath) : base() { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message) :
            base(message)
        { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message, Exception innerException) :
            base(message, innerException)
        { xmlFilePath = xmlPath; }

        public override string ToString() => base.ToString() + $", fail to load or create xml file: {xmlFilePath}";
    }
    #endregion

}
