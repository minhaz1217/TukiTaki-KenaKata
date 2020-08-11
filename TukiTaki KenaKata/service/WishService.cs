﻿using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.model;
using TukiTaki_KenaKata.persistant;
using Autofac;
using TukiTaki_KenaKata.persistant.model;
using TukiTaki_KenaKata.service.mapper;

namespace TukiTaki_KenaKata.service
{
    class WishService : IWishService
    {
        IDBRepository db;
        public WishService()
        {
            //IDBFactory dbFactory = new DBFactory();
            //db = dbFactory.GetCassandraDB();
            using (var scope = DependencyResolver.Instance().BeginLifetimeScope())
            {
                db = scope.Resolve<IDBRepository>();
            }
        }
        public List<WishDTO> GetAllWish()
        {
            List<Wish> wishes = this.db.GetAllWish();
            List<WishDTO> wishDTOs = new List<WishDTO>();
            foreach(Wish wish in wishes)
            {
                if(Helper.SafeGuidParse(wish.Id) == new Guid())
                {
                    Helper.MyPrint("Error: Invalid ID", "r");
                }
                else
                {
                    List<ProductDTO> products = new List<ProductDTO>();
                    products.AddRange( this.GetProductListForAWish(wish) );
                    wishDTOs.Add(new WishDTO(Helper.SafeGuidParse(wish.Id), wish.Name, products));
                }
            }
            return wishDTOs;
        }
        public List<ProductDTO> GetProductListForAWish(Wish wish)
        {
            List<ProductDTO> products = new List<ProductDTO>();
            List<WishList> wishLists = this.db.GetWishListByWish(Helper.SafeGuidParse(wish.Id));
            foreach(WishList wishlist in wishLists)
            {
                if (wishlist.ItemType == (int)ItemType.Product)
                {
                    Product product = this.db.GetSingleProduct(Helper.SafeGuidParse(wishlist.ItemId));
                    if(product != null)
                    {
                        products.Add(ModelToDTOMapper.ProductMapper( product ));
                    }
                }
                else if(wishlist.ItemType == (int)ItemType.Wish)
                {
                    Wish subWish = db.GetSingleWish(Helper.SafeGuidParse(wishlist.ItemId));
                    if(subWish != null)
                    {
                        products.AddRange( GetProductListForAWish( subWish ) );
                    }
                }
            }
            return products;
        }
        public WishDTO GetSingleWish(string stringId)
        {
            Guid wishId = Helper.SafeGuidParse(stringId);
            if (wishId == new Guid())
            {
                Helper.MyPrint("Error: Invalid Wish id.", "r");
                return null;
            }
            else
            {
                WishDTO wishDTO = new WishDTO(); 
                Wish wish = this.db.GetSingleWish( Helper.SafeGuidParse(stringId) );
                if(wish != null)
                {
                    List<ProductDTO> products = this.GetProductListForAWish(wish);
                    return new WishDTO(Helper.SafeGuidParse(wish.Id), wish.Name, products);
                }
            }
            return null;
        }
        public bool CreateWish(string name, List<WishListItemDTO> items)
        {
            List<WishList> wishLists = new List<WishList>();
            Guid wishId = Guid.NewGuid();
            bool wishTableSuccecc = this.db.CreateWish(new Wish(wishId.ToString(), name));
            foreach(WishListItemDTO item in items)
            {
                wishLists.Add(new WishList( Guid.NewGuid().ToString(), wishId.ToString(), item.Id.ToString(), (int)item.Type));
            }
            bool wishListTableSuccess = this.db.CreateWishLists(wishLists);
            return wishTableSuccecc && wishListTableSuccess;
        }
        public bool WishExists(string idString)
        {
            Guid wishId = Helper.SafeGuidParse(idString);
            if (wishId == new Guid())
            {
                Console.WriteLine("Invalid Product id");
                return false;
            }
            else
            {
                if (this.db.CheckWishCount(wishId) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ChangeWishName(string idString, string name)
        {
            Guid wishId = Helper.SafeGuidParse(idString);
            if (wishId == new Guid())
            {
                Console.WriteLine("Invalid Wish id");
                return false;
            }
            else
            {
                Wish wish = this.db.GetSingleWish( wishId );
                if(wish!= null)
                {
                    wish.Name = name;
                    return this.db.UpdateWish(wish);
                }
                else
                {
                    return false;
                }
            }
        }
        public bool AddItemToWish(string wishIdString, string itemIdString, ItemType itemType)
        {
            Guid wishId = Helper.SafeGuidParse(wishIdString);
            Guid itemId = Helper.SafeGuidParse(itemIdString);
            if (wishId == new Guid())
            {
                Helper.MyPrint("Wish Id invalid.", "r");
                return false;
            }
            else if (itemId == new Guid())
            {
                Helper.MyPrint("Item Id invalid.", "r");
                return false;
            }
            else
            {
                Wish wish = this.db.GetSingleWish(wishId);
                if (wish == null)
                {
                    Helper.MyPrint($"Error: wish doesn't exist {wishId.ToString()}.");
                    return false;
                }
                if (itemType == ItemType.Product)
                {
                    Product itemProduct = this.db.GetSingleProduct(itemId);
                    if (itemProduct == null)
                    {
                        Helper.MyPrint($"Error: Product doesn't exist {itemId.ToString()}.");
                        return false;
                    }
                    return this.db.CreateWishLists(new List<WishList> { new WishList(Guid.NewGuid().ToString(), wish.Id, itemProduct.Id, (int)itemType) });
                }
                else if (itemType == ItemType.Wish)
                {
                    Wish itemWish = this.db.GetSingleWish(wishId);
                    if (itemWish == null)
                    {
                        Helper.MyPrint($"Error: Wish doesn't exist {wishId.ToString()}.");
                        return false;
                    }
                    return this.db.CreateWishLists(new List<WishList> { new WishList(Guid.NewGuid().ToString(), wish.Id, itemWish.Id, (int)itemType) });
                }
            }
            return true;
        }
        public bool DeleteWishItem(string wishIdString, string itemIdString)
        {
            Guid wishId = Helper.SafeGuidParse(wishIdString);
            Guid itemId = Helper.SafeGuidParse(itemIdString);

            if(wishId == new Guid())
            {
                Helper.MyPrint("Wish id invalid.", "r");
                return false;
            }
            else if(itemId == new Guid())
            {
                Helper.MyPrint("Item id is invalid.", "r");
                return false;
            }
            else
            {
                Wish wish = this.db.GetSingleWish(wishId);
                if(wish == null)
                {
                    Helper.MyPrint($"Error: Wish doesn't exist {wishId.ToString()}.");
                    return false;
                }
                List<WishList> wishLists = this.db.GetWishListByWish( Helper.SafeGuidParse(wish.Id) );
                // TODO: FIX THIS AND IMPLEMENT LINQ, search for item id in the wishlists using linq
                return true;
            }
        }
        public bool DeleteWish(string wishIdString)
        {
            Guid wishId = Helper.SafeGuidParse(wishIdString);
            if(wishId == new Guid())
            {
                Helper.MyPrint("Wish id invalid.");
                return false;
            }
            else
            {
                Wish wish = this.db.GetSingleWish(wishId);
                if(wish == null)
                {
                    Helper.MyPrint($"Error: Wish doesn't exist {wish.Id}.");
                    return false;
                }
                bool wishDelete = this.db.DeleteWish(wishId);
                List<WishList> wishLists = this.db.GetWishListByWish(wishId);
                bool wishListDelete = this.db.DeleteWishLists(wishLists);
                return wishDelete && wishListDelete;
            }
        }





        private bool CheckCycle(Guid parent, Guid child)
        {
            List<WishList> wishLists1 = this.db.GetWishListByWish(parent);
            List<WishList> wishLists2 = this.db.GetWishListByWish(child);
            Dictionary<string, List<string>> graph = new Dictionary<string, List<string>>();
            Dictionary<string, bool> visited1 = new Dictionary<string, bool>();
            Dictionary<string, bool> visited2 = new Dictionary<string, bool>();

            graph[parent.ToString()] = new List<string>();
            visited1[parent.ToString()] = false;
            visited2[parent.ToString()] = false;

            graph[child.ToString()] = new List<string>();
            visited1[child.ToString()] = false;
            visited2[child.ToString()] = false;
            // TODO: can incorporate linq here
            foreach (WishList item in wishLists1)
            {
                if (item.ItemType == (int)ItemType.Wish)
                {
                    string wishId = item.WishId;
                    string itemId = item.ItemId;
                    if (!graph.ContainsKey(wishId))
                    {
                        graph[wishId] = new List<string>();
                        visited1[wishId] = false;
                        visited2[wishId] = false;
                    }
                    if (!graph.ContainsKey(itemId))
                    {
                        graph[itemId] = new List<string>();
                        visited1[itemId] = false;
                        visited2[itemId] = false;
                    }
                    graph[wishId].Add(itemId);
                }
            }

            //foreach (KeyValuePair<string, List<string>> item in graph)
            //{
            //    StringBuilder stringBuilder = new StringBuilder();
            //    item.Value.ForEach(delegate (string edge)
            //    {
            //        stringBuilder.Append(edge + ",");
            //    });
            //    Console.WriteLine($"{item.Key} -> {stringBuilder.ToString()}");
            //}
            visited1[parent.ToString()] = true;
            visited2[child.ToString()] = true;
            bool fromParent = this.CycleDetection(graph, visited1, parent.ToString(), child.ToString());
            bool fromChild = this.CycleDetection(graph, visited2, child.ToString(), parent.ToString());
            //Helper.MyPrint($"Parent: {parent.ToString()} -> {fromParent}");
            //Helper.MyPrint($"Child: {child.ToString()} -> {fromChild}");
            return fromParent || fromChild;
        }

        // it returns true if cycle exists other wise false
        private bool CycleDetection(Dictionary<string, List<string>> graph, Dictionary<string, bool> visited, string start, string end)
        {
            foreach (string edge in graph[start])
            {
                if (edge == end)
                {
                    return true;
                }
                else
                {
                    if (visited[edge] == false)
                    {
                        visited[edge] = true;
                        return CycleDetection(graph, visited, edge, end);
                    }
                    else
                    {
                        return true; // back edge exists
                    }
                }
            }
            return false;
        }
   


    // true means operation successful, false means operation not successful
    public bool CheckCycleInWishList(string  parentWish, string childWish)
        {
            Guid parentId = Helper.SafeGuidParse(parentWish);
            Guid childId = Helper.SafeGuidParse(childWish);
            if(parentId == new Guid())
            {
                Helper.MyPrint("Invalid source id");
                return false;
            }
            else if(childId  == new Guid())
            {
                Helper.MyPrint("New id is invalid");
                return false;
            }else if (this.CheckCycle(parentId, childId))
            {

                Helper.MyPrint("Cycle Exists.");
                return false;
            }

            return true;
        }
    }
}
