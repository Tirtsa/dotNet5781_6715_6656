using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DO
{
    public enum WindDirections { S, N, W, E, SE, SW, NE, NW, SSE, SEE, SSW, SWW, NNE, NEE, NNW, NWW }
    public class WindDirection //: IClonable
    {
        public WindDirections direction { get; set; }
    }
}
