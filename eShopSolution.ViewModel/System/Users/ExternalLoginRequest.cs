using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModel.System.Users
{
    public class ExternalLoginRequest
    {
        public string ProviderKey { get; set; }
        public string LoginProvider { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ProviderDisPlayName { get; set; }
        public string ImagePath { get; set; }

    }
}
