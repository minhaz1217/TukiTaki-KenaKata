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
        static public Guid SafeGuidParse(string choice)
        {
            Guid productId;
            if(Guid.TryParse(choice, out productId))
            {
                return productId;
            }
            else
            {
                return new Guid();
            }
        }
        public static void MyPrint(string variable, string color = "r")
        {
            Dictionary<string, int> colors = new Dictionary<string, int>();
            colors["r"] = 31;
            colors["g"] = 32;
            Console.WriteLine($"\x1b[{colors[color]}m\x1b[1m{variable}\x1b[0m");
        }
        public static string MyOutputString(string variable, string color = "r")
        {
            Dictionary<string, int> colors = new Dictionary<string, int>();
            colors["r"] = 31;
            colors["g"] = 32;
            colors["y"] = 33;
            return ($"\x1b[{colors[color]}m\x1b[1m{variable}\x1b[0m");
        }
    }
}
