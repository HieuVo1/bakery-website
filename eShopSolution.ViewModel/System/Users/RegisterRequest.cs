using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.System.Users
{
    public class RegisterRequest
    {
        public DateTime Dob { set; get; }
        public string Email { set; get; }
        public string UserName { set; get; }
        public string Phone { set; get; }
        public string Passwork { set; get; }
        public string ConfirmPasswork { set; get; }

    }
}
