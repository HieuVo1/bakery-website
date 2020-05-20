using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.Contact;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.Data.Entities;
using eShopSolution.Data.EF;
using eShopSolution.Application.Comom;

namespace eShopSolution.Application.Contacts
{
    public class ContactService : IContactService
    {
        private readonly EShopDbContext _context;
        public ContactService(EShopDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> Create(ContactViewModel request)
        {
            var contact = new Contact()
            {
                Name = request.FullName,
                Email = request.Email,
                Message = request.Messmage,
            };
            _context.Contacts.Add(contact);
            return await SaveChangeService.SaveChangeAsyncNotImage(_context);

        }
    }
}
