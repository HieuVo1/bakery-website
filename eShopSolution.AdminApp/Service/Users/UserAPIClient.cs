using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.System.Roles;
using eShopSolution.ViewModel.System.Users;
using Microsoft.AspNetCore.Http;
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

namespace eShopSolution.AdminApp.Service.Users
{
    public class UserAPIClient : IUserAPIClient
    {
        
        private readonly IHttpClientFactory _httpClientFactor;
        private HttpClient _client;
        private readonly IConfiguration _configuration;
        private IHttpContextAccessor _httpContextAccessor;
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

        public async Task<ApiResult<string>> Delete(Guid userId)
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            var response = await _client.DeleteAsync($"/api/users/{userId}");
            var data = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(data);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(data);
        }

        public async Task<ApiResult<PageViewModel<RoleViewModel>>> getListRole()
        {
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            var response = await _client.GetAsync($"/api/users/getListRole");
           
            using (HttpContent content = response.Content)
            {
                //convert data content to string using await
                var data = await content.ReadAsStringAsync();

                //If the data is not null, parse(deserialize) the data to a C# object
                if (data != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<ApiResultSuccess<PageViewModel<RoleViewModel>>>(data);
                    }
                    return JsonConvert.DeserializeObject<ApiResultErrors<PageViewModel<RoleViewModel>>>(data);

                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<ApiResult<PageViewModel<UserViewModel>>> getListUser(GetUserPaggingRequest request)
        {
            //https://locahost:port/products/?PageIndex=1&pageSize=1&categoryId=1
            var sections = _httpContextAccessor.HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sections);
            var response = await _client.GetAsync($"/api/users/getListUser?pageIndex={request.PageIndex}&pageSize={request.PageSize}&keyWord={request.Keyword}");
            using (HttpContent content = response.Content)
            {
                //convert data content to string using await
                var data = await content.ReadAsStringAsync();

                //If the data is not null, parse(deserialize) the data to a C# object
                if (data != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<ApiResultSuccess<PageViewModel<UserViewModel>>>(data);
                    }
                    return JsonConvert.DeserializeObject<ApiResultErrors<PageViewModel<UserViewModel>>>(data);

                }
                else
                {
                    return null;
                }
            }
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

        public async Task<ApiResult<UserViewModel>> getUserById(Guid userId)
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

        public async Task<ApiResult<string>> Register(RegisterRequest request)
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
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(result);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(result);
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

            var response = await _client.PatchAsync($"/api/users/{userId}", form);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResultSuccess<string>>(result);
            }
            return JsonConvert.DeserializeObject<ApiResultErrors<string>>(result);
        }
    }
}
