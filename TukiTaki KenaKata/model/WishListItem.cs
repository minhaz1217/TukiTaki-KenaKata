using System;
using System.Collections.Generic;
using System.Text;

namespace TukiTaki_KenaKata.model
{
    public enum ItemType
    {
        Product,
        Wish
    }
    class WishListItem { 
        public Guid id { get; set; }
        public ItemType type { get; set; }
        public WishListItem(Guid id, ItemType itemType)
        {
            this.id = id;
            this.type = itemType;
        }
    }
}
