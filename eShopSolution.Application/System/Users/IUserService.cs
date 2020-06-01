using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.System.Roles;
using eShopSolution.ViewModel.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
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
        Task<ApiResult<string>> ExternalLoginCallback(ExternalLoginRequest request);
        Task<ApiResult<string>> GetPasswordResetToken(string email);
        Task<ApiResult<bool>> ResetPassword(ResetPasswordViewModel request);
        Task<ApiResult<string>> ChangePassword(ChangePasswordViewModel request);
        Task<ApiResult<VerificationViewModel>> Register(RegisterRequest request);
        Task<ApiResult<bool>> ConfirmEmail(VerificationViewModel request);
        Task<ApiResult<PageViewModel<UserViewModel>>> GetListUser(GetUserPaggingRequest request);
        Task<ApiResult<UserViewModel>> GetById(Guid  userId);
        Task<ApiResult<UserViewModel>> GetByEmail(string  email);
        Task<ApiResult<UserViewModel>> GetByUserName(string  userName);
        Task<ApiResult<bool>> Delete(Guid userId);
        Task<ApiResult<bool>> Update(Guid userId,UserUpdateRequest request);
        Task<ApiResult<bool>> UserUpdateProfile(Guid userId,UpdateProfile request);
        Task<ApiResult<PageViewModel<RoleViewModel>>> GetListRole();
      
    }
}
