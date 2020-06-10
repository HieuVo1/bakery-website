using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.System.Roles;
using eShopSolution.ViewModel.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Service.Users
{
    public class UserAPIClient :BaseService, IUserAPIClient
    {
        public UserAPIClient(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
        }
        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            return await CreateAsync<ApiResult<string>, LoginRequest>("/api/users/authenticate", request);
        }

        public async Task<ApiResult<string>> Delete(Guid userId)
        {
            return await DeleteAsync<ApiResult<string>>($"/api/users/{userId}");
        }

        public async Task<ApiResult<PageViewModel<RoleViewModel>>> getListRole()
        {
            return await GetAsync<ApiResult<PageViewModel<RoleViewModel>>>($"/api/users/getListRole");
        }

        public async Task<ApiResult<PageViewModel<UserViewModel>>> getListUser(GetUserPaggingRequest request)
        {
            return await GetAsync<ApiResult<PageViewModel<UserViewModel>>>($"/api/users/getListUser?pageIndex={request.PageIndex}&pageSize={request.PageSize}&keyWord={request.Keyword}");
        }

        public async Task<ApiResult<UserViewModel>> GetUserByEmail(string email)
        {
            return await GetAsync<ApiResult<UserViewModel>>($"/api/users/GetByEmail/{email}");
        }

        public async Task<ApiResult<UserViewModel>> getUserById(Guid userId)
        {
            return await GetAsync<ApiResult<UserViewModel>>($"/api/users/{userId}");
        }

        public async Task<ApiResult<UserViewModel>> GetUserByUserName(string userName)
        {
            return await GetAsync<ApiResult<UserViewModel>>($"/api/users/GetByUserName/{userName}");
        }

        public async Task<ApiResult<string>> Register(RegisterRequest request)
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
            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                ByteArrayContent bytes = new ByteArrayContent(data);
                form.Add(bytes, "ThumbnailImage", request.ThumbnailImage.FileName);
            }
            return await CreateWithImageAsync<ApiResult<string>>($"/api/users/register", form);
        }

        public async Task<ApiResult<string>> Update(Guid userId, UserUpdateRequest request)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            MultipartFormDataContent form = new MultipartFormDataContent();
            form.Add(new StringContent(request.FullName), "FullName");
            form.Add(new StringContent(request.Phone), "Phone");
            form.Add(new StringContent(request.RoleId.ToString()), "RoleId");
            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                ByteArrayContent bytes = new ByteArrayContent(data);
                form.Add(bytes, "ThumbnailImage", request.ThumbnailImage.FileName);
            }
            return await UpdateWithImageAsync<ApiResult<string>>($"/api/users/{userId}", form);
        }
    }
}
