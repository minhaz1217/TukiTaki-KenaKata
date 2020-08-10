using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.model;
using TukiTaki_KenaKata.persistant;
using Autofac;

namespace TukiTaki_KenaKata.service
{
    class WishService : IWishService
    {
        IDBRepository db;
        public WishService()
        {
            using (var scope = DependencyResolver.Instance().BeginLifetimeScope())
            {
                db = scope.Resolve<IDBRepository>();
            }
        }
        public List<WishDTO> GetAllWish()
        {

            return db.GetAllWish();
        }

        public WishDTO GetSingleWish(string stringId)
        {
            Guid wishId = Helper.SafeGuidParse(stringId);
            if (wishId == new Guid())
            {
                Helper.MyPrint("Invalid Wish id.", "r");
                return null;
            }
            else
            {
                return this.db.GetSingleWish(wishId);
            }
        }
        public bool CreateWish(string name, List<WishListItemDTO> items)
        {
            return this.db.CreateWish(name, items);
        }
        public bool WishExists(string idString)
        {
            Guid wishId = Helper.SafeGuidParse(idString);
            if (wishId == new Guid())
            {
                Console.WriteLine("Invalid Product id");
                return false;
            }
            else
            {
                if (this.db.CheckWishCount(wishId) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ChangeWishName(string idString, string name)
        {
            Guid wishId = Helper.SafeGuidParse(idString);
            if (wishId == new Guid())
            {
                Console.WriteLine("Invalid Wish id");
                return false;
            }
            else
            {
                return this.db.ChangeWishName(wishId, name);
            }
        }
        public bool AddItemToWish(string wishIdString, string itemIdString, ItemType itemType)
        {
            Guid wishId = Helper.SafeGuidParse(wishIdString);
            Guid itemId = Helper.SafeGuidParse(itemIdString);
            if(wishId == new Guid())
            {
                Helper.MyPrint("Wish Id invalid.", "r");
                return false;
            }else if(itemId == new Guid())
            {
                Helper.MyPrint("Item Id invalid.", "r");
                return false;
            }
            else 
            {
                return this.db.AddItemToWish(wishId, new WishListItemDTO(itemId, itemType) );
            }
            
        }
        public bool DeleteWishItem(string wishIdString, string itemIdString)
        {
            Guid wishId = Helper.SafeGuidParse(wishIdString);
            Guid itemId = Helper.SafeGuidParse(itemIdString);

            if(wishId == new Guid())
            {
                Helper.MyPrint("Wish id invalid.", "r");
                return false;
            }
            else if(itemId == new Guid())
            {
                Helper.MyPrint("Item id is invalid.", "r");
                return false;
            }
            else
            {
                return this.db.DeleteItemFromWish(wishId, itemId);
            }
        }
        public bool DeleteWish(string wishIdString)
        {
            Guid wishId = Helper.SafeGuidParse(wishIdString);
            if(wishId == new Guid())
            {
                Helper.MyPrint("Wish id invalid.");
                return false;
            }
            else
            {
                return this.db.DeleteWish(wishId);

            }
        }

        // true means operation successful, false means operation not successful
        public bool CheckCycleInWishList(string  parentWish, string childWish)
        {
            Guid parentId = Helper.SafeGuidParse(parentWish);
            Guid childId = Helper.SafeGuidParse(childWish);
            if(parentId == new Guid())
            {
                Helper.MyPrint("Invalid source id");
                return false;
            }
            else if(childId  == new Guid())
            {
                Helper.MyPrint("New id is invalid");
                return false;
            }else if (db.CheckCycle(parentId, childId))
            {

                Helper.MyPrint("Cycle Exists.");
                return false;
            }

            return true;
        }
    }
}
