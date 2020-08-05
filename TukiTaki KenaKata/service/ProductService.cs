using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.model;
using TukiTaki_KenaKata.persistant;

namespace TukiTaki_KenaKata.service
{
    class ProductService
    {

        DBController db;

        public ProductService()
        {
            this.db = new DBController();
        }

        public List<Product> GetAllProduct()
        {
            return db.GetAllProduct();
        }

        public Product GetSingleProduct(int productId)
        {
            return this.db.GetSingleProduct(productId);
        }
        public bool CreateProduct(string name, string description, double price)
        {
            return this.db.CreateProduct(name, description, price);
        }

        public bool ProductExists(int productId)
        {
            Product product = this.db.GetSingleProduct(productId);
            if(product == null)
            {
                return false;
            }
            else
            {
                return true;
            }
            //return this.db.GetSingleProduct(productId)
        }
        public bool DeleteProduct(int productId)
        {
            return this.db.DeleteProduct(productId);
        }
    }
}
