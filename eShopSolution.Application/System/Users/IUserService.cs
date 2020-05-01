using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.System.Roles;
using eShopSolution.ViewModel.System.Users;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Users
{
    public interface IUserService
    {
        Task<ApiResult<string>> Authencate(LoginRequest request);
        Task<ApiResult<bool>> Register(RegisterRequest request);
        Task<ApiResult<PageViewModel<UserViewModel>>> GetListUser(GetUserPaggingRequest request);
        Task<ApiResult<UserViewModel>> GetById(Guid  userId);
        Task<ApiResult<bool>> Delete(Guid userId);
        Task<ApiResult<bool>> Update(Guid userId,UserUpdateRequest request);
        Task<ApiResult<PageViewModel<RoleViewModel>>> GetListRole();
      
    }
}
