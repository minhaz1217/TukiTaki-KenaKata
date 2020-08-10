using System;
using System.Collections.Generic;
using System.Text;

namespace TukiTaki_KenaKata.presentation
{
    interface IWishView
    {
        void CreateWish();
        void ShowAllWish();
        void ViewSingleWish();
        void UpdateWish();
        void DeleteWish();
        void ShowAllCoupons();
        void ShowWishListPrice();
    }
}
