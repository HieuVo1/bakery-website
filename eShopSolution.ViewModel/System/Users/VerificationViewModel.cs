using System;

namespace eShopSolution.ViewModel.System.Users
{
    public class VerificationViewModel
    {
        
        public string Token { get; set; }
        public string VerificationCode { get; set; }
        public Guid UserId { get; set; }
    }
}
