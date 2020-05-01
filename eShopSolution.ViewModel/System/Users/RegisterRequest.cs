using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModel.System.Users
{
    public class RegisterRequest
    {
        public DateTime Dob { set; get; }
        public string Email { set; get; }
        public string UserName { set; get; }
        public string Phone { set; get; }
        [DataType(DataType.Password)]
        public string Passwork { set; get; }
        [DataType(DataType.Password)]
        public string ConfirmPasswork { set; get; }
        public string FullName { set; get; }
        public Guid RoleId { set; get; }
        public IFormFile ThumbnailImage { get; set; }

    }
}
