using Cassandra;
using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.persistant.model;

namespace TukiTaki_KenaKata.persistant.mapper
{
    class CassandraMapper
    {
        public static Product DBProductMapper(Row row)
        {
            return new Product(
                row.GetValue<string>(Product.COL_ID),
                row.GetValue<string>(Product.COL_NAME),
                row.GetValue<string>(Product.COL_DESCRIPTION),
                row.GetValue<double>(Product.COL_PRICE));
        }
        public static Wish DBWishMapper(Row row)
        {
            return new Wish(
                row.GetValue<string>(Wish.COL_ID),
                row.GetValue<string>(Wish.COL_NAME));
        }
        public static WishList DBWishListMapper(Row row)
        {
            return new WishList(
                row.GetValue<string>(WishList.COL_ID),
                row.GetValue<string>(WishList.COL_WISH_ID),
                row.GetValue<string>(WishList.COL_ITEM_ID),
                row.GetValue<int>(WishList.COL_ITEM_TYPE));
        }
    }
}
