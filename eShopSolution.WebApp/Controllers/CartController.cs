using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.Catelog.Carts;
using eShopSolution.ViewModel.Catalog.Carts.CartItems;
using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.WebApp.Helpers;
using eShopSolution.WebApp.Services.products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace eShopSolution.WebApp.Controllers
{
    public class CartController : BaseController
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private const string CartSessionKey = "CartSessionKey";
        public CartController(IProductService productService,
            ICartService cartService,
            IConfiguration configuration) : base(configuration)
        {
            _productService = productService;
            _cartService = cartService;
        }
        public IActionResult Index()
        {
            var x = GetCartAsync();
            List<CartItemViewModel> cartItems = new List<CartItemViewModel>();
            //cart = CookieHelpers.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Request.Cookies, CartSessionKey);
            if (HttpContext.Session.GetObjectFromJson<List<CartItemViewModel>>(CartSessionKey) != null)
            {
                cartItems = HttpContext.Session.GetObjectFromJson<List<CartItemViewModel>>(CartSessionKey);
            }
            ViewBag.cart = cartItems;
            ViewBag.count = cartItems.Count();
            ViewBag.total = (cartItems != null) ? cartItems.Sum(item => item.Product.Price * item.Quantity) : 0;
            if (section != null)
            {
                ViewBag.IsLogged = true;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> BuyAsync(CartItemRequest request) {
            var product = await _productService.GetById(request.ProductID, languageDefauleId);
            if (section != null)
            {
                request.PriceChange = product.ResultObject.Price * request.Quantity;
                var add = await _cartService.AddToCart(request);
                if (add.IsSuccessed)
                {
                    SaveToSession(product.ResultObject,request.Quantity);
                    return RedirectToAction("index","cart");
                }
                return RedirectToAction("index", "cart");
            }
            else
            {
                SaveToSession(product.ResultObject, request.Quantity);
                return RedirectToAction("index", "cart");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(CartItemRequest request)
        {
            if (section != null)
            {
                var add = await _cartService.UpdateQuantity(request);
                if (add.IsSuccessed)
                {
                    UpdateToSession(request.ProductID, request.Quantity);
                    return Ok();
                }
                return BadRequest();
            }
            else
            {
                UpdateToSession(request.ProductID, request.Quantity);
                return Ok();
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(CartItemRequest request)
        {
            if (section != null)
            {
                var add = await _cartService.DeleteItem(request);
                if (add.IsSuccessed)
                {
                    DeleteItemFromSession(request.ProductID);
                    return Ok();
                }
                return BadRequest();
            }
            else
            {
                DeleteItemFromSession(request.ProductID);
                return Ok();
            }
        }
        public void SaveToSession(ProductViewModel product,int quantity)
        {
            var cartItems = HttpContext.Session.GetObjectFromJson<List<CartItemViewModel>>(CartSessionKey);
            if (cartItems == null)
            {
                List<CartItemViewModel> newCartItems = new List<CartItemViewModel>();
                newCartItems.Add(new CartItemViewModel { Product = product, Quantity = 1 });
                HttpContext.Session.SetObjectAsJson(CartSessionKey, newCartItems);
                //CookieHelpers.SetObjectAsJson(HttpContext.Response.Cookies, CartSessionKey, cart,null);
            }
            else
            {
                //List<CartItemViewModel> cartItems = CookieHelpers.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Request.Cookies, CartSessionKey);
                int index = cartItems.FindIndex(x => x.Product.Id == product.Id);
                if (index != -1)
                {
                    cartItems[index].Quantity+= quantity;
                }
                else
                {
                    cartItems.Add(new CartItemViewModel { Product = product, Quantity = quantity });
                }
                HttpContext.Session.SetObjectAsJson(CartSessionKey, cartItems);
                //CookieHelpers.SetObjectAsJson(HttpContext.Response.Cookies, CartSessionKey, cart,null);
            }
        }
        public void UpdateToSession(int productId,int quantity)
        {
            //List<CartItemViewModel> cartItems = CookieHelpers.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Request.Cookies, CartSessionKey);
            List<CartItemViewModel> cartItems = HttpContext.Session.GetObjectFromJson<List<CartItemViewModel>>(CartSessionKey);
                int index = cartItems.FindIndex(x => x.Product.Id == productId);
                if (index != -1)
                {
                    cartItems[index].Quantity= quantity;
                    HttpContext.Session.SetObjectAsJson(CartSessionKey, cartItems);
                    //CookieHelpers.SetObjectAsJson(HttpContext.Response.Cookies, CartSessionKey, cart, null);
                }
        }
        public void DeleteItemFromSession(int productId)
        {
            //List<CartItemViewModel> cart = CookieHelpers.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Request.Cookies, CartSessionKey);
            List<CartItemViewModel> cartItems = HttpContext.Session.GetObjectFromJson<List<CartItemViewModel>>(CartSessionKey);
            int index = cartItems.FindIndex(x => x.Product.Id == productId);
            if (index != -1)
            {
                cartItems.RemoveAt(index);
                HttpContext.Session.SetObjectAsJson(CartSessionKey, cartItems);
                //CookieHelpers.SetObjectAsJson(HttpContext.Response.Cookies, CartSessionKey, cart, null);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCartAsync()
        {
            var str = await RenderViewToString.RenderViewToStringAsync(this,"_Cart");
            return Ok(str);
        }
    }
}