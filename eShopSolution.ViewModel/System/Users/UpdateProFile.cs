using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.System.Users
{
    public class UpdateProfile
    {
        public string Phone { set; get; }
        public string FullName { set; get; }
        public DateTime Dob { set; get; }
        public string Address { set; get; }
        public IFormFile ThumbnailImage { get; set; }
    }
}
