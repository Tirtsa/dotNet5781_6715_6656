using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using APIDL;


namespace DS
{
    public static class DataSource
    {
        public static List<RunningNumber> idNumber;

        static DataSource()
        {
            idNumber = new List<RunningNumber>();
            idNumber.Add(new RunningNumber());
        }
    }
}