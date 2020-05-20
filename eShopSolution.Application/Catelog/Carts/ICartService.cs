using eShopSolution.ViewModel.Catalog.Carts;
using eShopSolution.ViewModel.Catalog.Carts.CartItems;
using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catelog.Carts
{
    public interface ICartService
    {
        Task<ApiResult<CartViewModel>> GetById(Guid userId);
        Task<ApiResult<string>> Create(CartCreateRequest request);
        Task<ApiResult<bool>> Update(CartUpdateRequest request);
        Task<ApiResult<bool>> Delete(int cartId);
        Task<ApiResult<bool>> AddToCart(CartItemCreateRequest request);
        Task<ApiResult<bool>> UpdateQuantity(CartItemUpdateRequest request);
        Task<ApiResult<bool>> DeleteItem(int cartId,int productId);
    }
}
