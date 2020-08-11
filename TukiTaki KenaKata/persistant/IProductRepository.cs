using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.model;
using TukiTaki_KenaKata.persistant.model;

namespace TukiTaki_KenaKata.persistant
{
    interface IProductRepository
    {
        List<Product> GetAllProduct();
        Product GetSingleProduct(Guid productId);
        bool CreateProduct(Product product);
        bool UpdateProduct(Product product);
        long CheckProductCount(Guid productId);
        bool DeleteProduct(Guid productId);
        //long CheckProductCount(Guid productId);
        //List<Product> GetAllProduct();
        //Product GetSingleProduct(Guid productId);
        //bool CreateProduct(string name, string description, double price);
        //bool ChangeProductName(Guid productId, string name);
        //bool ChangeProductDescription(Guid productId, string description);
        //bool ChangeProductPrice(Guid productId, double price);
        //bool DeleteProduct(Guid productId);
        //bool CreateWish(string name, List<WishListItemDTO> items);
        //List<WishList> GetWishListItemForAWish(Guid wishId);
        //List<Product> GetAllProductFromWish(Guid wishId);
        //List<Wish> GetAllWish();
        //long CheckWishCount(Guid wishId);
        //Wish GetSingleWish(Guid productId);
        //bool ChangeWishName(Guid wishId, string newName);
        //bool AddItemToWish(Guid wishId, WishListItemDTO item);
        //bool DeleteItemFromWish(Guid wishId, Guid item);
        //bool DeleteWish(Guid wishId);
        //bool CheckCycle(Guid parent, Guid child);
        //bool CycleDetection(Dictionary<string, List<string>> graph, Dictionary<string, bool> visited, string start, string end);
    }
}
