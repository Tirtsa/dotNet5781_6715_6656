using System;
using BLApi;
using BO;


namespace PlConsole
{
    class Program
    {
        static IBL bl;
        //static IDAL dl

        static void Main(string[] args)
        {
            bl = BlFactory.GetBL();
            //dl = DalFactory.GetDal();
            //dl.AddLine(new BusLine {BusLineNumber = 1, Area = General, FisrtStationKey = 83831, Last
            //var myList = dl.GetAllLine
            //foreach item in myList ( console.writeLine(item)....
            
            Console.ReadKey();
        }
    }
}