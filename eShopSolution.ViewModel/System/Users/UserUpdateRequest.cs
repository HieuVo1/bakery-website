using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.System.Users
{
    public class UserUpdateRequest
    {
        public string Phone { set; get; }
        public string FullName { set; get; }
        public string Email { set; get; }
        public Guid RoleId { set; get; }
        public IFormFile ThumbnailImage { get; set; }
    }
}
