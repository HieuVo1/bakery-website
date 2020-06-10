using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.System.Roles;
using eShopSolution.ViewModel.System.Users;
using eShopSolution.WebApp.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Users
{
    public class UserAPIClient :BaseService, IUserAPIClient
    {
        public UserAPIClient(IHttpClientFactory httpClientFactory, 
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration):base(httpClientFactory,httpContextAccessor,configuration)
        {
        }
        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            return await CreateAsync<ApiResult<string>, LoginRequest>("/api/users/authenticate", request);
        }
        public async Task<ApiResult<UserViewModel>> GetUserByEmail(string email)
        {
            return await GetAsync<ApiResult<UserViewModel>>($"/api/users/GetByEmail/{email}");
        }

        public async Task<ApiResult<UserViewModel>> GetUserById(Guid userId)
        {
            var sections = _httpContextAccessor.HttpContext.Request.Cookies.GetObjectFromJson<string>("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            return await GetAsync<ApiResult<UserViewModel>>($"/api/users/{userId}");
        }
        public async Task<ApiResult<UserViewModel>> GetUserByUserName(string userName)
        {
            return await GetAsync<ApiResult<UserViewModel>>($"/api/users/GetByUserName/{userName}");
           
        }


        public async Task<ApiResult<string>> ExternalLoginCallback(ExternalLoginRequest request)
        {
            return await CreateAsync<ApiResult<string>, ExternalLoginRequest>($"/api/users/ExternalSignIn/", request);
        }

        public async Task<ApiResult<VerificationViewModel>> Register(RegisterRequest request)
        {
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(request.FullName), "FullName");
            form.Add(new StringContent(request.UserName), "UserName");
            form.Add(new StringContent(request.Passwork), "Passwork");
            form.Add(new StringContent(request.Phone), "Phone");
            form.Add(new StringContent(request.RoleId.ToString()), "RoleId");
            form.Add(new StringContent(request.Email), "Email");
            form.Add(new StringContent(request.Dob.ToString()), "Dob");
            form.Add(new StringContent(request.ConfirmPasswork), "ConfirmPasswork");
            form.Add(new StringContent(request.Address), "Address");
            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                ByteArrayContent bytes = new ByteArrayContent(data);
                form.Add(bytes, "ThumbnailImage", request.ThumbnailImage.FileName);
            }
            return await CreateWithImageAsync<ApiResult<VerificationViewModel>>($"/api/users/register", form);
        }

        public async Task<ApiResult<string>> ConfirmEmail(VerificationViewModel request)
        {
            return await CreateAsync<ApiResult<string>, VerificationViewModel>($"/api/users/confirmEmail", request);
        }

        public async Task<ApiResult<string>> GetPasswordResetToken(string email)
        {
            return await GetAsync<ApiResult<string>>($"/api/users/getPasswordResetToken/{email}");
        }

        public async Task<ApiResult<string>> ResetPassword(ResetPasswordViewModel request)
        {
            return await CreateAsync<ApiResult<string>, ResetPasswordViewModel>($"/api/users/ResetPassword", request);
        }

        public async Task<ApiResult<string>> ChangePassword(ChangePasswordViewModel request)
        {
            return await CreateAsync<ApiResult<string>, ChangePasswordViewModel>($"/api/users/ChangePassword", request);
        }

        public async Task<ApiResult<string>> Update(Guid userId, UpdateProfile request)
        { 
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(request.FullName), "FullName");
            form.Add(new StringContent(request.Phone), "Phone");
            form.Add(new StringContent(request.Address), "Address");
            form.Add(new StringContent(request.Dob.ToString()), "Dob");
            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                ByteArrayContent bytes = new ByteArrayContent(data);
                form.Add(bytes, "ThumbnailImage", request.ThumbnailImage.FileName);
            }
            return await UpdateWithImageAsync<ApiResult<string>>($"/api/users/userUpdate/{userId}", form);
        }
    }
}
