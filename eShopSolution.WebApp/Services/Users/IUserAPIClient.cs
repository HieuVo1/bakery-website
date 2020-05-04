using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.System.Roles;
using eShopSolution.ViewModel.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Users
{
    public interface IUserAPIClient
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);
        Task<ApiResult<UserViewModel>> GetUserById(Guid userId);
        Task<ApiResult<UserViewModel>> GetUserByEmail(string email);
        Task<ApiResult<UserViewModel>> GetUserByUserName(string userName);
        Task<ApiResult<string>> Register(RegisterRequest request);


    }
}
