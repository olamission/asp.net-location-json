using Newtonsoft.Json;
using OnlineShoppingStore.DAL;
using OnlineShoppingStore.Models;
using OnlineShoppingStore.Models.Home;
using OnlineShoppingStore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Security.Policy;
using System.Net;

//using PayPal.Api;


namespace OnlineShoppingStore.Controllers
{
    public class HomeController : Controller
    {
        dbMyOnlineShoppingEntities ctx = new dbMyOnlineShoppingEntities();
        public ActionResult Index(string search,int? page)
        {
            HomeIndexViewModel model = new HomeIndexViewModel();
            return View(model.CreateModel(search,4, page));
        }
        public ActionResult Checkout()
        {
            GetStores();
            return View();
        }

        public class newItem {

            public string product { get; set; }
            public string value { get; set; }
            
        }
        public void GetValue(List<newItem> newCart)
        {
            
            foreach (Item item in (List<Item>)Session["cart"])
            {
                Debug.WriteLine(item.Product.ProductName);
                Debug.WriteLine(item.Quantity);

                foreach (newItem newitem in newCart)
                {
                    Debug.WriteLine(newitem.product);
                    Debug.WriteLine(newitem.value);
                    if (newitem.product == item.Product.ProductName) {
                        item.Quantity = Int32.Parse(newitem.value);
                    }
                }
            }
            
            

        }

        public ActionResult CheckoutDetails()
        {
            

            return View();
        }
        public ActionResult DecreaseQty(int productId)
        {
            if (Session["cart"] != null)
            {
                List<Item> cart = (List<Item>)Session["cart"];
                var product = ctx.Tbl_Product.Find(productId);
                foreach (var item in cart)
                {
                    if (item.Product.ProductId == productId)
                    {
                        int prevQty = item.Quantity;
                        if (prevQty > 0)
                        {
                            cart.Remove(item);
                            cart.Add(new Item()
                            {
                                Product = product,
                                Quantity = prevQty - 1
                            });
                        }
                        break;
                    }
                }
                Session["cart"] = cart;
            }
            return Redirect("Checkout");
        }
        public ActionResult AddToCart(int productId,string url)
        {
            if (Session["cart"] == null)
            {
                List<Item> cart = new List<Item>();
                var product = ctx.Tbl_Product.Find(productId);
                cart.Add(new Item()
                {
                    Product = product,
                    Quantity = 1
                });
                Session["cart"] = cart;
            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart"];
                var count = cart.Count();
                var product = ctx.Tbl_Product.Find(productId);
                for (int i = 0; i < count;i++ )
                {
                    if (cart[i].Product.ProductId == productId)
                    {
                        int prevQty = cart[i].Quantity;
                        cart.Remove(cart[i]);
                        cart.Add(new Item()
                        {
                            Product = product,
                            Quantity = prevQty + 1
                        });
                        break;
                    }
                    else
                    {
                        var prd = cart.Where(x => x.Product.ProductId == productId).SingleOrDefault();
                        if (prd == null)
                        {
                            cart.Add(new Item()
                            {
                                Product = product,
                                Quantity = 1
                            });
                        }
                    }
                }
                Session["cart"] = cart;
                /*foreach (Item item in (List<Item>)Session["cart"]) {
                    Debug.WriteLine(item.Product.ProductName);
                    Debug.WriteLine(item.Quantity);
                }*/


            }
            return Redirect(url);
        }
        public ActionResult RemoveFromCart(int productId)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            foreach (var item in cart)
            {
                if (item.Product.ProductId == productId)
                {
                    cart.Remove(item);

                    break;
                }
            }
            Session["cart"] = cart;
            return Redirect("Index");
        }
        [HttpGet]
        public string GetStores() {
            var path = Server.MapPath("~/stores.json");
            dynamic jsonFile = JsonConvert.DeserializeObject(System.IO.File.ReadAllText( path.ToString()));// using both newtonsoft.json and system.IO.File we can get any file from the asp.net
            Debug.WriteLine($"{ jsonFile["storeList"][0]["name"]}");
            return Convert.ToString(jsonFile);
        }


        
    }
}