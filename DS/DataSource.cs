using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;


namespace DS
{
    public static class DataSource
    {
        public static List<WindDirection> directions;

        static DataSource()
        {
            directions = new List<WindDirection>();
            directions.Add(new WindDirection());
        }
    }
}