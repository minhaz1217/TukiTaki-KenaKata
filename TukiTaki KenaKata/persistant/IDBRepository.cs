using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.model;

namespace TukiTaki_KenaKata.persistant
{
    interface IDBRepository
    {
        long CheckProductCount(Guid productId);
        List<ProductDTO> GetAllProduct();
        ProductDTO GetSingleProduct(Guid productId);
        bool CreateProduct(string name, string description, double price);
        bool ChangeProductName(Guid productId, string name);
        bool ChangeProductDescription(Guid productId, string description);
        bool ChangeProductPrice(Guid productId, double price);
        bool DeleteProduct(Guid productId);
        bool CreateWish(string name, List<WishListItemDTO> items);
        List<WishListItemDTO> GetWishListItemForAWish(Guid wishId);
        List<ProductDTO> GetAllProductFromWish(Guid wishId);
        List<WishDTO> GetAllWish();
        long CheckWishCount(Guid wishId);
        WishDTO GetSingleWish(Guid productId);
        bool ChangeWishName(Guid wishId, string newName);
        bool AddItemToWish(Guid wishId, WishListItemDTO item);
        bool DeleteItemFromWish(Guid wishId, Guid item);
        bool DeleteWish(Guid wishId);
        bool CheckCycle(Guid parent, Guid child);
        bool CycleDetection(Dictionary<string, List<string>> graph, Dictionary<string, bool> visited, string start, string end);
    }
}
