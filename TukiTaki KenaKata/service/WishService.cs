using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.model;
using TukiTaki_KenaKata.persistant;

namespace TukiTaki_KenaKata.service
{
    class WishService
    {
        DBController db;
        public WishService()
        {
            this.db = new DBController();
        }
        public List<Wish> GetAllWish()
        {

            return db.GetAllWish();
        }
        public Wish GetSingleWish(int wishId)
        {
            return this.db.GetSingleWish(wishId);
        }
        public bool CreateWish(string name, WishType wishType, List<Product> products)
        {
            return this.db.CreateWish(name, wishType, products);
        }

        public bool WishExists(int productId)
        {
            Wish wish = this.db.GetSingleWish(productId);
            if (wish == null)
            {
                return false;
            }
            else
            {
                return true;
            }
            //return this.db.GetSingleProduct(productId)
        }
        public bool DeleteWish(int productId)
        {
            return this.db.DeleteWish(productId);
        }
    }
}
