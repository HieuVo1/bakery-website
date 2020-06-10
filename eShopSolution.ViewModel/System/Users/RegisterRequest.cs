using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace eShopSolution.ViewModel.System.Users
{
    public class RegisterRequest
    {
        public DateTime Dob { set; get; }
        [Remote(action: ("IsEmailUse"), controller: ("Login"))]
        public string Email { set; get; }
        [Remote(action:("IsUserNameUse"),controller:("Login"))]
        public string UserName { set; get; }
        public string Address { set; get; }
        public string Phone { set; get; }
        public string Passwork { set; get; }
        [Compare("Passwork", ErrorMessage = "password and confirmPassword is not match")]
        public string ConfirmPasswork { set; get; }
        public string FullName { set; get; }
        public Guid RoleId { set; get; }
        public IFormFile ThumbnailImage { get; set; }

    }
}
