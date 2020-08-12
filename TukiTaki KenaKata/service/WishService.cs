using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.model;
using TukiTaki_KenaKata.persistant;
using Autofac;
using TukiTaki_KenaKata.persistant.model;
using TukiTaki_KenaKata.service.mapper;
using System.Linq;
using TukiTaki_KenaKata.service.model;

namespace TukiTaki_KenaKata.service
{
    class WishService : IWishService
    {
        IDBRepository db;
        Dictionary<string, bool> visited;
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
            List<Product> allProd= this.db.GetAllProduct();
            List<WishList> allWishLists = this.db.GetAllWishList();
            List<ProductDTO> allProducts = new List<ProductDTO>();
            allProd.ForEach(delegate(Product product)
            {
                allProducts.Add(ModelToDTOMapper.ProductMapper(product));
            });
            List<WishDTO> wishDTOs = new List<WishDTO>();

            foreach(Wish wish in wishes)
            {
                if(Helper.SafeGuidParse(wish.Id) == new Guid())
                {
                    Helper.MyPrint("Error: Invalid ID", "r");
                }
                else
                {
                    wishDTOs.Add(this.BuildWish(wish, wishes, allProducts, allWishLists));
                }
            }
            return wishDTOs;
        }
        public WishDTO BuildWish(Wish wish, List<Wish> allWishes, List<ProductDTO> allProducts, List<WishList> allWishLists)
        {
            List<Component> myComponents = new List<Component>();
            List<WishList> selectedWishList = allWishLists.Where(s => s.WishId == wish.Id).Select(s => s).ToList();
            foreach (WishList wishlist in selectedWishList)
            {
                if (wishlist.ItemType == (int)ItemType.Product)
                {
                    myComponents.AddRange(allProducts.Where(s => (s.Id.ToString()) == wishlist.ItemId).Select(s => s).ToList());
                }
                else if (wishlist.ItemType == (int)ItemType.Wish)
                {
                    myComponents.Add(this.BuildWish( 
                        allWishes.Where(s=>s.Id == wishlist.ItemId).Select(s=>s).Single(),
                        allWishes,allProducts, allWishLists));
                }
            }
            return new WishDTO(Helper.SafeGuidParse(wish.Id), wish.Name, myComponents);
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
                List<Wish> allWishes = this.db.GetAllWish();
                List<Product> allProd = this.db.GetAllProduct();
                List<WishList> allWishLists = this.db.GetAllWishList();
                List<ProductDTO> allProducts = new List<ProductDTO>();
                allProd.ForEach(delegate (Product product)
                {
                    allProducts.Add(ModelToDTOMapper.ProductMapper(product));
                });
                WishDTO wishDTO = new WishDTO(); 
                Wish wish = this.db.GetSingleWish( Helper.SafeGuidParse(stringId) );
                if(wish != null)
                {
                    return this.BuildWish(wish, allWishes, allProducts, allWishLists);
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
                    Wish itemWish = this.db.GetSingleWish(itemId);
                    if (itemWish == null)
                    {
                        Helper.MyPrint($"Error: Wish doesn't exist {wishId.ToString()}.");
                        return false;
                    }
                    else
                    {
                        if(this.CanAddThisWishToThatWish( wish.Id, itemWish.Id))
                        {
                            //Helper.MyPrint("HI");
                            return this.db.CreateWishLists(new List<WishList> { new WishList(Guid.NewGuid().ToString(), wish.Id, itemWish.Id, (int)itemType) });
                        }
                        else
                        {
                            Helper.MyPrint("Error: Cycle exists, you can't add this.", "r");
                            return false;
                        }
                    }
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
                var linqQuery = wishLists.Where(s=> s.ItemId==itemId.ToString()).Select(s => s);
                List<WishList> itemsToDelete = new List<WishList>();
                foreach(WishList wishList in linqQuery)
                {
                    int z = itemsToDelete.Select(x => x).Where(s => s.ItemId == wishList.ItemId).Count();
                    if(z == 0)
                    {
                        itemsToDelete.Add(wishList);
                    }
                }
                if(itemsToDelete.Count > 0)
                {
                    this.db.DeleteWishLists(itemsToDelete);
                    return true;
                }
                else
                {
                    Helper.MyPrint("Error: Nothing has been deleted.", "r");
                    return false;
                }

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




        private string getWishName(string wishidString)
        {
            return this.db.GetSingleWish(Helper.SafeGuidParse(wishidString)).Name;
        }
        private bool CycleExists(Guid start, Guid end)
        {
            //Helper.MyPrint($"GOT {start} {end}");
            List<WishList> allWishLists = this.db.GetAllWishList();
            Dictionary<string, List<string>> graph1 = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> graph2 = new Dictionary<string, List<string>>();
            this.visited = new Dictionary<string, bool>();
            Dictionary<string, bool> visited2 = new Dictionary<string, bool>();
            graph1[start.ToString()] = new List<string>();
            graph1[end.ToString()] = new List<string>();
            visited[start.ToString()] = false ;
            visited[end.ToString()] = false;
            foreach(WishList wishList in allWishLists)
            {
                if(wishList.ItemType == (int)ItemType.Wish)
                {
                    string wishId = wishList.WishId;
                    string itemId = wishList.ItemId;
                    // to protect from **key not found in the next function
                    if (!graph1.ContainsKey(wishId))
                    {
                        graph1[wishId] = new List<string>();
                    }
                    if (!graph1.ContainsKey(itemId))
                    {
                        graph1[itemId] = new List<string>();
                    }
                    graph1[wishId].Add(itemId);
                    visited[wishId] = false;
                    visited[itemId] = false;
                    //Helper.MyPrint($"{wishId} {itemId}");
                }
            }
            graph1[start.ToString()].Add(end.ToString());
            //foreach(KeyValuePair<String, List<String> > kv in graph1){
            //    string str = Helper.MyOutputString(getWishName(kv.Key) + " -> ", "b");
            //    foreach(string x in kv.Value)
            //    {
            //        str += "" + getWishName(x) + "," ;
            //    }
            //    Console.WriteLine(str);
            //}
            foreach (KeyValuePair<String, List<String>> kv in graph1)
            {
                if (!visited[kv.Key])
                {
                    //visited[kv.Key] = true;
                    bool check = CycleDetection(graph1, kv.Key);
                    if (check == true)
                    {
                        //Helper.MyPrint($"Problem in {kv.Key}");
                        return true;
                    }
                }
            }
            return false;
        }

        // it returns true if cycle exists other wise false
        private bool CycleDetection(Dictionary<string, List<string>> graph, string start)
        {
            //Helper.MyPrint("Start " + this.getWishName(start));
            if (!visited[start])
            {
                visited[start] = true;
                foreach (string edge in graph[start])
                {
                    if (visited[edge] == false && CycleDetection(graph, edge))
                    {
                        return true;
                    }
                    else if(visited[edge] == true)
                    {
                        //Helper.MyPrint("Got Visited " + this.getWishName(edge));
                        return true;
                    }

                }
                //Helper.MyPrint($"{this.getWishName(start)} set to false");
                visited[start] = false;
            }
            return false;
        }



        // true means operation successful, false means operation not successful
        public bool CanAddThisWishToThatWish(string  parentWish, string childWish)
        {
            if(parentWish == childWish)
            {
                return false;
            }
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
            }else
            {
                List<WishList> wishLists = this.db.GetWishListByWish(parentId);
                if(wishLists.Where(x=>x.ItemId == childId.ToString()).Count() > 0)
                {
                    Helper.MyPrint("Error: Can't add same wish multiple times.", "r");
                    return false;
                }
                else if(this.CycleExists(parentId, childId))
                {
                    Helper.MyPrint("Cycle Exists.");
                    return false;
                }
            }
            return true;
        }
    }
}
