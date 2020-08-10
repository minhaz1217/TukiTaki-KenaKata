using Cassandra;
using System;
using System.IO;
using System.Text;
using TukiTaki_KenaKata.model;
using TukiTaki_KenaKata.persistant;
using TukiTaki_KenaKata.presentation;
using Autofac;
using System.Configuration;
namespace TukiTaki_KenaKata
{
    class Program
    {
        static void Main(string[] args)
        {
            IHome home = null;

            using (var scope = DependencyResolver.Instance().BeginLifetimeScope())
            {
                home = scope.Resolve<IHome>();
            }
            //DBController db = new DBController();
            //db.CheckCycle(Helper.SafeGuidParse("6ca697e6-8aa3-4536-8dc0-2ff6a68780ea"), Helper.SafeGuidParse("789cdd37-ecbc-4007-a059-55b2670b72f6"));
            Console.WriteLine("Hello World\nHI");

            home.ShowMainMenu();
        }
    }
}
