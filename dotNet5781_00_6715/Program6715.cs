using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_00_6715
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome6715();
            Welcome6715bis();
            Console.ReadKey();
        }

        static partial void Welcome6715bis();

        private static void Welcome6715()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }
    }
}
