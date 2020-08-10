using System;
using System.Collections.Generic;
using System.Text;

namespace TukiTaki_KenaKata.persistant.model
{
    class Product
    {
        public const string COL_ID = "id";
        public const string COL_NAME = "name";
        public const string COL_DESCRIPTION = "description";
        public const string COL_PRICE = "price";

        string id;
        string description;
        string name;
        double price;
        public Product(string id, string description, string name, double price) 
        {
            this.id = id;
            this.description = description;
            this.name = name;
            this.price = price;
        }
    }
}
