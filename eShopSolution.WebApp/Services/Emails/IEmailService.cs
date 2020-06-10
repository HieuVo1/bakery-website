using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Email;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Emails
{
    public interface IEmailService
    {
        Task<ApiResult<string>> SendEmail(EmailMessage message);
    }
}
