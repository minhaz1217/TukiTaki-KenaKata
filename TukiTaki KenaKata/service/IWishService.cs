using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.model;
using TukiTaki_KenaKata.persistant.model;

namespace TukiTaki_KenaKata.service
{
    interface IWishService
    {
        List<WishDTO> GetAllWish();
        WishDTO GetSingleWish(string stringId);
        bool CreateWish(string name, List<WishListItemDTO> items);
        bool WishExists(string idString);
        bool ChangeWishName(string idString, string name);
        bool AddItemToWish(string wishIdString, string itemIdString, ItemType itemType);
        bool DeleteWishItem(string wishIdString, string itemIdString);
        bool DeleteWish(string wishIdString);
        bool CycleExists(Guid start, Guid end);
        bool CanAddThisWishToThatWish(string parentWish, string childWish);
    }
}
