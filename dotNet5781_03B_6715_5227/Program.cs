using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace dotNet5781_03B_6715_5227
{
    class Program
    {
        public static Random r = new Random(DateTime.Now.Millisecond);
      

        public class Bus
        {
            public string Immatriculation
            {
                get
                {
                    //return immatriculation in format 12-345-67 or 123-45-678
                    Console.WriteLine(DateStart.Year);
                    if (DateStart.Year < 2018)
                        return Immatriculation.Substring(0, 2) + "-" + Immatriculation.Substring(2, 3) + "-" +
                            Immatriculation.Substring(5);
                    else
                        return Immatriculation.Substring(0, 3) + "-" + Immatriculation.Substring(3, 2) + "-" +
                            Immatriculation.Substring(5);
                }
                set { Immatriculation = value; }
            }
            public DateTime DateStart { get; set; }

            float kilometrage = 0;
            public float Kilometrage { get; }
            public float KmOfFuel { get; set; } //how much km can the bus drive with its gasoline
            public DateTime MaintenanceDate { get; set; }
            public float MaintenanceKm { get; set; }

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

        }



        static void Main(string[] args)
        {
            List<Bus> buses = new List<Bus>(); //create empty list of buses

            addBus();




            /// <summary>
            /// help function,add bus to list
            /// </summary>
            void addBus()
            {

            }
        }
    }
}
