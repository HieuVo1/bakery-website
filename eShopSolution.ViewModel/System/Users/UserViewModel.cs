using System;

namespace eShopSolution.ViewModel.System.Users
{
    public class UserViewModel
    {
        public DateTime Dob { set; get; }
        public string FullName { set; get; }
        public string ImagePath { get; set; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public string Address { set; get; }
        public string UserName { set; get; }
        public string Role { get; set; }
        public Guid RoleId { get; set; }
        public Guid Id { set; get; }
    }
}
