using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Email;
using System.Threading.Tasks;

namespace eShopSolution.Application.Comom
{
    public interface IEmailService
    {
        Task<ApiResult<string>> SendEmailAsync(EmailMessage message);
        ApiResult<string> SendEmail(EmailMessage message);
    }
}
