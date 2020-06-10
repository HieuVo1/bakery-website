using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Contact;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Contacts
{
    public class ContactService :BaseService, IContactService
    {
        public ContactService(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
        }
        public async Task<ApiResult<string>> Create(ContactViewModel request)
        {
            return await CreateAsync<ApiResult<string>, ContactViewModel>($"/api/contacts", request);
        }
    }
}
