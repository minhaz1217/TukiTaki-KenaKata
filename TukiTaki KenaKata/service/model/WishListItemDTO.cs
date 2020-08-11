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
        public Guid Id { get; set; }
        public ItemType Type { get; set; }
        public WishListItemDTO(Guid id, ItemType itemType)
        {
            this.Id = id;
            this.Type = itemType;
        }
    }
}
