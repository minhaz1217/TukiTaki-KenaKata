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
    class WishListItemDTO { 
        public Guid id { get; set; }
        public ItemType type { get; set; }
        public WishListItemDTO(Guid id, ItemType itemType)
        {
            this.id = id;
            this.type = itemType;
        }
    }
}
