using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace APIDL

{
    public interface IDAL
    {
        double GetTemparture(int day);
        WindDirection GetWindDirection(int day);

    }
}
