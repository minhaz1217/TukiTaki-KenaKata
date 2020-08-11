using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.persistant.model;

namespace TukiTaki_KenaKata.persistant
{
    interface IWishRepository
    {
        List<Wish> GetAllWish();
        List<WishList> GetWishListByWish(Guid wishId);
        Wish GetSingleWish(Guid productId);
        bool CreateWish(Wish wish);
        long CheckWishCount(Guid wishId);
        bool UpdateWish(Wish wish);
        bool DeleteWish(Guid wishId);
    }
}
