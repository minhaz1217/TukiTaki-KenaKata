using System;
using System.Collections.Generic;
using System.Text;

namespace TukiTaki_KenaKata.persistant.model
{
    class WishList
    {
        public const string TABLE_NAME = "wishlist";
        public const string COL_ID = "id";
        public const string COL_WISH_ID = "wish_id";
        public const string COL_ITEM_ID = "item_id";
        public const string COL_ITEM_TYPE = "item_type";

        public string Id { get; set; }
        public string ItemId { get; set; }
        public string WishId { get; set; }
        public int ItemType { get; set; }
        public WishList(string id, string wishId, string itemId, int itemType)
        {
            this.Id = id;
            this.ItemId = itemId;
            this.WishId = wishId;
            this.ItemType = itemType;
        }
        public override string ToString()
        {
            return $"{this.Id}({this.ItemType}) {this.WishId} -> {this.ItemId}" ;
        }
    }
}
