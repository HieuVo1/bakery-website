using eShopSolution.ViewModel.Catalog.Carts;
using eShopSolution.ViewModel.Catalog.Carts.CartItems;
using eShopSolution.ViewModel.Common;
using System;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catelog.Carts
{
    public interface ICartService
    {
        Task<ApiResult<CartViewModel>> GetById(Guid userId);
        Task<ApiResult<bool>> Create(CartCreateRequest request);
        Task<ApiResult<bool>> Update(CartUpdateRequest request);
        Task<ApiResult<bool>> Delete(int cartId);
        Task<ApiResult<bool>> AddToCart(CartItemRequest request);
        Task<ApiResult<bool>> UpdateQuantity(CartItemRequest request);
        Task<ApiResult<bool>> DeleteItem(int cartId,int productId,decimal priceChange);
        Task<ApiResult<bool>> DeleteAll(int cartId);
    }
}
