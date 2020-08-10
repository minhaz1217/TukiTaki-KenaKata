using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.model;
using TukiTaki_KenaKata.persistant;

namespace TukiTaki_KenaKata.service
{
    class ProductService : IProductService
    {

        IDBRepository db = null;

        public ProductService()
        {
            using (var scope = DependencyResolver.Instance().BeginLifetimeScope())
            {
                db = scope.Resolve<IDBRepository>();
            }
        }

        public List<Product> GetAllProduct()
        {
            // TODO: use linq
            return db.GetAllProduct();
        }

        public Product GetSingleProduct(string productId)
        {
            Guid id = Helper.SafeGuidParse(productId);

            if (id == new Guid())
            {
                Helper.MyPrint("Invalid Product id", "r");
                return null;
            }
            else
            {
                return this.db.GetSingleProduct(id);
            }
        }
        public bool CreateProduct(string name, string description, double price)
        {
            return this.db.CreateProduct(name, description, price);
        }
        public bool ChangeProductName(string idString, string name)
        {
            Guid productId = Helper.SafeGuidParse(idString);
            if (productId == new Guid())
            {
                Console.WriteLine("Invalid Product id");
                return false;
            }
            else
            {
                return this.db.ChangeProductName(productId, name);
            }
        }
        public bool ChangeProductDescription(string idString, string description)
        {
            Guid productId = Helper.SafeGuidParse(idString);
            if (productId == new Guid())
            {
                Console.WriteLine("Invalid Product id");
                return false;
            }
            else
            {
                return this.db.ChangeProductDescription(productId, description);
            }
        }

        public bool ChangeProductPrice(string idString, double price)
        {
            Guid productId = Helper.SafeGuidParse(idString);
            if(price < 0)
            {
                Helper.MyPrint("Price can't be negative.", "r");
                return false;
            }
            else if (productId == new Guid())
            {
                Console.WriteLine("Invalid Product id");
                return false;
            }
            else
            {
                return this.db.ChangeProductPrice(productId, price);
            }
        }
        public bool ProductExists(string idString)
        {
            Guid productId = Helper.SafeGuidParse(idString);
            if (productId == new Guid())
            {
                Console.WriteLine("Invalid Product id");
                return false;
            }
            else
            {
                if (this.db.CheckProductCount(productId) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool DeleteProduct(string productId)
        {
            Guid id = Helper.SafeGuidParse(productId);
            if (id == new Guid())
            {
                Console.WriteLine("Invalid Product id");
                return false;
            }
            else
            {
                return this.db.DeleteProduct(id);
            }
        }
    }
}
