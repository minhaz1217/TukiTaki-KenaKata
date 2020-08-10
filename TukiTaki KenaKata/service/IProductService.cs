using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.model;

namespace TukiTaki_KenaKata.service
{
    interface IProductService
    {
        List<Product> GetAllProduct();
        Product GetSingleProduct(string productId);
        bool CreateProduct(string name, string description, double price);
        bool ChangeProductName(string idString, string name);
        bool ChangeProductDescription(string idString, string description);
        bool ChangeProductPrice(string idString, double price);
        bool ProductExists(string idString);
        bool DeleteProduct(string productId);
    }
}
