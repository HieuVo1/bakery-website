using eShopSolution.Application.Comom;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Catalog.Carts;
using eShopSolution.ViewModel.Catalog.Carts.CartItems;
using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.ViewModel.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catelog.Carts
{
    public class CartService : ICartService
    {
        private readonly EShopDbContext _context;
        public CartService(EShopDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> AddToCart(CartItemCreateRequest request)
        {
            var cartItem = await _context.CartProducts.FirstOrDefaultAsync(x => x.CartID == request.CartID && x.ProductID == request.ProductID);
            if (cartItem != null) {
                cartItem.Quantity += request.Quantity;
                return await SaveChangeService.SaveChangeAsyncNotImage(_context);
            }
            else
            {
                var item = new CartProduct
                {
                    CartID = request.CartID,
                    ProductID = request.ProductID,
                    Quantity = request.Quantity
                };
                _context.CartProducts.Add(item);
                return await SaveChangeService.SaveChangeAsyncNotImage(_context);
            }
            
        }

        public async Task<ApiResult<string>> Create(CartCreateRequest request)
        {
            var cart = new Cart
            {
                Created_At = DateTime.Now,
                UserId = request.UserId,
                Price = request.Price,
            };
            foreach (var item in request.Items)
            {
                var cartProduct = new CartProduct
                {
                    ProductID = item.Product.Id,
                    Quantity = item.Quantity
                };
                cart.CartProducts.Add(cartProduct);
            }
            _context.Carts.Add(cart);
            _context.Entry(cart).GetDatabaseValues();
            return new ApiResultSuccess<string>(cart.Id.ToString());
        }

        public async Task<ApiResult<bool>> Delete(int cartId)
        {
            var cart = await _context.Carts.FindAsync(cartId);
            if (cart == null) return new ApiResultErrors<bool>($"Can not find cart with id: {cartId}");
            _context.Carts.Remove(cart);
            return await SaveChangeService.SaveChangeAsyncNotImage(_context);
        }

        public async Task<ApiResult<bool>> DeleteItem(int cartId, int productId)
        {
            var cartItem = await _context.CartProducts.FirstOrDefaultAsync(x => x.CartID == cartId
            && x.ProductID == productId);
            if (cartItem == null) return new ApiResultErrors<bool>($"Can not find cartItem");
            _context.CartProducts.Remove(cartItem);
            return await SaveChangeService.SaveChangeAsyncNotImage(_context);
        }

        public async Task<ApiResult<CartViewModel>> GetById(Guid userId)
        {
            
            var cart = await _context.Carts.FirstOrDefaultAsync(x => x.UserId == userId);
            if (cart == null) return new ApiResultErrors<CartViewModel>($"Can not find cart with id: {userId}");
            var query = from c in _context.CartProducts where c.CartID == cart.Id
                        join p in _context.Products on c.ProductID equals p.Id
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join img in _context.ProductImages on p.Id equals img.ProductId
                        select new { c, p,pt,img };
            var cartViewModel = new CartViewModel
            {
                Id = cart.Id,
                UserId = cart.UserId,
                TotalPrice = cart.Price,
                Created_At = cart.Created_At, 
            };
            var data = await query.Select(
                x => new CartItemViewModel 
                { 
                    Product = new ProductViewModel
                    {
                        Price = x.p.Price,
                        Name = x.pt.Name,
                        ImagePath = x.img.ImagePath,
                        Id = x.p.Id
                    },
                   Quantity = x.c.Quantity

                }).ToListAsync();
            cartViewModel.CartItems = data;

            return new ApiResultSuccess<CartViewModel>(cartViewModel);
        }

        public Task<ApiResult<bool>> Update(CartUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<bool>> UpdateQuantity(CartItemUpdateRequest request)
        {
            var cartItem = await _context.CartProducts.FirstOrDefaultAsync(x => x.CartID == request.CartID 
            && x.ProductID == request.ProductID);
            if (cartItem == null)
            {
                var newCartItem = new CartProduct
                {
                    CartID = request.CartID,
                    ProductID = request.ProductID,
                    Quantity = request.Quantity
                };
                _context.CartProducts.Add(newCartItem);
                return await SaveChangeService.SaveChangeAsyncNotImage(_context);
            }
            cartItem.Quantity = request.Quantity;
            return await SaveChangeService.SaveChangeAsyncNotImage(_context);
        }
    }
}
