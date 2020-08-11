using Cassandra;
using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.persistant;

namespace TukiTaki_KenaKata.model
{
    class WishDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<ProductDTO> Products { get; set; }
        public WishDTO() {}
        public WishDTO(Guid id, string name, List<ProductDTO> products)
        {
            this.Id = id;
            this.Name = name;
            this.Products = products;
        }

        public override string ToString()
        {
            StringBuilder outputString = new StringBuilder();
            outputString.Append(Helper.MyOutputString(this.Id.ToString() +"\n", "y"));
            outputString.Append(Helper.MyOutputString(this.Name.ToString()+"\n", "c"));
            this.Products.ForEach( delegate(ProductDTO product)
                {
                    outputString.Append($"({product.ToString()})\n");
                });
            return outputString.ToString() ;
        }
    }
}
