using OnlineShoppingStore.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShoppingStore.Models.Home
{
    public class StoreList
    {
        public IList<ShopData> storeList { get; set; }
    }
}