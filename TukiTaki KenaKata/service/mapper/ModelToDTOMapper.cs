using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.model;
using TukiTaki_KenaKata.persistant.model;

namespace TukiTaki_KenaKata.service.mapper
{
    class ModelToDTOMapper
    {
        public static ProductDTO ProductMapper(Product product)
        {
            Guid productId = Helper.SafeGuidParse(product.Id);
            if(productId == new Guid())
            {
                Helper.MyPrint("Id is invalid", "r");
                return new ProductDTO();
            }
            else
            {
                return new ProductDTO(productId, product.Name, product.Description, product.Price);
            }
        }

    }
}
