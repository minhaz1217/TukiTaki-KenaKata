using Cassandra;
using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.model;

namespace TukiTaki_KenaKata.persistant
{
    class DBController : IDBController
    {
        private const string SERVER_NAME = "localhost";
        private const string KEYSPACE_NAME = "tukitaki_kenakata";
        private static Cluster cluster=null; 
        private static Session session=null;

        public DBController()
        {
            DBController.InitInstance();
        }
        private static void InitInstance()
        {
            if(cluster == null)
            {
                cluster = (Cluster)Cluster.Builder().AddContactPoints(SERVER_NAME).Build();
                session = (Session)cluster.Connect(KEYSPACE_NAME);
            }
            else
            {
                session = (Session)cluster.Connect(KEYSPACE_NAME);
            }
        }

        public long CheckProductCount(Guid productId)
        {
            PreparedStatement ps = session.Prepare("select count(id) as count from product where id=?;");
            BoundStatement statement = ps.Bind(productId.ToString());
            RowSet rowSet = session.Execute(statement);
            Console.WriteLine(session.GetType());
            foreach (Row row in rowSet)
            {
                return row.GetValue<long>("count");
            }
            return 0;
        }
        public List<Product> GetAllProduct()
        {
            List<Product> products = new List<Product>();
            dynamic rs = session.Execute("select * from product;");
            foreach (Cassandra.Row row in rs)
            {
                products.Add(new Product(row));

            }
            return products;
        }
        

        // This GetSingleProduct can return null if there is no product to return for that id;
        public Product GetSingleProduct(Guid productId)
        {

            PreparedStatement ps = session.Prepare("SELECT * FROM product where id=?;");
            BoundStatement statement = ps.Bind(Guid.Parse("6582fa38-8d96-4d32-907d-85a8f75a95a4").ToString());
            RowSet rowSet = session.Execute(statement);
            Console.WriteLine(session.GetType());
            foreach (Row row in rowSet)
            {
                return new Product(row);
            }
            return null;
        }
        public bool CreateProduct(string name, string description, double price)
        {
            dynamic ps = session.Prepare("INSERT INTO product(id, name, description,price) VALUES(?,?,?,?);");
            var statement = ps.Bind(Guid.NewGuid().ToString(), name, description, price);
            Cassandra.RowSet x = session.Execute(statement);
            return x.Info.IsSchemaInAgreement;
        }
        public bool ChangeProductName(Guid productId, string name)
        {
            PreparedStatement ps = session.Prepare("UPDATE product SET name=? WHERE id=?");
            BoundStatement statement = ps.Bind(name, productId.ToString());
            RowSet rowSet = session.Execute(statement);
            return true;
        }
        public bool ChangeProductDescription(Guid productId, string description)
        {
            PreparedStatement ps = session.Prepare("UPDATE product SET description=? WHERE id=?");
            BoundStatement statement = ps.Bind(description, productId.ToString());
            RowSet rowSet = session.Execute(statement);
            return true;
        }
        public bool ChangeProductPrice(Guid productId, double price)
        {
            PreparedStatement ps = session.Prepare("UPDATE product SET price=? WHERE id=?");
            BoundStatement statement = ps.Bind(price, productId.ToString());
            RowSet rowSet = session.Execute(statement);
            return true;
        }
        public bool DeleteProduct(Guid productId)
        {
            dynamic ps = session.Prepare("DELETE FROM product WHERE id=?;");
            var statement = ps.Bind(productId.ToString());
            Cassandra.RowSet x = session.Execute(statement);
            //Console.WriteLine(x.Info.QueriedHost)
            return true;
        }

        public List<Wish> GetAllWish()
        {
            List<Wish> wishItems = new List<Wish>() {  };
            List<Product> productItems = new List<Product>() {  };
            List<Wish> wishes = new List<Wish>()
            {

                //new Wish(1,"Get pen", WishType.Product,wishItems),
                //new Wish(2,"Get wish", WishType.Wish, productItems),
                //new Wish(2,"Book", WishType.Wish, productItems),
            };
            return wishes;
        }


        // This GetSingleProduct can return null if there is no product to return for that id;

        public Wish GetSingleWish(int wishId)
        {
            if (wishId == 0)
            {
                return null;
            }
            else
            {
                List<Product> productItems = new List<Product>() { };
                return new Wish();
            }
        }
        public bool CreateWish(string name, WishType wishType, List<Product> products)
        {
            // TODO: store the product in the db and get the id for it
            int id = 0;
            Wish wish = new Wish();
            return true;
        }
        public bool DeleteWish(int wishId)
        {
            return true;
        }
    }
}
