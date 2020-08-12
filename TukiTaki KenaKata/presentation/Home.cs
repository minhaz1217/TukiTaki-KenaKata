using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace TukiTaki_KenaKata.presentation
{
    class Home :IHome
    {
        public void ShowMainMenu()
        {
            int choice = 0;
            while (true)
            {
                Console.WriteLine("1. Show All Product");
                Console.WriteLine("2. View Single Product");
                Console.WriteLine("3. Create Product");
                Console.WriteLine("4. Update Product Info");
                Console.WriteLine("5. Delete Product");
                Console.WriteLine("6. Show All Wish List");
                Console.WriteLine("7. View Single Wish List");
                Console.WriteLine("8. Create Wish List");
                Console.WriteLine("9. Update Wish List");
                Console.WriteLine("10. Delete Wish List");
                Console.WriteLine("11. Show All Coupons");
                Console.WriteLine("12. Show Wish List Price");
                Console.WriteLine("Any other key to exit.");
                Console.WriteLine("Enter your choice (1-12): ");
                choice = Helper.ReadSafeInt();
                if( !(choice >= 1 && choice <= 12))
                {
                    Console.WriteLine("Thank you very much.");
                    Console.ReadKey();
                    break;
                }

                {
                    IProductView productsView = null;
                    IWishView wishView = null;
                    using (var scope = DependencyResolver.Instance().BeginLifetimeScope())
                    {
                        wishView = scope.Resolve<IWishView>();
                        productsView = scope.Resolve<IProductView>();
                    }
                    switch (choice)
                    {
                        case 1:
                            productsView.ShowAllProductsView();
                            break;
                        case 2:
                            productsView.ViewSingleProduct();
                            break;
                        case 3:
                            productsView.CreateProduct();
                            break;
                        case 4:
                            productsView.UpdateProduct();
                            break;
                        case 5:
                            productsView.DeleteProduct();
                            break;
                        case 6:
                            wishView.ShowAllWish();
                            break;
                        case 7:
                            wishView.ViewSingleWish();
                            break;
                        case 8:
                            wishView.CreateWish();
                            break;
                        case 9:
                            wishView.UpdateWish();
                            break;
                        case 10:
                            wishView.DeleteWish();
                            break;
                        case 11:
                            wishView.ShowAllCoupons();
                            break;
                        case 12:
                            wishView.ShowWishListPrice();
                            break;
                        default:
                            break;
                    }
                }
                Console.WriteLine($"Entered Value {choice}");
            }
        }
    }
}
