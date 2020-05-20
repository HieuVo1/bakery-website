using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.System.Roles;
using eShopSolution.ViewModel.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Users
{
    public interface IUserAPIClient
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);
        Task<ApiResult<string>> ExternalLoginCallback(ExternalLoginRequest request);
        Task<ApiResult<string>> ConfirmEmail(VerificationViewModel request);
        Task<ApiResult<string>> GetPasswordResetToken(string email);
        Task<ApiResult<string>> ResetPassword(ResetPasswordViewModel request);
        Task<ApiResult<string>> ChangePassword(ChangePasswordViewModel request);
        Task<ApiResult<UserViewModel>> GetUserById(Guid userId);
        Task<ApiResult<UserViewModel>> GetUserByEmail(string email);
        Task<ApiResult<UserViewModel>> GetUserByUserName(string userName);
        Task<ApiResult<VerificationViewModel>> Register(RegisterRequest request);


    }
}
