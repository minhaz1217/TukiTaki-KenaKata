using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.model;

namespace TukiTaki_KenaKata.persistant
{
    class DBController : IDBController
    {
        public List<Product> GetAllProduct()
        {
            List<Product> products = new List<Product>()
            {
                new Product(1,"Pen", "This is a simple pen", 5),
                new Product(2,"Book", "This is a simple book", 200),
                new Product(3,"Mug", "This is a simple mug", 50)
            };
            return products;
        }

        // This GetSingleProduct can return null if there is no product to return for that id;

        public Product GetSingleProduct(int productId)
        {
            if (productId == 0)
            {
                return null;
            }
            else
            {
                return new Product(1, "Pen", "This is a simple pen", 5);
            }
        }
        public bool CreateProduct(string name, string description, double price)
        {
            // TODO: store the product in the db and get the id for it
            int id = 0;
            Product p = new Product(id, name, description, price);
            return true;
        }
        public bool DeleteProduct(int productId)
        {
            return true;
        }

        public List<Wish> GetAllWish()
        {
            List<Wish> wishItems = new List<Wish>() {  };
            List<Product> productItems = new List<Product>() {  };
            List<Wish> wishes = new List<Wish>()
            {

                new Wish(1,"Get pen", WishType.Product,wishItems),
                new Wish(2,"Get wish", WishType.Wish, productItems),
                new Wish(2,"Book", WishType.Wish, productItems),
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
                return new Wish(1, "Pen", WishType.Product, productItems);
            }
        }
        public bool CreateWish(string name, WishType wishType, List<Product> products)
        {
            // TODO: store the product in the db and get the id for it
            int id = 0;
            Wish wish = new Wish(id, name, wishType, products);
            return true;
        }
        public bool DeleteWish(int wishId)
        {
            return true;
        }
    }
}
