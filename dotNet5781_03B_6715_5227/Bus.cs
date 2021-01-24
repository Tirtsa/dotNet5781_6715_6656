using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_03B_6715_5227
{
    public class Bus
    {
        public enum Status { Ready, Travelling, Refueling, underMaintenance,};
        string immatriculation;
        public string Immatriculation 
        {
            get
            {
                if (immatriculation == null)
                    return "";
                if (DateStart.Year < 2018)
                    return immatriculation.Substring(0, 2) + "-" + immatriculation.Substring(2, 3) + "-" +
                        immatriculation.Substring(5);
                else
                    return immatriculation.Substring(0, 3) + "-" + immatriculation.Substring(3, 2) + "-" +
                        immatriculation.Substring(5);
            }
            set
            {
                immatriculation = value;
            }
        }
        DateTime dateStart;
        public DateTime DateStart { get { return dateStart.Date; } set { dateStart = value; } }

        float kilometrage = 0;
        public float Kilometrage { get; set; }
        public float KmOfFuel { get; set; } //how much km can the bus drive with its gasoline
        public DateTime MaintenanceDate { get; set; }
        public float MaintenanceKm { get; set; }

        Status busStatus = Status.Ready;
        public string BusStatus 
        { 
            get
            {
                return GetStatus();
            }
            set
            {
                SetStatus(value);
            }
        }
        private string GetStatus()
        {
            string status = "";
            switch (busStatus)
            {
                case Status.Ready:
                    status = "מוכן לנסיעה";
                    break;
                case Status.Refueling:
                    status = "בתידלוק";
                    break;
                case Status.Travelling:
                    status = "באמצע נסיעה";
                    break;
                case Status.underMaintenance:
                    status = "בטיפול";
                    break;
                default:
                    break;
            }
            if ((DateTime.Now - MaintenanceDate).Days >= 365 || MaintenanceKm >= 20000)
                status = "מסוכן";
            return status;
        }

        private void SetStatus(string status)
        {
            
            switch (status)
            {
                case "מוכן לנסיעה":
                    busStatus = Status.Ready;
                    break;
                case "בתידלוק" :
                    busStatus = Status.Refueling;
                    break;
                case "באמצע נסיעה" :
                    busStatus = Status.Travelling;
                    break;
                case "בטיפול":
                    busStatus = Status.underMaintenance;
                    break;
                default:
                    break;
            }
        }


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
            if (Dangerous(km) == true)
                throw new ArgumentException("אוטובוס " + this.Immatriculation + " מוגדר כמסוכן. נא לשלחו לטיפול בהקדם");
            if (KmOfFuel < km)
                throw new ArgumentException("לאוטובוס " + this.Immatriculation + " אין מספיק דלק לנסיעה. נא לבצע תידלוק");
            if (this.busStatus != Status.Ready)
                throw new ArgumentException("אוטובוס " + this.Immatriculation + " אינו יכול לנסוע, הוא כבר " + 
                    this.GetStatus());
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
