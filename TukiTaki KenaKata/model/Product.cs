using System;
using System.Collections.Generic;
using System.Text;

namespace TukiTaki_KenaKata.model
{
    class Product
    {
        Guid id { get; set; }
        string name { get; set; }
        string description { get; set; }
        double price { get; set; }

        public Product()
        {

        }
        public Product(Cassandra.Row row)
        {
            this.id = Guid.Parse(row.GetValue<string>("id"));
            this.name = row.GetValue<string>("name");
            this.description = row.GetValue<string>("description");
            this.price = row.GetValue<double>("price");
        }
        public Product(Guid id, string name, string description, double price)
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
