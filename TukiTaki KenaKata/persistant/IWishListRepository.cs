using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.persistant.model;

namespace TukiTaki_KenaKata.persistant
{
    interface IWishListRepository
    {
        bool CreateWishLists(List<WishList> wishLists);
        bool DeleteWishLists(List<WishList> wishLists);
    }
}
