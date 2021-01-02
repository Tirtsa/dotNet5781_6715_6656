﻿using System;
using BLApi;
using APIDL;
//using DL;
using BO;
using DO;

namespace BL
{
    public class BLIMP : IBL
    {
        static Random rnd = new Random(DateTime.Now.Millisecond);

        readonly IDAL dal = DalFactory.GetDal();

        public Weather GetWeather(int day)
        {
            Weather w = new Weather();
            double feeling;
            //WindDirections dir;
            RunningNumber number;

            //feeling = dal.GetTemparture(day);
            //number = dal.GetRunningNumber.Number;

            /*switch (number)
            {
                case RunningNumbers.start:
                    feeling += 2;
                    break;
                default:
                    break;
            }
            w.Feeling = (int)feeling;*/
            return w;
        }


    }
}