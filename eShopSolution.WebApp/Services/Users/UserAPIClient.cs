using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.System.Roles;
using eShopSolution.ViewModel.System.Users;
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
    public class UserAPIClient : IUserAPIClient
    {

        private readonly IHttpClientFactory _httpClientFactor;
        private HttpClient _client;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public UserAPIClient(IHttpClientFactory httpClientFactory, 
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _httpClientFactor = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _client = _httpClientFactor.CreateClient();
            _configuration = configuration;
            var baseUrl = _configuration.GetSection("BackendUrlBase").Value;
            _client.BaseAddress = new Uri(baseUrl);
        }
        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/users/authenticate", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(await response.Content.ReadAsStringAsync());
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(await response.Content.ReadAsStringAsync());
        }
        public async Task<ApiResult<UserViewModel>> GetUserByEmail(string email)
        { 
            var response = await _client.GetAsync($"/api/users/GetByEmail/{email}");
            var data = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<UserViewModel>>(data);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<UserViewModel>>(data);
        }

        public async Task<ApiResult<UserViewModel>> GetUserById(Guid userId)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            var response = await _client.GetAsync($"/api/users/{userId}");
            var data = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<UserViewModel>>(data);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<UserViewModel>>(data);
        }

        public async Task<ApiResult<UserViewModel>> GetUserByUserName(string userName)
        {
            var response = await _client.GetAsync($"/api/users/GetByUserName/{userName}");
            var data = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<UserViewModel>>(data);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<UserViewModel>>(data);
        }


        public async Task<ApiResult<string>> ExternalLoginCallback(ExternalLoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/api/users/ExternalSignIn/", httpContent);
            var data = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(data);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(data);
        }

        public async Task<ApiResult<VerificationViewModel>> Register(RegisterRequest request)
        {
            //_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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

            var response = await _client.PostAsync($"/api/users/register", form);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<VerificationViewModel>>(result);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<VerificationViewModel>>(result);
        }

        public async Task<ApiResult<string>> ConfirmEmail(VerificationViewModel request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/api/users/confirmEmail", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(result);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(result);
        }

        public async Task<ApiResult<string>> GetPasswordResetToken(string email)
        {
            var response = await _client.GetAsync($"/api/users/getPasswordResetToken/{email}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(result);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(result);
        }

        public async Task<ApiResult<string>> ResetPassword(ResetPasswordViewModel request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/api/users/ResetPassword", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(result);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(result);
        }

        public async Task<ApiResult<string>> ChangePassword(ChangePasswordViewModel request)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/api/users/ChangePassword", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(result);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(result);
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

            var response = await _client.PatchAsync($"/api/users/userUpdate/{userId}", form);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(result);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(result);
        }
    }
}
