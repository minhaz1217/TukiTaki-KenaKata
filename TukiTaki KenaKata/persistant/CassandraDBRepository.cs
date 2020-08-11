using Cassandra;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using TukiTaki_KenaKata.model;
using TukiTaki_KenaKata.persistant.mapper;
using TukiTaki_KenaKata.persistant.model;

namespace TukiTaki_KenaKata.persistant
{
    class CassandraDBRepository : IDBRepository
    {
        private static CassandraDBRepository instance = null;
        private static string SERVER_NAME;
        private static string KEYSPACE_NAME;
        private static Cluster cluster = null;
        private static Session session = null;

        private CassandraDBRepository() { }
        public static CassandraDBRepository Instance()
        {

            // DBController.GetInstance();
            // new DBController();
            if (instance == null)
            {
                instance = new CassandraDBRepository();
                SERVER_NAME = ConfigurationManager.AppSettings.Get("CASSANDRA_SERVER_NAME");
                KEYSPACE_NAME = ConfigurationManager.AppSettings.Get("CASSANDRA_KEYSPACE_NAME");
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
            foreach (Row row in rs)
            {
                products.Add(CassandraMapper.DBProductMapper(row));
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
                return CassandraMapper.DBProductMapper(row);
            }
            return null;
        }
        public bool CreateProduct(Product product)
        {
            dynamic ps = session.Prepare("INSERT INTO product(id, name, description,price) VALUES(?,?,?,?);");
            Statement statement = ps.Bind(product.Id, product.Name, product.Description, product.Price);
            RowSet x = session.Execute(statement);
            return x.Info.IsSchemaInAgreement;
        }


        public bool UpdateProduct(Product product)
        {
            PreparedStatement ps = session.Prepare("UPDATE product SET name=?, description=?, price=? WHERE id=?");
            BoundStatement statement = ps.Bind(product.Name, product.Description, product.Price, product.Id);
            RowSet rowSet = session.Execute(statement);
            return true;
        }
        //public bool ChangeProductName(Product product)
        //{
        //    PreparedStatement ps = session.Prepare("UPDATE product SET name=? WHERE id=?");
        //    BoundStatement statement = ps.Bind(name, productId.ToString());
        //    RowSet rowSet = session.Execute(statement);
        //    return true;
        //}
        //public bool ChangeProductDescription(Guid productId, string description)
        //{
        //    PreparedStatement ps = session.Prepare("UPDATE product SET description=? WHERE id=?");
        //    BoundStatement statement = ps.Bind(description, productId.ToString());
        //    RowSet rowSet = session.Execute(statement);
        //    return true;
        //}
        //public bool ChangeProductPrice(Guid productId, double price)
        //{
        //    PreparedStatement ps = session.Prepare("UPDATE product SET price=? WHERE id=?");
        //    BoundStatement statement = ps.Bind(price, productId.ToString());
        //    RowSet rowSet = session.Execute(statement);
        //    return true;
        //}
        public bool DeleteProduct(Guid productId)
        {
            dynamic ps = session.Prepare("DELETE FROM product WHERE id=?;");
            var statement = ps.Bind(productId.ToString());
            Cassandra.RowSet x = session.Execute(statement);
            //Console.WriteLine(x.Info.QueriedHost)
            return true;
        }

        public bool CreateWishLists(List<WishList> wishLists)
        {

            PreparedStatement wishListInsert = session.Prepare("INSERT INTO wishlist(id, item_id, wish_id, item_type) VALUES(?,?,?,?);");
            BatchStatement batch = new BatchStatement();
            foreach (WishList item in wishLists)
            {
                batch.Add(wishListInsert.Bind(item.Id, item.ItemId, item.WishId, item.ItemType));
            }
            RowSet x = session.Execute(batch);
            return true;
        }
        public bool CreateWish(Wish wish)
        {
            Guid wishId = Guid.NewGuid();
            PreparedStatement wishTableInsert = session.Prepare("INSERT INTO wish(id, name) VALUES(?,?);");
            session.Execute(wishTableInsert.Bind(wish.Id, wish.Name));
            return true;
            return true;
        }

        public List<WishList> GetWishListByWish(Guid wishId)
        {
            List<WishList> wishLists = new List<WishList>();
            List<ProductDTO> products = new List<ProductDTO>();
            PreparedStatement wishListPS = session.Prepare("select * from wishlist where wish_id=? ALLOW FILTERING");
            BoundStatement wishListBS = wishListPS.Bind(wishId.ToString());
            RowSet rows = session.Execute(wishListBS);
            foreach (Row row in rows)
            {   
                wishLists.Add(CassandraMapper.DBWishListMapper(row));
            }
            return wishLists;
        }
        //public List<ProductDTO> GetAllProductFromWish(Guid wishId)
        //{
        //    List<ProductDTO> products = new List<ProductDTO>();
        //    PreparedStatement wishListPS = session.Prepare("select * from wishlist where wish_id=? ALLOW FILTERING");
        //    BoundStatement wishListBS = wishListPS.Bind(wishId.ToString());
        //    RowSet wishListResult = session.Execute(wishListBS);

        //    PreparedStatement productPreparedStatement = session.Prepare("select * from product where id = ?");
        //    foreach (Row wishlistRow in wishListResult)
        //    {
        //        if (wishlistRow.GetValue<int>("item_type") == (int)ItemType.Product)
        //        {
        //            Guid productId = Helper.SafeGuidParse(wishlistRow.GetValue<string>("item_id"));
        //            if (productId != new Guid())
        //            {
        //                RowSet productRowSet = session.Execute(productPreparedStatement.Bind(productId.ToString()));
        //                foreach (Row productRow in productRowSet)
        //                {
        //                    products.Add(new ProductDTO(productRow));
        //                }
        //                //productBatch.Add(productPreparedStatement.Bind(productId.ToString()));
        //            }
        //        }
        //        else if (wishlistRow.GetValue<int>("item_type") == (int)ItemType.Wish)
        //        {
        //            Guid reWishId = Helper.SafeGuidParse(wishlistRow.GetValue<string>("item_id"));
        //            if (reWishId != new Guid())
        //            {
        //                products.AddRange(GetAllProductFromWish(reWishId));
        //            }
        //        }
        //    }
        //    return products;
        //}
        public List<Wish> GetAllWish()
        {
            List<Wish> wishes = new List<Wish>();
            RowSet wishesResult = session.Execute("select * from wish;");
            foreach (Row wishRow in wishesResult)
            {
                wishes.Add(CassandraMapper.DBWishMapper(wishRow));
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
                wish = CassandraMapper.DBWishMapper(wishRow);
            }
            return wish;
        }
        public bool UpdateWish(Wish wish)
        {
            PreparedStatement ps = session.Prepare("UPDATE wish SET name=? WHERE id=?");
            BoundStatement statement = ps.Bind(wish.Name, wish.Id);
            RowSet rowSet = session.Execute(statement);
            return true;
        }
        public bool DeleteWishLists(List<WishList> wishLists)
        {
            PreparedStatement removeWishFromWishListPS = session.Prepare("DELETE from wishlist where id=?;");
            BatchStatement batch = new BatchStatement();
            foreach (WishList wishList in wishLists)
            {
                batch.Add(removeWishFromWishListPS.Bind(wishList.Id));
            }
            RowSet x = session.Execute(batch);
            return true;
        }
        public bool DeleteWish(Guid wishId)
        {
            PreparedStatement removeWishFromWishPS = session.Prepare("DELETE from wish where id=?;");
            session.Execute(removeWishFromWishPS.Bind(wishId.ToString()));
            return true;
        }
    }
}
