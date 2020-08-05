using System;
using System.Collections.Generic;
using System.Text;

namespace TukiTaki_KenaKata
{
    class Helper
    {
        static public int ReadSafeInt()
        {
            int a;
            bool success = int.TryParse(Console.ReadLine().Trim(), out a);
            if (success)
            {
                return a;
            }
            else
            {
                Console.WriteLine("You didn't enter an int, Program Will now exit");
                Console.ReadKey();
                Environment.Exit(0);
                throw new DataMisalignedException("Format doesn't match");
            }
        }
        static public double ReadSafeDouble()
        {
            double a;
            bool success = double.TryParse(Console.ReadLine().Trim(), out a);
            if (success)
            {
                return a;
            }
            else
            {
                Console.WriteLine("You didn't enter a double, Program Will now exit");
                Console.ReadKey();
                Environment.Exit(0);
                throw new DataMisalignedException("Format doesn't match");
            }
        }
    }
}
