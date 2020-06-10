using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Email;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Emails
{
    public class EmailService :BaseService, IEmailService
    {
        public EmailService(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
        }
        public async Task<ApiResult<string>> SendEmail(EmailMessage message)
        {
            return await CreateAsync<ApiResult<string>, EmailMessage>($"/api/emails/SendMessage/", message);
        }
    }
}
