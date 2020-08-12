using System;
using System.Collections.Generic;
using System.Text;

namespace TukiTaki_KenaKata.persistant.model
{
    class Product
    {
        public const string TABLE_NAME = "product";
        public const string COL_ID = "id";
        public const string COL_NAME = "name";
        public const string COL_DESCRIPTION = "description";
        public const string COL_PRICE = "price";

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public Product(string id, string name, string description, double price) 
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Price = price;
        }
    }
}
