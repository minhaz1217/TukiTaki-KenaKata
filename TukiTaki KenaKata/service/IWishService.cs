using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.model;

namespace TukiTaki_KenaKata.service
{
    interface IWishService
    {
        List<Wish> GetAllWish();
        Wish GetSingleWish(string stringId);
        bool CreateWish(string name, List<WishListItem> items);
        bool WishExists(string idString);
        bool ChangeWishName(string idString, string name);
        bool AddItemToWish(string wishIdString, string itemIdString, ItemType itemType);
        bool DeleteWishItem(string wishIdString, string itemIdString);
        bool DeleteWish(string wishIdString);
        bool CheckCycleInWishList(string parentWish, string childWish);
    }
}
