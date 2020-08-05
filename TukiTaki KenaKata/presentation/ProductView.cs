using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.model;
using TukiTaki_KenaKata.service;

namespace TukiTaki_KenaKata.presentation
{
    class ProductView
    {
        ProductService productService;
        public ProductView()
        {
            this.productService = new ProductService();
        }
        public void ShowAllProductsView()
        {
            List<Product> products = new List<Product>();
            products = this.productService.GetAllProduct();
            for (int i = 0; i < products.Count; i++)
            {
                Console.WriteLine(products[i].ToString());
            }
        }
        public void ViewSingleProduct()
        {
            Console.WriteLine( "Enter the product id you want to see: " );
            Console.WriteLine( "Enter -1 to go back: " );
            int productId = Helper.ReadSafeInt();
            if(productId == -1)
            {
                return;
            }
            else
            {
                this.productService.GetSingleProduct(productId);
            }
        }
        public void CreateProduct()
        {
            Console.WriteLine("Enter product name: ");
            string name = Console.ReadLine().Trim();
            Console.WriteLine("Enter product description: ");
            string description = Console.ReadLine().Trim();
            Console.WriteLine("Enter product price: ");
            double price = Helper.ReadSafeDouble();
            this.productService.CreateProduct(name, description, price);

        }
        public void UpdateProduct()
        {
            Console.WriteLine("Enter the product id that you want to edit: ");
            int productId = Helper.ReadSafeInt();
            if (this.productService.ProductExists(productId))
            {
                Product product = this.productService.GetSingleProduct(productId);
                Console.WriteLine(product.ToString());
            }
            else
            {
                Console.WriteLine("Product Doesn't Exist.");
            }
        }
        public void DeleteProduct()
        {
            Console.WriteLine("Enter the product id that you want to delete(-1 to go  back): ");
            int productId = Helper.ReadSafeInt();
            if(productId < 0)
            {
                return;
            }
            else
            {
                this.productService.DeleteProduct(productId);
            }
        }
        
    }
}
