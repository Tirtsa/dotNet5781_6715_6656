using System;
using BlApi;
using APIDL;
//using DL;
using BO;
using DO;

namespace BL
{
    public class BlImp1 : IBL
    {
        static Random rnd = new Random(DateTime.Now.Millisecond);

        readonly IDAL dal = DalFactory.GetDal();

        public Weather GetWeather(int day)
        {
            Weather w = new Weather();
            double feeling;
            WindDirections dir;


            feeling = dal.GetTemparture(day);
            dir = dal.GetWindDirection(day).direction;

            switch (dir)
            {
                case WindDirections.S:
                    feeling += 2;
                    break;
                case WindDirections.SSE:
                    feeling += 1.5;
                    break;
                case WindDirections.SE:
                    feeling += 1;
                    break;
                case WindDirections.SEE:
                    feeling += 0.5;
                    break;
                case WindDirections.E:
                    feeling -= 0.5;
                    break;
                case WindDirections.NEE:
                    feeling -= 1;
                    break;
                case WindDirections.NE:
                    feeling -= 1.5;
                    break;
                case WindDirections.NNE:
                    feeling -= 2;
                    break;
                case WindDirections.N:
                    feeling -= 3;
                    break;
                case WindDirections.NNW:
                    feeling -= 2.5;
                    break;
                case WindDirections.NW:
                    feeling -= 2;
                    break;
                case WindDirections.NWW:
                    feeling -= 1.5;
                    break;
                case WindDirections.W:
                    feeling -= 1;
                    break;
                case WindDirections.SWW:
                    feeling -= 0;
                    break;
                case WindDirections.SW:
                    break;
                case WindDirections.SSW:
                    feeling += 1;
                    break;
            }
            w.Feeling = (int)feeling;
            return w;
        }


    }
}