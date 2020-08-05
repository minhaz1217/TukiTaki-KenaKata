using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.model;
using TukiTaki_KenaKata.service;

namespace TukiTaki_KenaKata.presentation
{
    class WishView
    {
        WishService wishService;
        public WishView()
        {
            this.wishService = new WishService();
        }
        public void ShowAllWish()
        {
            List<Wish> wishes= new List<Wish>();
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
            int wishId = Helper.ReadSafeInt();
            if (wishId == -1)
            {
                return;
            }
            else
            {
                this.wishService.GetSingleWish(wishId);
            }
        }
        public void CreateWish()
        {
            Console.WriteLine("Enter wish name: ");
            string name = Console.ReadLine().Trim();
            //Console.WriteLine("Select product description: ");
            //string description = Console.ReadLine().Trim();
            //Console.WriteLine("Enter product price: ");
            //double price = Helper.ReadSafeDouble();
            //this.wishService.CreateWish(name, description, price);

        }
        public void UpdateWish()
        {
            Console.WriteLine("UpdateProduct");
        }
        public void DeleteWish()
        {
            Console.WriteLine("DeleteProduct");
        }
        public void ShowWishListPrice()
        {
            Console.WriteLine("Show wish list price");
        }
    }
}
