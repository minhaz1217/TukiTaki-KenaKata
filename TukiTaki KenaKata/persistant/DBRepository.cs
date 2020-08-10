using Cassandra;
using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.model;

namespace TukiTaki_KenaKata.persistant
{
    class DBRepository : IDBRepository
    {
        private static DBRepository instance = null;
        private const string SERVER_NAME = "localhost";
        private const string KEYSPACE_NAME = "tukitaki_kenakata";
        private static Cluster cluster=null;
        private static Session session=null;

        private DBRepository()
        {
        }
        public static DBRepository Instance()
        {

            // DBController.GetInstance();
            // new DBController();
            if(instance == null)
            {
                instance = new DBRepository();
                if (cluster == null)
                {
                    cluster = (Cluster)Cluster.Builder().AddContactPoints(SERVER_NAME).Build();
                    session = (Session)cluster.Connect(KEYSPACE_NAME);
                }
                else
                {
                    session = (Session)cluster.Connect(KEYSPACE_NAME);
                }
            }
            return instance;
        }

        public long CheckProductCount(Guid productId)
        {
            PreparedStatement ps = session.Prepare("select count(id) as count from product where id=?;");
            BoundStatement statement = ps.Bind(productId.ToString());
            RowSet rowSet = session.Execute(statement);
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
            BoundStatement statement = ps.Bind(productId.ToString());
            RowSet rowSet = session.Execute(statement);
            foreach (Row row in rowSet)
            {
                return new Product(row);
            }
            return null;
        }
        public bool CreateProduct(string name, string description, double price)
        {
            dynamic ps = session.Prepare("INSERT INTO product(id, name, description,price) VALUES(?,?,?,?);");
            Statement statement = ps.Bind(Guid.NewGuid().ToString(), name, description, price);
            RowSet x = session.Execute(statement);
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


        public bool CreateWish(string name, List<WishListItem> items)
        {
            Guid wishId = Guid.NewGuid();
            PreparedStatement wishTableInsert = session.Prepare("INSERT INTO wish(id, name) VALUES(?,?);");
            PreparedStatement wishListInsert = session.Prepare("INSERT INTO wishlist(id, item_id, wish_id, item_type) VALUES(?,?,?,?);");
            BatchStatement batch = new BatchStatement();
            batch.Add(wishTableInsert.Bind(wishId.ToString(), name));
            foreach (WishListItem item in items)
            {
                batch.Add(wishListInsert.Bind(Guid.NewGuid().ToString(), item.id.ToString(), wishId.ToString(), (int)item.type));
            }
            RowSet x = session.Execute(batch);
            return true;
        }

        public List<WishListItem> GetWishListItemForAWish(Guid wishId)
        {
            List<WishListItem> wishListItems = new List<WishListItem>();

            return wishListItems;
        }

        public List<Product> GetAllProductFromWish(Guid wishId)
        {
            List<Product> products = new List<Product>();
            PreparedStatement wishListPS = session.Prepare("select * from wishlist where wish_id=? ALLOW FILTERING");
            BoundStatement wishListBS = wishListPS.Bind(wishId.ToString());
            RowSet wishListResult = session.Execute(wishListBS);

            PreparedStatement productPreparedStatement = session.Prepare("select * from product where id = ?");
            foreach (Row wishlistRow in wishListResult)
            {
                if (wishlistRow.GetValue<int>("item_type") == (int)ItemType.Product)
                {
                    Guid productId = Helper.SafeGuidParse(wishlistRow.GetValue<string>("item_id"));
                    if (productId != new Guid())
                    {
                        RowSet productRowSet = session.Execute(productPreparedStatement.Bind(productId.ToString()));
                        foreach(Row productRow in productRowSet)
                        {
                            products.Add(new Product(productRow));
                        }
                        //productBatch.Add(productPreparedStatement.Bind(productId.ToString()));
                    }
                } else if (wishlistRow.GetValue<int>("item_type") == (int)ItemType.Wish)
                {
                    Guid reWishId = Helper.SafeGuidParse(wishlistRow.GetValue<string>("item_id"));
                    if (reWishId != new Guid())
                    {
                        products.AddRange(GetAllProductFromWish(reWishId));
                    }
                }
            }
            return products;
        }
        public List<Wish> GetAllWish()
        {
            List<Wish> wishes = new List<Wish>();
            RowSet wishesResult = session.Execute("select * from wish;");
            foreach (Row wishRow in wishesResult)
            {
                wishes.Add(new Wish(wishRow));
            }
            return wishes;
        }

        public long CheckWishCount(Guid wishId)
        {
            PreparedStatement ps = session.Prepare("select count(id) as count from wish where id=?;");
            BoundStatement statement = ps.Bind(wishId.ToString());
            RowSet rowSet = session.Execute(statement);
            foreach (Row row in rowSet)
            {
                return row.GetValue<long>("count");
            }
            return 0;
        }

        // This GetSingleWish can return null if there is no wish to return for that id;
        public Wish GetSingleWish(Guid productId)
        {

            Wish wish = null;
            PreparedStatement preparedStatement = session.Prepare("select * from wish where id=?");
            RowSet wishesResult = session.Execute(preparedStatement.Bind(productId.ToString()));
            foreach (Row wishRow in wishesResult)
            {
                wish = new Wish(wishRow);
            }
            return wish;
        }
        public bool ChangeWishName(Guid wishId, string newName)
        {

            PreparedStatement ps = session.Prepare("UPDATE wish SET name=? WHERE id=?");
            BoundStatement statement = ps.Bind(newName, wishId.ToString());
            RowSet rowSet = session.Execute(statement);
            return true;
        }
        public bool AddItemToWish(Guid wishId, WishListItem item)
        {
            PreparedStatement wishListInsert = session.Prepare("INSERT INTO wishlist(id, item_id, wish_id, item_type) VALUES(?,?,?,?);");
            RowSet x = session.Execute(wishListInsert.Bind(Guid.NewGuid().ToString(), item.id.ToString(), wishId.ToString(), (int)item.type));
            return true;
        }
        public bool DeleteItemFromWish(Guid wishId, Guid item)
        {
            // TODO: error
            PreparedStatement wishListInsert = session.Prepare("DELETE from wishlist where wish_id=? and item_id=?;");
            // wishlist >> id wish_id item_id
            //select * from wishlist where wish_id =?  >> id


            RowSet x = session.Execute(wishListInsert.Bind(wishId.ToString(), item.ToString()));
            return true;
        }
        public bool DeleteWish(Guid wishId)
        {
            PreparedStatement removeWishFromWishListPS = session.Prepare("DELETE from wishlist where id=?;");
            PreparedStatement removeWishFromWishPS = session.Prepare("DELETE from wish where id=?;");

            PreparedStatement selectWishListRowsPS = session.Prepare("SELECT * from wishlist where wish_id=? ALLOW FILTERING;");
            PreparedStatement selectWishListRowsByItemIdPS = session.Prepare("SELECT * from wishlist where item_id=? ALLOW FILTERING;");
            RowSet selectWishListRowsByWishId = session.Execute(selectWishListRowsPS.Bind(wishId.ToString()));
            RowSet selectWishListRowsByItemId= session.Execute(selectWishListRowsByItemIdPS.Bind(wishId.ToString()));
            BatchStatement batch = new BatchStatement();
            foreach(Row row in selectWishListRowsByWishId)
            {
                batch.Add(removeWishFromWishListPS.Bind(row.GetValue<string>("id")));
            }

            foreach (Row row in selectWishListRowsByItemId)
            {
                batch.Add(removeWishFromWishListPS.Bind(row.GetValue<string>("id")));
            }
            batch.Add(removeWishFromWishPS.Bind(wishId.ToString()));
            RowSet x = session.Execute( batch );
            return true;
        }
        public bool CheckCycle(Guid parent, Guid child)
        {
            RowSet rows = session.Execute("select * from wishlist;");
            Dictionary<string, List<string>> graph = new Dictionary<string, List<string>>();
            Dictionary<string, bool> visited1 = new Dictionary<string, bool>();
            Dictionary<string, bool> visited2 = new Dictionary<string, bool>();

            graph[parent.ToString()] = new List<string>();
            visited1[parent.ToString()] = false;
            visited2[parent.ToString()] = false;

            graph[child.ToString()] = new List<string>();
            visited1[child.ToString()] = false;
            visited2[child.ToString()] = false;
            foreach (Row item in rows)
            {
                if(item.GetValue<int>("item_type")  == (int)ItemType.Wish)
                {

                    string wishId = item.GetValue<string>("wish_id");
                    string itemId = item.GetValue<string>("item_id");
                    if (!graph.ContainsKey(wishId))
                    {
                        graph[wishId] = new List<string>();
                        visited1[wishId] = false;
                        visited2[wishId] = false;
                    }
                    if (!graph.ContainsKey(itemId))
                    {
                        graph[itemId] = new List<string>();
                        visited1[itemId] = false;
                        visited2[itemId] = false;
                    }
                    graph[wishId].Add(itemId);
                }
            }

            foreach(KeyValuePair<string, List<string>> item in graph)
            {
                StringBuilder stringBuilder = new StringBuilder();
                item.Value.ForEach(delegate(string edge)
                {
                    stringBuilder.Append(edge + ",");
                });
                Console.WriteLine($"{item.Key} -> {stringBuilder.ToString()}");
            }
            visited1[parent.ToString()] = true;
            visited2[child.ToString()] = true;
            bool fromParent = CycleDetection(graph, visited1, parent.ToString(), child.ToString());
            bool fromChild = CycleDetection(graph, visited2, child.ToString(), parent.ToString());
            Helper.MyPrint($"Parent: {parent.ToString()} -> {fromParent}");
            Helper.MyPrint($"Child: {child.ToString()} -> {fromChild}");
            return  fromParent ||  fromChild;
        }

        // it returns true if cycle exists other wise false
        public bool CycleDetection(Dictionary<string, List<string> > graph, Dictionary<string, bool> visited, string start, string end)
        {
            foreach(string edge in graph[start])
            {
                if(edge == end)
                {
                    return true;
                }
                else
                {
                    if(visited[edge] == false)
                    {
                        visited[edge] = true;
                        return CycleDetection(graph, visited, edge, end);
                    }
                    else
                    {
                        return true; // back edge exists
                    }
                }
            }
            return false;
        }
    }
}
