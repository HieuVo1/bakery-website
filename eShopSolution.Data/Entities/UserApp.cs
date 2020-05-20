using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class UserApp :IdentityUser<Guid>
    {
        public DateTime Dob { set; get; }
        public string FullName { set; get; }
        public string ImagePath { get; set; }
        public Guid RoleID { set; get; }
        public RoleApp RoleApp { set; get; }
        public List<Cart> Carts { set; get; }
        public List<Order> Orders { set; get; }
        public List<Blog> Blogs { set; get; }
        public List<Comment> Comments { set; get; }
    }
}
