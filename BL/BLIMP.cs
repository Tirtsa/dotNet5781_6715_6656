using System;
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
    }
}