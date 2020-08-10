using System;
using System.Collections.Generic;
using System.Text;

namespace TukiTaki_KenaKata.persistant.model
{
    class WishList
    {
        public const string COL_ID = "id";
        public const string COL_WISH_ID = "wish_id";
        public const string COL_ITEM_ID = "item_id";
        public const string COL_ITEM_TYPE = "item_type";

        string id;
        string itemId;
        string wishId;
        int itemType;
        public WishList(string id, string itemId, string wishId, int itemType)
        {
            this.id = id;
            this.itemId = itemId;
            this.wishId = wishId;
            this.itemType = itemType;
        }
    }
}
