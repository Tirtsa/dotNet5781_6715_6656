using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_03B_6715_5227
{
    public class Bus
    {
        public enum Status { Ready, Travelling, Refueling, underMaintenance };
        public string Immatriculation { get; set; }
        DateTime dateStart;
        public DateTime DateStart { get { return dateStart.Date; } set { dateStart = value; } }

        float kilometrage = 0;
        public float Kilometrage { get; set; }
        public float KmOfFuel { get; set; } //how much km can the bus drive with its gasoline
        public DateTime MaintenanceDate { get; set; }
        public float MaintenanceKm { get; set; }

        Status busStatus = Status.Ready;
        public Status BusStatus { get; set; }

        /// <summary>
        /// Dangerous - return if bus can do travel or is 'dangerous'
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public bool Dangerous(int km)
        {
            if ((MaintenanceKm + km) > 20000 || (DateTime.Now - MaintenanceDate).Days >= 365)
                return true;
            else
                return false;
        }

        /// <summary>
        /// addTravel - add kms to kilometrage and diminue of kmoffuel
        /// </summary>
        /// <param name="km"></param>
        public void addTravel(int km)
        {
            this.kilometrage += km;
            this.KmOfFuel -= km;
        }

        //ctor
        public Bus() { }
        public override string ToString()
        {
            if (DateStart.Year < 2018)
                return Immatriculation.Substring(0, 2) + "-" + Immatriculation.Substring(2, 3) + "-" +
                    Immatriculation.Substring(5);
            else
                return Immatriculation.Substring(0, 3) + "-" + Immatriculation.Substring(3, 2) + "-" +
                    Immatriculation.Substring(5);
        }

    }
}
