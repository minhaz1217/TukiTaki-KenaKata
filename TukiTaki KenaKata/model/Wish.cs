using Cassandra;
using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.persistant;

namespace TukiTaki_KenaKata.model
{
    class Wish
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public List<Product> products { get; set; }
        public Wish()
        {

        }
        public Wish(Guid id, string name, List<Product> products)
        {
            this.id = id;
            this.name = name;
            this.products = products;
        }
        public Wish(Row wish)
        {
            this.id = Helper.SafeGuidParse(wish.GetValue<string>("id"));
            this.name = wish.GetValue<string>("name");
            //Helper.MyPrint($"{this.id} {this.name}", "r");
            this.products = (DBRepository.Instance().GetAllProductFromWish(this.id));
        }

        public override string ToString()
        {
            StringBuilder outputString = new StringBuilder();
            outputString.Append(Helper.MyOutputString(this.id.ToString() +"\n", "y"));
            outputString.Append(Helper.MyOutputString(this.name.ToString()+"\n", "g"));
            this.products.ForEach( delegate(Product product)
                {
                    outputString.Append($"({product.ToString()})\n");
                });
            return outputString.ToString() ;
        }
    }
}
