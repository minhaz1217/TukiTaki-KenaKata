using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.model;

namespace TukiTaki_KenaKata.persistant
{
    interface IDBController
    {
        List<Product> GetAllProduct();
        Product GetSingleProduct(int productId);
    }
}
