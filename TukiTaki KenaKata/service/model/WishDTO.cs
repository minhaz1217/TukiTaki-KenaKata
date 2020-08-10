using Cassandra;
using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.persistant;

namespace TukiTaki_KenaKata.model
{
    class WishDTO
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public List<ProductDTO> products { get; set; }
        public WishDTO()
        {

        }
        public WishDTO(Guid id, string name, List<ProductDTO> products)
        {
            this.id = id;
            this.name = name;
            this.products = products;
        }
        public WishDTO(Row wish)
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
            this.products.ForEach( delegate(ProductDTO product)
                {
                    outputString.Append($"({product.ToString()})\n");
                });
            return outputString.ToString() ;
        }
    }
}
