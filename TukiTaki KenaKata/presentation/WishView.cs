using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.model;
using TukiTaki_KenaKata.service;
using Autofac;

namespace TukiTaki_KenaKata.presentation
{
    class WishView : IWishView
    {
        IWishService wishService;
        IProductView productView;
        IProductService productService;

        Dictionary<string, double> coupons = new Dictionary<string, double>();
        public WishView()
        {

            using (var scope = DependencyResolver.Instance().BeginLifetimeScope())
            {
                this.wishService = scope.Resolve<IWishService>();
                this.productView = scope.Resolve<IProductView>();
                this.productService = scope.Resolve<IProductService>();
            }

            this.coupons["drop10"] = .1;
            this.coupons["drop20"] = .2;
            this.coupons["drop50"] = .5;
            this.coupons["drop100"] = 1;
        }

        public void CreateWish()
        {
            Console.WriteLine("Enter wish name: ");
            string name = Console.ReadLine().Trim();
            Guid wishId = Guid.NewGuid();
            List<WishListItemDTO> items = new List<WishListItemDTO>();
            while (true)
            {
                Console.WriteLine($"1. To add Product in {name}?");
                Console.WriteLine($"2. To add another wish list in {name}?");
                Console.WriteLine($"-1 to finish.");
                int intChoice = Helper.ReadSafeInt();
                if(intChoice == 1)
                {
                    this.productView.ShowAllProductsView();
                    Console.WriteLine($"Enter product id you want to add in {name}");
                    string idString = Console.ReadLine();
                    if (this.productService.ProductExists(idString))
                    {
                        items.Add(new WishListItemDTO(Helper.SafeGuidParse(idString), ItemType.Product));
                        Helper.MyPrint("Product has been added to your list.", "g");
                    }
                    else
                    {
                        Helper.MyPrint("Product Not Found.", "r");
                    }
                }else if(intChoice == 2)
                {

                    this.ShowAllWish();
                    Console.WriteLine($"Enter wish id you want to add in {name}");
                    string idString = Console.ReadLine().Trim();
                    Guid newWishId = Helper.SafeGuidParse(idString);
                    if(newWishId != new Guid())
                    {
                        items.Add(new WishListItemDTO(Helper.SafeGuidParse(idString), ItemType.Wish));
                        Helper.MyPrint($"Wish added to {name}");
                    }
                    else
                    {
                        Helper.MyPrint("Wish id invalid.");
                    }
                }
                else
                {
                    this.wishService.CreateWish(name, items);
                    return;
                }
            }

        }
        public void ShowAllWish()
        {
            List<WishDTO> wishes= new List<WishDTO>();
            wishes = this.wishService.GetAllWish();
            for (int i = 0; i < wishes.Count; i++)
            {
                Console.WriteLine(wishes[i].ToString());
            }
        }
        public void ViewSingleWish()
        {
            Console.WriteLine("Enter the wish id you want to see: ");
            Console.WriteLine("Enter -1 to go back: ");
            string idString = Console.ReadLine().Trim();
            WishDTO wish = this.wishService.GetSingleWish(idString);
            if(wish== null)
            {
                Helper.MyPrint("No wish found.");
            }
            else
            {
                Console.WriteLine(wish.ToString());
            }
        }
        public void UpdateWish()
        {
            Console.WriteLine("Enter the wish id you want to update: ");
            Console.WriteLine("Enter -1 to go back: ");
            string idString = Console.ReadLine().Trim();
            if (this.wishService.WishExists(idString))
            {
                Console.WriteLine("1. To Update Name.");
                Console.WriteLine("2. To Insert New Item.");
                Console.WriteLine("3. To Remove an item from the list(product or wish).");
                Console.WriteLine("-1. To go back.");
                int choiceInt = Helper.ReadSafeInt();
                if(choiceInt == 1)
                {
                    Console.WriteLine("Enter the new name: ");
                    string name = Console.ReadLine();
                    this.wishService.ChangeWishName(idString, name);
                }else if(choiceInt == 2)
                {
                    Console.WriteLine($"1. To add Product?");
                    Console.WriteLine($"2. To add another wish list?");
                    Console.WriteLine($"-1 to finish.");
                    int choiceInt2 = Helper.ReadSafeInt();
                    if(choiceInt2 == 1)
                    {
                        this.productView.ShowAllProductsView();
                        Console.WriteLine("Enter Product id:");
                        string productIdString = Console.ReadLine().Trim();
                        this.wishService.AddItemToWish(idString, productIdString, ItemType.Product);

                    }else if(choiceInt2 == 2)
                    {
                        this.ShowAllWish();
                        Console.WriteLine("Enter Wish id:");
                        string newWishIdString = Console.ReadLine().Trim();
                        if (this.wishService.CheckCycleInWishList(idString, newWishIdString))
                        {
                            this.wishService.AddItemToWish(idString, newWishIdString, ItemType.Wish);
                            Helper.MyPrint("Entered Succesfully.", "g");
                        }
                        else
                        {
                            Helper.MyPrint("Problem occured.");

                        }
                    }
                    else
                    {
                        return;
                    }
                }else if(choiceInt == 3)
                {
                    Console.WriteLine("Enter item id: ");
                    string itemIdString = Console.ReadLine().Trim();
                    this.wishService.DeleteWishItem(idString, itemIdString);
                }
                else
                {
                    return;
                }
            }
            else
            {
                Helper.MyPrint("Wish doesn't exist.", "r");
            }
        }
        public void DeleteWish()
        {
            this.ShowAllWish();
            Console.WriteLine("Enter the id of the wish you want to delete");
            string wishIdString = Console.ReadLine().Trim();
            if (this.wishService.WishExists(wishIdString))
            {
                this.wishService.DeleteWish(wishIdString);
                Helper.MyPrint("Wish removed", "g");
            }
            else
            {
                Helper.MyPrint("Wish doesn't exist");
            }
            Console.WriteLine("DeleteProduct");
        }
        public void ShowAllCoupons()
        {
            foreach (KeyValuePair<string, double> coupon in coupons)
            {
                Helper.MyPrint($"{coupon.Key} gives you {coupon.Value * 10}%");
            }
        }
        public void ShowWishListPrice()
        {
            this.ShowAllWish();
            Console.WriteLine("Enter Wish list id:");

            string wishIdString = Console.ReadLine().Trim();
            if (this.wishService.WishExists(wishIdString))
            {
                Console.WriteLine("Enter discout codes, seperated by spaces");
                string discountKeys = Console.ReadLine().Trim();
                double totalDiscount = 0;
                foreach(string coupon in discountKeys.Split(" "))
                {
                    if(coupon != "")
                    {
                        if (this.coupons.ContainsKey(coupon))
                        {
                            totalDiscount += this.coupons[coupon];
                        }
                        else
                        {
                            Helper.MyPrint($"Error: {coupon} doesn't exist.", "r");
                        }
                    }
                }
                if(totalDiscount > 1)
                {
                    totalDiscount = 1;
                }
                WishDTO wish = this.wishService.GetSingleWish(wishIdString);
                if(wish != null)
                {
                    double totalWishPrice = 0;
                    foreach(ProductDTO product in wish.Products)
                    {
                        totalWishPrice += product.Price;
                    }
                    Helper.MyPrint($"Your total wish will cost {totalWishPrice - totalWishPrice*totalDiscount} taka.", "g");
                }

            }
            else
            {
                Helper.MyPrint("Wish doesn't exist.");
            }

        }
    }
}
