using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModel.System.Users
{
    public class VerificationViewModel
    {
        
        public string Token { get; set; }
        public string VerificationCode { get; set; }
        public Guid UserId { get; set; }
    }
}
