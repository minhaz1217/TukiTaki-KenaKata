using Cassandra;
using System;
using TukiTaki_KenaKata.model;
using TukiTaki_KenaKata.presentation;

namespace TukiTaki_KenaKata
{
    class Program
    {
        static void Main(string[] args)
        {
            Home home = new Home();
            //Cluster cluster = Cluster.Builder().AddContactPoints("localhost").Build();
            //// Connect to the nodes using a keyspace
            //Session session = (Session)cluster.Connect("tukitaki_kenakata");
            ////6582fa38-8d96-4d32-907d-85a8f75a95a4
            //PreparedStatement ps = session.Prepare("UPDATE product SET name=? WHERE id=?");
            //BoundStatement statement = ps.Bind("myName", Guid.Parse("6582fa39-8d96-4d32-907d-85a8f75a95a4").ToString());
            //RowSet rowSet = session.Execute(statement);
            //Console.WriteLine(session.GetType());
            //Console.WriteLine(rowSet.GetRows().ToString());
            //foreach(Row row in rowSet)
            //{
            //    Console.WriteLine(new Product(row).ToString());
            //}
            ////Console.WriteLine($"Created {x.Columns} {x.GetType()} {x.Info.IsSchemaInAgreement}");
            //Console.WriteLine("End");
            //Console.ReadKey();
            home.ShowMainMenu();
        }
    }
}
