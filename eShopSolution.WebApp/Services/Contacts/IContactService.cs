using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Contact;
using System.Threading.Tasks;

namespace eShopSolution.WebApp.Services.Contacts
{
    public interface IContactService
    {
        Task<ApiResult<string>> Create(ContactViewModel request);
    }
}
