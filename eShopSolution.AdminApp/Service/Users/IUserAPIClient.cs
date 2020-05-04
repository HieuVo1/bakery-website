using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.System.Roles;
using eShopSolution.ViewModel.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Service.Users
{
    public interface IUserAPIClient
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);
        Task<ApiResult<PageViewModel<UserViewModel>>> getListUser(GetUserPaggingRequest request);
        Task<ApiResult<UserViewModel>> getUserById(Guid userId);
        Task<ApiResult<PageViewModel<RoleViewModel>>> getListRole();
        Task<ApiResult<string>> Register(RegisterRequest request);
        Task<ApiResult<string>> Delete(Guid userId);
        Task<ApiResult<string>> Update(Guid userId,UserUpdateRequest request);
        Task<ApiResult<UserViewModel>> GetUserByEmail(string email);
        Task<ApiResult<UserViewModel>> GetUserByUserName(string userName);
    }
}
