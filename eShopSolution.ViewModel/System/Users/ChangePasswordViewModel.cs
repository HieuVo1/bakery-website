using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModel.System.Users
{
   public class ChangePasswordViewModel
    { 
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string CurentPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "password and confirmPassword is not match")]
        public string ConfirmPassword { get; set; }
    }
}
