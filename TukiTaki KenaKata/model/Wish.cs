using System;
using System.Collections.Generic;
using System.Text;

namespace TukiTaki_KenaKata.model
{
    public enum WishType
    {
        Product,
        Wish
    }
    class Wish
    {
        int id { get; set; }
        string name { get; set; }
        WishType wishType { get; set; }
        List<Product> items { get; set; }
        List<Wish> wishes { get; set; }
        public Wish(int id, string name, WishType wishType, List<Product> items)
        {
            this.id = id;
            this.name = name;
            this.wishType = wishType;
            this.items = items;
        }
        public Wish(int id, string name, WishType wishType, List<Wish> wishes)
        {
            this.id = id;
            this.name = name;
            this.wishType = wishType;
            this.wishes = wishes;
        }

        string ToString()
        {
            return $"{this.id} {this.name} {this.wishType} {this.items}";
        }
    }
}
