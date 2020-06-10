using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Contact;
using System.Threading.Tasks;

namespace eShopSolution.Application.Contacts
{
    public interface IContactService
    {
        Task<ApiResult<bool>> Create(ContactViewModel request);
    }
}
