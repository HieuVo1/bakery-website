using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.Catelog.Carts;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Catalog.Carts;
using eShopSolution.ViewModel.Catalog.Carts.CartItems;
using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.WebApp.Helpers;
using eShopSolution.WebApp.Services.products;
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
            List<CartItemViewModel> cart = new List<CartItemViewModel>();
            cart = CookieHelpers.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Request.Cookies, CartSessionKey);
            ViewBag.cart = cart;
            ViewBag.count = cart.Count();
            ViewBag.total = (cart != null) ? cart.Sum(item => item.Product.Price * item.Quantity) : 0;
            if (section != null)
            {
                ViewBag.IsLogged = true;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> BuyAsync(CartItemCreateRequest request) {
            var product = await _productService.GetById(request.ProductID, languageDefauleId);
            if (section != null)
            {
                var add = await _cartService.AddToCart(request);
                if (add.IsSuccessed)
                {
                    SaveToCookie(product.ResultObject,request.Quantity);
                    return RedirectToAction("index","cart");
                }
                return RedirectToAction("index", "cart");
            }
            else
            {
                SaveToCookie(product.ResultObject, request.Quantity);
                return RedirectToAction("index", "cart");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(CartItemUpdateRequest request)
        {
            if (section != null)
            {
                var add = await _cartService.UpdateQuantity(request);
                if (add.IsSuccessed)
                {
                    UpdateToCookie(request.ProductID, request.Quantity);
                    return Ok();
                }
                return BadRequest();
            }
            else
            {
                UpdateToCookie(request.ProductID, request.Quantity);
                return Ok();
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int cartId,int productId)
        {
            if (section != null)
            {
                var add = await _cartService.DeleteItem(cartId, productId);
                if (add.IsSuccessed)
                {
                    DeleteItemFromCookie(productId);
                    return Ok();
                }
                return BadRequest();
            }
            else
            {
                DeleteItemFromCookie(productId);
                return Ok();
            }
        }
        public void SaveToCookie(ProductViewModel product,int quantity)
        {
            if (CookieHelpers.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Request.Cookies, CartSessionKey) == null)
            {
                List<CartItemViewModel> cart = new List<CartItemViewModel>();
                cart.Add(new CartItemViewModel { Product = product, Quantity = 1 });
                CookieHelpers.SetObjectAsJson(HttpContext.Response.Cookies, CartSessionKey, cart,null);
            }
            else
            {
                List<CartItemViewModel> cart = CookieHelpers.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Request.Cookies, CartSessionKey);
                int index = cart.FindIndex(x => x.Product.Id == product.Id);
                if (index != -1)
                {
                    cart[index].Quantity+= quantity;
                }
                else
                {
                    cart.Add(new CartItemViewModel { Product = product, Quantity = quantity });
                }
                CookieHelpers.SetObjectAsJson(HttpContext.Response.Cookies, CartSessionKey, cart,null);
            }
        }
        public void UpdateToCookie(int productId,int quantity)
        {
                List<CartItemViewModel> cart = CookieHelpers.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Request.Cookies, CartSessionKey);
                int index = cart.FindIndex(x => x.Product.Id == productId);
                if (index != -1)
                {
                    cart[index].Quantity= quantity;
                    CookieHelpers.SetObjectAsJson(HttpContext.Response.Cookies, CartSessionKey, cart, null);
                }
        }
        public void DeleteItemFromCookie(int productId)
        {
            List<CartItemViewModel> cart = CookieHelpers.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Request.Cookies, CartSessionKey);
            int index = cart.FindIndex(x => x.Product.Id == productId);
            if (index != -1)
            {
                cart.RemoveAt(index);
                CookieHelpers.SetObjectAsJson(HttpContext.Response.Cookies, CartSessionKey, cart, null);
            }
        }
        [HttpGet]
        public IActionResult GetCart()
        {
            return PartialView("_Cart");
        }
    }
}