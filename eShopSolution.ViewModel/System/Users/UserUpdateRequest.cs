using Microsoft.AspNetCore.Http;
using System;

namespace eShopSolution.ViewModel.System.Users
{
    public class UserUpdateRequest
    {
        public string Phone { set; get; }
        public string FullName { set; get; }
        public Guid RoleId { set; get; }
        public IFormFile ThumbnailImage { get; set; }
    }
}
