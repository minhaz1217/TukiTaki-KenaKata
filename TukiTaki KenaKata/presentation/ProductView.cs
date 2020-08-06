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
            Console.WriteLine("Enter the product id that you want to see(-1 to go  back): ");
            string choice = Console.ReadLine().Trim();
            int ch = 0; ;
            int.TryParse(choice, out ch);
            if (ch == 0)
            {

                Product product = this.productService.GetSingleProduct(choice);
                Console.WriteLine(product.ToString());
            }
            else
            {
                return;
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
            // TODO: work on update product
            Console.WriteLine("1. Change Product Name");
            Console.WriteLine("2. Change Product Description");
            Console.WriteLine("3. Change Product Price");
            Console.WriteLine("-1 to go back");
            int choice = Helper.ReadSafeInt();
            if(choice <0)
            {
                return;
            }
            else
            {
                this.ShowAllProductsView();
                Console.WriteLine("Enter the product id: ");
                string choiceString = Console.ReadLine().Trim();
                if (productService.ProductExists(choiceString))
                {

                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("Enter product name: ");
                            string name = Console.ReadLine().Trim();
                            productService.ChangeProductName(choiceString, name);
                            break;
                        case 2:
                            Console.WriteLine("Enter product Description: ");
                            string description = Console.ReadLine().Trim();
                            productService.ChangeProductDescription(choiceString, description);
                            break;
                        case 3:
                            Console.WriteLine("Enter product price: ");
                            double price = Helper.ReadSafeDouble();
                            productService.ChangeProductPrice(choiceString, price);
                            break;
                        default:
                            return;
                    }
                }
                else
                {
                    Console.WriteLine("Wrong input");
                }
            }
        }
        public void DeleteProduct()
        {
            this.ShowAllProductsView();
            Console.WriteLine("Enter the product id that you want to delete(-1 to go  back): ");
            string choice = Console.ReadLine().Trim();
            int ch = 0; ;
            int.TryParse(choice, out ch);
            if(ch == 0)
            {

                this.productService.DeleteProduct(choice);
            }
            else
            {
                return;
            }
        }
        
    }
}
