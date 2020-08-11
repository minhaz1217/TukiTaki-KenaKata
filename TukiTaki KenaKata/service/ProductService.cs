using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.model;
using TukiTaki_KenaKata.persistant;
using TukiTaki_KenaKata.persistant.model;
using TukiTaki_KenaKata.service.mapper;

namespace TukiTaki_KenaKata.service
{
    class ProductService : IProductService
    {

        IDBRepository db = null;

        public ProductService()
        {
            //IDBFactory dbFactory = new DBFactory();
            //db = dbFactory.GetCassandraDB();
            using (var scope = DependencyResolver.Instance().BeginLifetimeScope())
            {
                db = scope.Resolve<IDBRepository>();
            }
        }

        public List<ProductDTO> GetAllProduct()
        {
            // TODO: use linq
            List<Product> products = db.GetAllProduct();
            List<ProductDTO> productDTOs = new List<ProductDTO>();
            foreach (Product prod in products)
            {
                if(Helper.SafeGuidParse(prod.Id) == new Guid())
                {
                    Helper.MyPrint("Error: Invalid Id", "r");
                }
                else
                {
                    productDTOs.Add(ModelToDTOMapper.ProductMapper(prod));
                }
            }
            return productDTOs;
        }

        public ProductDTO GetSingleProduct(string productId)
        {
            Guid id = Helper.SafeGuidParse(productId);

            if (id == new Guid())
            {
                Helper.MyPrint("Invalid Product id", "r");
                return null;
            }
            else
            {
                return ModelToDTOMapper.ProductMapper(this.db.GetSingleProduct(id));
            }
        }
        public bool CreateProduct(string name, string description, double price)
        {
            Guid productId = Guid.NewGuid();
            Product product = new Product(productId.ToString(),name, description,price );
            return this.db.CreateProduct(product);
        }

        public bool ChangeProductName(string idString, string name)
        {
            Guid productId = Helper.SafeGuidParse(idString);
            if (productId == new Guid())
            {
                Helper.MyPrint("Error: Invalid Id", "r");
                return false;
            }
            else
            {
                Product product = this.db.GetSingleProduct(productId);
                if(product == null)
                {
                    Helper.MyPrint("Error: Product Doesn't exist", "r");
                    return false;
                }
                else
                {
                    product.Name = name;
                    return this.db.UpdateProduct(product);
                }

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
                Product product = this.db.GetSingleProduct(productId);
                if (product == null)
                {
                    Helper.MyPrint("Error: Product Doesn't exist", "r");
                    return false;
                }
                else
                {
                    product.Description = description;
                    return this.db.UpdateProduct(product);
                }
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
                Product product = this.db.GetSingleProduct(productId);
                if (product == null)
                {
                    Helper.MyPrint("Error: Product Doesn't exist", "r");
                    return false;
                }
                else
                {
                    product.Price = price;
                    return this.db.UpdateProduct(product);
                }
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
        public bool DeleteProduct(string idString)
        {
            Guid productId = Helper.SafeGuidParse((string)idString);
            if (productId == new Guid())
            {
                Console.WriteLine("Invalid Product id");
                return false;
            }
            else
            {
                return this.db.DeleteProduct(productId);
            }
        }
    }
}
