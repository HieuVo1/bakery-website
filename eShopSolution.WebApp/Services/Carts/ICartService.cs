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
        Task<ApiResult<string>> Create(CartCreateRequest request);
        Task<ApiResult<string>> Update(CartUpdateRequest request);
        Task<ApiResult<string>> Delete(int cartId);
        Task<ApiResult<string>> AddToCart(CartItemRequest request);
        Task<ApiResult<string>> UpdateQuantity(CartItemRequest request);
        Task<ApiResult<string>> DeleteItem(CartItemRequest request);
        Task<ApiResult<string>> DeleteAll(int cartId);
    }
}
