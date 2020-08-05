using System;
using System.Collections.Generic;
using System.Text;

namespace TukiTaki_KenaKata.model
{
    class Product
    {
        int id { get; set; }
        string name { get; set; }
        string description { get; set; }
        double price { get; set; }

        public Product()
        {

        }
        public Product(int id, string name, string description, double price)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.price = price;
        }

        public string ToString()
        {
            return $"{this.id}. {this.name} ({this.description}) : {this.price}";
        }
    }
}
